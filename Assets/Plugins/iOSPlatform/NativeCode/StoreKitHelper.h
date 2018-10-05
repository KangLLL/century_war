//
//  StoreKitHelper.h
//  Unity-iPhone
//
//  Created by LiK on 12-5-22.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import "StoreKitReceiptRequest.h"
#import "ValidatingTransaction.h"

typedef enum{
    PurchaseNotStart,
    PurchaseStartPurchase,
    PurchasePurchasing,
    PurchaseSuccess,
    PurchaseFail,
    PurchaseCancel
}PurchaseState;

typedef enum{
    RequestNotStart,
	RequestRequesting,
	RequestSuccess,
	RequestFail
}RequestState;

@interface StoreKitHelper : NSObject<SKProductsRequestDelegate,SKPaymentTransactionObserver,StoreKitReceiptRequestDelegate>{
    PurchaseState _purchaseState;
    RequestState _requestState;
    int _quantity;
    NSString *_productIdentifer;
    NSMutableArray *_transactionArray;
    NSMutableDictionary *_productsDict;
    NSMutableDictionary *_validateTransactionDict;
}

+ (StoreKitHelper *)sharedHelper;

- (BOOL)canMakePayments;
- (void)purchaseProduct:(NSString*)productIdentifier quantity:(int)quantity;
- (PurchaseState)getPurchaseState;

- (NSString *)getTransactionID:(NSString *)productID quantity:(int)quantity;
- (NSString *)getTransactionReceipt:(NSString *)transactionID;
- (int)validateTransactionReceipt:(NSString *)transactionID receipt:(NSString *)receipt isTest:(BOOL)isTest;
- (ValidateState)getTransactionValidationState:(int)validatingID;
- (SKProduct *)comfirmTransaction:(NSString *)transactionID;


- (BOOL)isProductAvailable:(NSString *)productID;
- (void)requestProducts:(NSArray *)productsID isNeedRefresh:(BOOL)refreshFlag;
- (RequestState)getRequestState;
- (NSString *)getUnconfirmedTransactionID;
- (NSString *)getUnconfirmedProductID;

- (NSDictionary *)getValidProducts;
@end
