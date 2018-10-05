//
//  StoreKitHelper.m
//  Unity-iPhone
//
//  Created by LiK on 12-5-22.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "StoreKitHelper.h"
#import "NSDataBase64.h"

#define StringNullOrEmpty( _x_ ) ( _x_ == nil ) ? YES : [ _x_ length ] == 0

static StoreKitHelper *_sigletonHelper;

@interface StoreKitHelper()

- (void)transactionFail:(SKPaymentTransaction *)transaction;
- (void)transactionSuccess:(SKPaymentTransaction *)transaction;

@end

@implementation StoreKitHelper

#pragma mark - Life Cycle

+ (StoreKitHelper *)sharedHelper
{
    if(_sigletonHelper == nil){
        _sigletonHelper = [[StoreKitHelper alloc] init];
    }
    return _sigletonHelper;
}

- (id)init
{
    if(self = [super init])
	{
		// we weak link and check for existance of StoreKit here
#if !TARGET_OS_IPHONE
		Class cl = NSClassFromString( @"SKPaymentQueue" );
		if( !cl )
			return nil;
#endif
		// Listen to transaction changes
		[[SKPaymentQueue defaultQueue] addTransactionObserver:self];
        
        _transactionArray = [[NSMutableArray alloc] init];
        _productsDict = [[NSMutableDictionary alloc] init];
	}
	return self;
}

- (void)dealloc
{
    [_transactionArray release];
    [_productsDict release];
    [super dealloc];
}
#pragma mark - Public Methods

- (BOOL)canMakePayments
{
    return [SKPaymentQueue canMakePayments];
}

- (NSDictionary *)getValidProducts
{
    if(_requestState == RequestRequesting){
        return nil;
    }
    return _productsDict;
}

- (NSString *)getUnconfirmedTransactionID
{
    if([_transactionArray count] > 0){
        SKPaymentTransaction *transaction = (SKPaymentTransaction *)[_transactionArray objectAtIndex:0];
        return transaction.transactionIdentifier;
    }
    else{
        return nil;
    }
}

- (NSString *)getUnconfirmedProductID
{
    if([_transactionArray count] > 0){
        SKPaymentTransaction *transaction = (SKPaymentTransaction *)[_transactionArray objectAtIndex:0];
        return transaction.payment.productIdentifier;
    }
    else{
        return nil;
    }
}

- (BOOL)isProductAvailable:(NSString *)productID
{
    return [[_productsDict allKeys] containsObject:productID];
}

- (void)requestProducts:(NSArray *)productsID isNeedRefresh:(BOOL)refreshFlag
{
    if(productsID != nil){
        if(_requestState != RequestRequesting){
            NSMutableSet *identifiers = [NSMutableSet set];
            if(refreshFlag){
                [_productsDict removeAllObjects];
                [identifiers addObjectsFromArray:productsID];
            }
            else{
                for (NSString * productID in productsID) {
                    if(![self isProductAvailable:productID]){
                        [identifiers addObject:productID];
                    }
                }
            }
            if([identifiers count] == 0){
                _requestState = RequestSuccess;
            }
            else{
                _requestState = RequestRequesting;
                SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:identifiers];
                request.delegate = self;
                [request start];
            }
        }
    }
    else{
        _requestState = RequestFail;
    }
}

- (void)purchaseProduct:(NSString *)productIdentifier quantity:(int)quantity
{
    if(!StringNullOrEmpty( productIdentifier )){
        if([self isProductAvailable:productIdentifier] && _requestState != RequestRequesting
           &&_purchaseState != PurchasePurchasing && _purchaseState != PurchaseStartPurchase){
            _productIdentifer = productIdentifier;
            _quantity = quantity;
            
            SKProduct *product = [_productsDict objectForKey:productIdentifier];
           
            _purchaseState = PurchaseStartPurchase;
            SKMutablePayment *payment = [SKMutablePayment paymentWithProduct:product];
            payment.quantity = _quantity;
            [[SKPaymentQueue defaultQueue] addPayment:payment];
        }
        else {
                _purchaseState = PurchaseFail;
        }
    }
    else{
        _purchaseState = PurchaseFail;
    }
}

- (RequestState)getRequestState
{
    return _requestState;
}

- (PurchaseState)getPurchaseState
{
    return _purchaseState;
}

- (NSString *)getTransactionID:(NSString *)productID quantity:(int)quantity
{
    if(!StringNullOrEmpty( productID )){
        NSPredicate *predicate = [NSPredicate predicateWithFormat:@"payment.productIdentifier LIKE %@ AND payment.quantity = %d",productID,quantity];
        SKPaymentTransaction *transaction = [[_transactionArray filteredArrayUsingPredicate:predicate] lastObject];
        NSString *result = transaction == nil ? nil : transaction.transactionIdentifier;
        return result;
    }
    return nil;
}

- (NSString *)getTransactionReceipt:(NSString *)transactionID
{
    if(!StringNullOrEmpty( transactionID )){
        NSPredicate *predicate = [NSPredicate predicateWithFormat:@"transactionIdentifier LIKE %@",transactionID];
        SKPaymentTransaction *transaction = [[_transactionArray filteredArrayUsingPredicate:predicate] lastObject];
        NSString *result = transaction == nil ? nil : [transaction.transactionReceipt base64Encoding];
        return result;
    }
    return nil;
}

