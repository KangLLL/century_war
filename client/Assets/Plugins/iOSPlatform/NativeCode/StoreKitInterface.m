//
//  StoreKitInterface.m
//  Unity-iPhone
//
//  Created by LiK on 12-5-22.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//


#import "StoreKitHelper.h"
#import "CommonUtility.h"
#import "JSONKit.h"

#define PRODUCT_ID                          @"ProductID"
#define PRODUCT_LOCALE_DESCRIPTION          @"ProductLocaleDesciption"
#define PRODUCT_LOCALE_TITLE                @"ProductLocaleTitle"
#define PRODUCT_LOCALE_CONCURRENCY_SYMBOL   @"ProductLocaleConcurrencySymbol"
#define PRODUCT_LOCALE_PRICE                @"ProductLocalePrice"

bool _CanMakePayments()
{
    return [[StoreKitHelper sharedHelper] canMakePayments];
}

void _RequestProducts(const char *productsID,bool isNeedRefresh)
{
    NSString *identifiers = [CommonUtility getStringParam:productsID];
    NSArray *products = [identifiers componentsSeparatedByString:@" "];
    [[StoreKitHelper sharedHelper] requestProducts:products isNeedRefresh:isNeedRefresh];
}

bool _IsProductAvailable(const char *productID)
{
    NSString *identifierString = [CommonUtility getStringParam:productID];
    return [[StoreKitHelper sharedHelper] isProductAvailable:identifierString];
}

int _GetRequestState()
{
    return (int)[[StoreKitHelper sharedHelper] getRequestState];
}

char *_GetUnconfirmedTransactionID()
{
    NSString *transactionID = [[StoreKitHelper sharedHelper] getUnconfirmedTransactionID];
    return [CommonUtility makeStringCopy:transactionID];
}

char *_GetUnconfirmedProductID()
{
    NSString *productID = [[StoreKitHelper sharedHelper] getUnconfirmedProductID];
    return [CommonUtility makeStringCopy:productID];
}

void _PurchaseProduct(const char *productID,int quantity)
{
    NSString *identifiersString = [CommonUtility getStringParam:productID];
    [[StoreKitHelper sharedHelper] purchaseProduct:identifiersString quantity:quantity];
}

int _GetPurchaseState()
{
    return (int)[[StoreKitHelper sharedHelper] getPurchaseState];
}

char *_GetTransactionID(const char *productID, int quantity)
{
    NSString *productIdentifier = [CommonUtility getStringParam:productID];
    NSString *transactionID = [[StoreKitHelper sharedHelper] getTransactionID:productIdentifier quantity:quantity];
    return [CommonUtility makeStringCopy:transactionID];
}

char *_GetTransactionReceipt(const char *transactionID)
{
    NSString *transactionIdentifier = [CommonUtility getStringParam:transactionID];
    NSString *receipt = [[StoreKitHelper sharedHelper] getTransactionReceipt:transactionIdentifier];
    return [CommonUtility makeStringCopy:receipt];
}

int _ValidateTransactionReceipt( const char *transactionID, const char *receipt, bool isTest)
{
    NSString *transactionIdentifier = [CommonUtility getStringParam:transactionID];
    NSString *receiptString = [CommonUtility getStringParam:receipt];
    int validationID = [[StoreKitHelper sharedHelper] validateTransactionReceipt:transactionIdentifier receipt:receiptString isTest:isTest];
    return validationID;
}

int _GetTransactionValidationState(int validationID)
{
    return (int)[[StoreKitHelper sharedHelper] getTransactionValidationState:validationID];
}

char *_ComfirmTransaction(const char *transactionID)
{
    NSString *transactionIdentifier = [CommonUtility getStringParam:transactionID];
    SKProduct *product = [[StoreKitHelper sharedHelper] comfirmTransaction:transactionIdentifier];
    if(product == nil){
        return [CommonUtility makeStringCopy:nil];
    }
    NSDictionary *dictionary = [NSDictionary dictionaryWithObjectsAndKeys:product.productIdentifier, PRODUCT_ID, product.localizedTitle, PRODUCT_LOCALE_TITLE, product.localizedDescription, PRODUCT_LOCALE_DESCRIPTION, [product.price stringValue], PRODUCT_LOCALE_PRICE,[product.priceLocale objectForKey:NSLocaleCurrencySymbol], PRODUCT_LOCALE_CONCURRENCY_SYMBOL,nil];
    NSString *result = [dictionary JSONString];
    return [CommonUtility makeStringCopy:result];
}

char *_GetValidProductsInformation()
{
    NSDictionary *products = [[StoreKitHelper sharedHelper] getValidProducts];
    if(products == nil || [products count] == 0){
        return [CommonUtility makeStringCopy:nil];
    }
    NSMutableArray *resultProducts = [NSMutableArray array];
    for (NSString *identifier in [products allKeys]) {
        SKProduct *product = (SKProduct *)[products objectForKey:identifier];
        NSDictionary *dictionary = [NSDictionary dictionaryWithObjectsAndKeys:identifier, PRODUCT_ID, product.localizedTitle, PRODUCT_LOCALE_TITLE, product.localizedDescription, PRODUCT_LOCALE_DESCRIPTION, [product.price stringValue], PRODUCT_LOCALE_PRICE,[product.priceLocale objectForKey:NSLocaleCurrencySymbol], PRODUCT_LOCALE_CONCURRENCY_SYMBOL,nil];
        [resultProducts addObject:dictionary];
    }
    NSString *result = [resultProducts JSONString];
    return [CommonUtility makeStringCopy:result];
}