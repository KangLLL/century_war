/*
 *  U3dNdComPlatformInterface.h
 *  Untitled
 *
 *  Created by Sie Kensou on 12-8-2.
 *  Copyright 2012 NetDragon WebSoft Inc. All rights reserved.
 *
 */

#ifndef _U3D_NDCOMPLATFORM_INTERFACE_H_
#define _U3D_NDCOMPLATFORM_INTERFACE_H_

/*****************************************************************************************
	本接口是91移动开放平台SDK,针对Unity3D接入相关API，进行的封装。具体API的参数，用法等请参照
	SDK中的相关文档描述。Unity3D平台的API，去除U3d前缀后，同Native API命名一致。
	在消息通知中，采用的key1=value1&key2=value2的格式封装多个参数为字符串，开发者需要按照这种格式
	进行解析。详见【sendU3dMessage】等相关消息事件源码
	
*****************************************************************************************/

#if defined(__cplusplus)
extern "C" { 
#endif

#pragma mark interface

/**  初始化平台 **/
    extern void U3dInitializeNdPlatform(int appId, const char *szAppKey);
    
/**  设置应用程序屏幕方向 **/ 
    extern void U3dNdSetScreenOrientation(int orientation);//orientation 0:竖屏 1:横屏朝左 2:竖屏朝下 3:横屏朝右
   
/**  是否已经登录 **/     
    extern bool U3dNdIsLogined();  

/**  登录接口 **/ 
    extern int U3dNdLogin(int nFlag);   //nFlag预留，传0即可
    
/**  登出接口 **/     
    extern void U3dNdLogout(int nFlag); //nFlag 标识（按位标识）0,表示注销；1，表示注销，并清除自动登录
    
/**  设置测试模式 （注意：正式发布时，需要注释掉给调用  **/     
    extern void U3dNdSetDebugMode(int nFlag); //nFlag 预留，默认为0
    
/**  获取登录用户ID **/     
    extern long U3dNdGetUin();

/**  获取登录用户昵称 **/
    extern const char *U3dNdGetNickname();

/**  获取登录会话ID **/
    extern const char *U3dNdGetSession();
    
/**  进入平台个人中心 **/    
    extern void U3dNdEnterPlatform(int nFlag);  //nFlag预留， 传0即可
    
/**  进入91豆充值中心 **/    
    extern int U3dNdEnterRecharge(int nFlag, const char *content); //nFlag、content为预留参数；目前传0和空即可
    
/**  用户反馈 **/    
    extern int U3dNdUserFeedBack();
    
/**  核查版本更新 **/    
    extern int U3dNdCheckUpdate(int nFlag); //nFlag预留， 传0即可
    
/**  验证支付订单是否有效 **/    
    extern int U3dNdSearchPayResult(const char *cooOrderSerial);    
    
/**  同步支付API （详情查看文档） **/    
    extern int U3dNdUniPay(const char *cooOrderSerial, const char *productId, const char *productName,
                           double productPrice, double productOriginalPrice, int productCount, const char *payDescription);

/**   异步支付API （详情查看文档） **/
    extern int U3dNdUniPayAsyn(const char *cooOrderSerial, const char *productId, const char *productName,
                           double productPrice, double productOriginalPrice, int productCount, const char *payDescription);

/**   代币支付API （详情查看文档） **/
    extern int U3dNdUniPayForCoin(const char *cooOrderSerial, int needPayCoins, const char *payDescription);

#if defined(__cplusplus)
}
#endif
        
    
#endif

