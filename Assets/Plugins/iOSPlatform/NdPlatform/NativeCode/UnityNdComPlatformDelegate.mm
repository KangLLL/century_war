//
//  UnityNdComPlatformDelegate.m
//  Untitled
//
//  Created by Sie Kensou on 12-8-2.
//  Copyright 2012 NetDragon WebSoft Inc. All rights reserved.
//

#import "UnityNdComPlatformDelegate.h"
#import <NdComPlatform/NdComPlatform.h>
#import <NdComPlatform/NdCPNotifications.h>
#include "U3dNdComPlatformInterface.h"
#import "CommonUtility.h"

#pragma mark UnityNdComPlatformDelegate

static UnityNdComPlatformDelegate *_sigleton;

@implementation UnityNdComPlatformDelegate

+ (UnityNdComPlatformDelegate *)sharedDelegate
{
    if(_sigleton == NULL){
        _sigleton = [[UnityNdComPlatformDelegate alloc] init];
    }
    return _sigleton;
}

- (void)InitializePlatform:(int)appID key:(NSString *)appKey
{
    NdInitConfigure *cfg = [[[NdInitConfigure alloc] init] autorelease];
    cfg.appid = appID;
    cfg.appKey = appKey;
    [[NdComPlatform defaultPlatform] NdInit:cfg];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(SNSInitResult:) name:(NSString *)kNdCPInitDidFinishNotification object:nil];
}

- (void)addNdComPlatformObservers
{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(loginFinished:) name:kNdCPLoginNotification object:nil];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(buyFinished:) name:kNdCPBuyResultNotification object:nil];    
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(leavePlatform:) name:kNdCPLeavePlatformNotification object:nil];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(sessionInvalid:) name:kNdCPSessionInvalidNotification object:nil];
}

- (void)SNSInitResult:(NSNotification *)notify
{
    [CommonUtility sendU3dMessage:@"PlatformInitialSuccess" param:nil];
    /*[NSDictionary dictionaryWithObjectsAndKeys:@"yes",@"result",@"no",@"ppt",@"feifei",@"kkk",nil]];
                                                                   */
    [self addNdComPlatformObservers];
}

- (void)loginFinished:(NSNotification *)aNotify
{
/**  登录接口通知 **/ 
	NSDictionary *dict = [aNotify userInfo];
    BOOL success = [[dict objectForKey:@"result"] boolValue];
    if(success == YES)
    {
        [CommonUtility sendU3dMessage:@"NdPlayerLoginFinished" param:nil];
    }
    else
    {
        [CommonUtility sendU3dMessage:@"NdPlayerLoginFinished" param:
                    [NSDictionary dictionaryWithObjectsAndKeys:[dict objectForKey:@"result"], @"result", [dict objectForKey:@"error"], @"error", nil]];
    }
}

- (void)buyFinished:(NSNotification *)aNotify
{
    NSDictionary *dict = [aNotify userInfo];
    NSMutableDictionary *paramDict = [NSMutableDictionary dictionaryWithObjectsAndKeys:[dict objectForKey:@"result"], @"result", [dict objectForKey:@"error"], @"error", nil];
    
    NdBuyInfo *buyInfo = [dict objectForKey:@"buyInfo"];

    if (buyInfo)
    {
        [paramDict setValue:buyInfo.cooOrderSerial forKey:@"CooOrderSerial"];
        [paramDict setValue:buyInfo.productId forKey:@"ProductId"];
        [paramDict setValue:buyInfo.productName forKey:@"ProductName"];
        [paramDict setValue:[NSNumber numberWithFloat:buyInfo.productPrice] forKey:@"ProductPrice"];
        [paramDict setValue:[NSNumber numberWithFloat:buyInfo.productOrignalPrice] forKey:@"ProductOrignalPrice"];
        [paramDict setValue:[NSNumber numberWithInt:buyInfo.productCount] forKey:@"ProductCount"];
        [paramDict setValue:buyInfo.payDescription forKey:@"PayDescription"];

    }
    
/**  支付结果通知 详情查看文档 **/  
    [CommonUtility sendU3dMessage:@"NdBuyFinished" param:paramDict];
}