- (int)validateTransactionReceipt:(NSString *)transactionID receipt:(NSString *)receipt isTest:(BOOL)isTest
{
    if(!StringNullOrEmpty(transactionID) && !StringNullOrEmpty(receipt)){
        NSPredicate *predicate = [NSPredicate predicateWithFormat:@"transactionIdentifier LIKE %@",transactionID];
        SKPaymentTransaction *transaction = [[_transactionArray filteredArrayUsingPredicate:predicate] lastObject];
        if(transaction != nil){
            if(_validateTransactionDict != nil && [_validateTransactionDict count] > 0){
                for (NSString *validatingKey in [_validateTransactionDict allKeys]) {
                    ValidatingTransaction *validatingTransaction = [_validateTransactionDict objectForKey:validatingKey];
                    if([validatingTransaction.transactionID isEqualToString:transactionID])
                        return [validatingKey intValue];
                }
            }
    
            StoreKitReceiptRequest *request = [[StoreKitReceiptRequest alloc] initWithDelegate:self isTest:isTest];
            [request validateReceipt:receipt];
            int validateingID = [request hash];
            NSString *keyString = [NSString stringWithFormat:@"%d",validateingID];
            ValidatingTransaction *validatingTransaction = [[[ValidatingTransaction alloc] init] autorelease];
            validatingTransaction.transactionID = transaction.transactionIdentifier;
            validatingTransaction.state = Validating;
            
            if(_validateTransactionDict == nil){
                _validateTransactionDict = [[NSMutableDictionary alloc] init];
            }
            [_validateTransactionDict setValue:validatingTransaction forKey:keyString];
            return validateingID;
        }
    }
    return 0;
}

- (ValidateState)getTransactionValidationState:(int)validationID
{
    NSString *key = [NSString stringWithFormat:@"%d",validationID ];
    ValidatingTransaction *validatingTransaction = [_validateTransactionDict objectForKey:key];
    if(validatingTransaction != nil){
        ValidateState state = validatingTransaction.state;
        return state;
    }
    return ValidateFail;
}

- (SKProduct *)comfirmTransaction:(NSString *)transactionID
{
    if(!StringNullOrEmpty( transactionID )){
        NSPredicate *predicate = [NSPredicate predicateWithFormat:@"transactionIdentifier LIKE %@",transactionID];
        SKPaymentTransaction *transaction = [[_transactionArray filteredArrayUsingPredicate:predicate] lastObject];
        if(_validateTransactionDict != nil && [_validateTransactionDict count] > 0){
            for (NSString *validatingKey in [_validateTransactionDict allKeys]) {
                ValidatingTransaction *validatingTransaction = [_validateTransactionDict objectForKey:validatingKey];
                if([validatingTransaction.transactionID isEqualToString:transactionID])
                    [_validateTransactionDict removeObjectForKey:validatingKey];
            }
        }
        if(transaction != nil){
            [_transactionArray removeObject:transaction];
            [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
            return [_productsDict objectForKey:transaction.payment.productIdentifier];
        }
    }
    return nil;
}

#pragma mark - SKProducts Request Delegate

- (void)productsRequest:(SKProductsRequest*)request didReceiveResponse:(SKProductsResponse*)response
{
    for (SKProduct *product in response.products)
    {
        [_productsDict setObject:product forKey:product.productIdentifier];
    }
    for (NSString *identifier in response.invalidProductIdentifiers) {
        NSLog(@"%@",identifier);
    }
    if([response.invalidProductIdentifiers count] > 0){
        _requestState = RequestFail;
    }
    else{
        _requestState = RequestSuccess;
    }
}

- (void)request:(SKRequest*)request didFailWithError:(NSError*)error
{
    _requestState = RequestFail;
}

#pragma mark - SKPayment Transaction Observer

- (void)paymentQueue:(SKPaymentQueue*)queue updatedTransactions:(NSArray*)transactions
{
	for( SKPaymentTransaction *transaction in transactions ){
		switch( transaction.transactionState ){
            case SKPaymentTransactionStatePurchasing:{
                _purchaseState = PurchasePurchasing;
                break;
            }
			case SKPaymentTransactionStateFailed:{
				[self transactionFail:transaction];
				break;
            }
			case SKPaymentTransactionStatePurchased:{
                [self transactionSuccess:transaction];
                break;
            }
			case SKPaymentTransactionStateRestored:{
				[self transactionSuccess:transaction.originalTransaction];
				break;
            }
        }
    }
}

#pragma mark - Private Methods

- (void)transactionFail:(SKPaymentTransaction *)transaction
{
    _purchaseState = transaction.error.code == SKErrorPaymentCancelled ? PurchaseCancel : PurchaseFail;
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
}

- (void)transactionSuccess:(SKPaymentTransaction *)transaction
{
    _purchaseState = PurchaseSuccess;
    [_transactionArray addObject:transaction];
    
}

#pragma mark - StoreKitReceiptRequestDelegate

- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request validatedWithStatusCode:(int)statusCode
{
    NSString *key = [NSString stringWithFormat:@"%d",[request hash]];
    if(_validateTransactionDict != nil && [_validateTransactionDict objectForKey:key] != nil){
        ValidateState state = statusCode == 0 ? ValidateSuccess : ValidateFail;
        ValidatingTransaction *validatingTransaction = [_validateTransactionDict objectForKey:key];
        validatingTransaction.state = state;
    }
    [request release];
}

- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request didFailWithError:(NSError*)error
{
    NSString *key = [NSString stringWithFormat:@"%d",[request hash]];
    if(_validateTransactionDict != nil && [_validateTransactionDict objectForKey:key] != nil){
        ValidatingTransaction *validatingTransaction = [_validateTransactionDict objectForKey:key];
        validatingTransaction.state = ValidateFail;
    }
    [request release];
}

- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request validatedWithResponse:(NSString*)response
{
    
}

@end
