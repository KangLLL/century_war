//
//  CommonUtility.h
//  Unity-iPhone
//
//  Created by LiK on 12-5-29.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface CommonUtility : NSObject

+ (char *)makeStringCopy:(NSString *)string;
+ (NSString *)getStringParam:(const char *)param;
+ (NSString *)getDeviceID;
+ (NSString *)getFullPath:(NSString *)filename;

+ (void)sendU3dMessage:(NSString *)messageName param:(NSDictionary *)dict;
@end