- (void)leavePlatform:(NSNotification *)aNotify
{

/**  关闭91平台通知，包括用户取消登录，取消支付等通知，详情查看文档 **/  
    [CommonUtility sendU3dMessage:@"NdPlatformLeaved" param:nil];
}

- (void)sessionInvalid:(NSNotification *)aNotify
{

/**  登录会话超时，或失效通知 **/ 
    [CommonUtility sendU3dMessage:@"kNdCPSessionInvalidNotification" param:nil];
}

/*
- (void)appVersionUpdateDidFinish:(ND_APP_UPDATE_RESULT)updateResult
{   
*/
/**  版本升级回调 **/
/*
    [CommonUtility sendU3dMessage:@"appVersionUpdateDidFinish" param:[NSDictionary dictionaryWithObject:[NSNumber numberWithInt:updateResult] forKey:@"updateResult"]];
}
*/
- (void)searchPayResultInfoDidFinish:(int)error bSuccess:(BOOL)bSuccess buyInfo:(NdBuyInfo*)buyInfo
{
    NSMutableDictionary *dict = [NSMutableDictionary dictionary];
    [dict setValue:[NSNumber numberWithInt:error] forKey:@"error"];
    [dict setValue:[NSNumber numberWithBool:bSuccess]  forKey:@"success"];

    if (buyInfo)
    {
        [dict setValue:buyInfo.cooOrderSerial forKey:@"CooOrderSerial"];
        [dict setValue:buyInfo.productId forKey:@"ProductId"];
        [dict setValue:buyInfo.productName forKey:@"ProductName"];
        [dict setValue:[NSNumber numberWithFloat:buyInfo.productPrice] forKey:@"ProductPrice"];
        [dict setValue:[NSNumber numberWithFloat:buyInfo.productOrignalPrice] forKey:@"ProductOrignalPrice"];
        [dict setValue:[NSNumber numberWithInt:buyInfo.productCount] forKey:@"ProductCount"];
        [dict setValue:buyInfo.payDescription forKey:@"PayDescription"];
    }

/**  订单查询回调 **/  
    [CommonUtility sendU3dMessage:@"searchPayResultInfoDidFinish" param:dict];
}
@end



#if defined(__cplusplus)
extern "C" {
#endif

#pragma mark c apis
void U3dInitializeNdPlatform(int appId, const char *szAppKey)
{
    [[UnityNdComPlatformDelegate sharedDelegate] InitializePlatform:appId key:[CommonUtility getStringParam:szAppKey]];
}

int U3dNdLogin(int nFlag)
{
    return [[NdComPlatform defaultPlatform] NdLogin:nFlag];
}

long U3dNdGetUin()
{
    NSString *strUin = [[NdComPlatform defaultPlatform] loginUin];
    if ([strUin length] == 0)
        return -1;
    return [strUin longLongValue];
}
    
void U3dNdLogout(int nFlag)
{
    [[NdComPlatform defaultPlatform] NdLogout:nFlag];
}    
    
const char *U3dNdGetSession()
{
    return [[[NdComPlatform defaultPlatform] sessionId] UTF8String];
}
    
void U3dNdSetDebugMode(int nFlag)
{
    [[NdComPlatform defaultPlatform] NdSetDebugMode:nFlag];
}

void U3dNdSetAutoRotation(bool bFlag)
{
    [[NdComPlatform defaultPlatform] NdSetAutoRotation:bFlag];
}
    
void U3dNdSetScreenOrientation(int orientation)
{
    UIInterfaceOrientation orient = UIInterfaceOrientationPortrait;
    switch (orientation) {
        case 0:
            orient = UIInterfaceOrientationPortrait;
            break;
        case 1:
            orient = UIInterfaceOrientationLandscapeLeft;
            break;
        case 2:
            orient = UIInterfaceOrientationPortraitUpsideDown;
            break;
        case 3:
            orient = UIInterfaceOrientationLandscapeRight;
            break;
        default:
            break;
    }
    [[NdComPlatform defaultPlatform] NdSetScreenOrientation:orient];
}

bool U3dNdIsLogined()
{
    return [[NdComPlatform defaultPlatform] isLogined];
}

const char *U3dNdGetNickname()
{
    return [[[NdComPlatform defaultPlatform] nickName] UTF8String];
}
    
int U3dNdEnterRecharge(int nFlag, const char *content)
{
    return 1;
    /*
    return [[NdComPlatform defaultPlatform] NdEnterRecharge:nFlag content:[CommonUtility getStringParam:content]];
    */
}

void U3dNdEnterPlatform(int nFlag)
{
    [[NdComPlatform defaultPlatform] NdEnterPlatform:nFlag];
}

int U3dNdUserFeedBack()
{
    return [[NdComPlatform defaultPlatform] NdUserFeedBack];
}

void U3dNdShowToolBar()
{
    [[NdComPlatform defaultPlatform] NdShowToolBar:NdToolBarAtTopLeft];
}
    
void U3dNdHideToolBar()
{
    [[NdComPlatform defaultPlatform] NdHideToolBar];
}
    
int U3dNdCheckUpdate(int nFlag)
{
    return 1;
    /*
    return [[NdComPlatform defaultPlatform] NdAppVersionUpdate:nFlag delegate:[UnityNdComPlatformDelegate class]];
    */
}

    
int U3dNdSearchPayResult(const char *cooOrderSerial)
{
    NSString *order = nil;
    if (strlen(cooOrderSerial) != 0)
        order = [CommonUtility getStringParam:cooOrderSerial];
    return [[NdComPlatform defaultPlatform] NdSearchPayResultInfo:order delegate:[UnityNdComPlatformDelegate class]];
}
 
int U3dNdUniPay(const char *cooOrderSerial, const char *productId, const char *productName,
                       double productPrice, double productOriginalPrice, int productCount, const char *payDescription)
{
    NdBuyInfo *buyInfo = [[[NdBuyInfo alloc] init] autorelease];
    buyInfo.cooOrderSerial = [CommonUtility getStringParam:cooOrderSerial];
    buyInfo.productId = [CommonUtility getStringParam:productId];
    buyInfo.productName = [CommonUtility getStringParam:productName];
    buyInfo.productPrice = productPrice;
    buyInfo.productOrignalPrice = productOriginalPrice;
    buyInfo.productCount = productCount;
    buyInfo.payDescription = [CommonUtility getStringParam:payDescription];

    return [[NdComPlatform defaultPlatform] NdUniPay:buyInfo];
}

int U3dNdUniPayAsyn(const char *cooOrderSerial, const char *productId, const char *productName,
                           double productPrice, double productOriginalPrice, int productCount, const char *payDescription)
{
    NdBuyInfo *buyInfo = [[[NdBuyInfo alloc] init] autorelease];
    buyInfo.cooOrderSerial = [CommonUtility getStringParam:cooOrderSerial];
    buyInfo.productId = [CommonUtility getStringParam:productId];
    buyInfo.productName = [CommonUtility getStringParam:productName];
    buyInfo.productPrice = productPrice;
    buyInfo.productOrignalPrice = productOriginalPrice;
    buyInfo.productCount = productCount;
    buyInfo.payDescription = [CommonUtility getStringParam:payDescription];
    
    return [[NdComPlatform defaultPlatform] NdUniPayAsyn:buyInfo];    
}

int U3dNdUniPayForCoin(const char *cooOrderSerial, int needPayCoins, const char *payDescription)
{
    return [[NdComPlatform defaultPlatform] NdUniPayForCoin:[CommonUtility getStringParam:cooOrderSerial] needPayCoins:needPayCoins payDescription:[CommonUtility getStringParam:payDescription]];
}
    
#if defined(__cplusplus)
}
#endif
