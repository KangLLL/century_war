//
//  CommonUtility.m
//  Unity-iPhone
//
//  Created by LiK on 12-5-29.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "CommonUtility.h"
#import "UIDevice+DeviceExtend.h"

@implementation CommonUtility

+ (char *)makeStringCopy:(NSString *)string
{
    if(string != nil){
        return strdup([string UTF8String]);
    }
    return NULL;
}

+ (NSString *)getStringParam:(const char *)param
{
    if(param != NULL)
        return [NSString stringWithUTF8String:param];
    return [NSString stringWithUTF8String:""];
}

+ (NSString *)getDeviceID
{
    return [[UIDevice currentDevice] uniqueGlobalDeviceIdentifier];
}

+ (NSString *)getFullPath:(NSString *)filename
{
    return [[NSBundle mainBundle] pathForResource:filename ofType:nil];
}

+ (void)sendU3dMessage:(NSString *)messageName param:(NSDictionary *)dict
{
    NSString *param = @"";
    NSError *error = nil;
    if([dict count] > 0){
        NSData *data = [NSJSONSerialization dataWithJSONObject:dict options:kNilOptions error:&error];
        if(error == nil){
            param = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
        }
    }
    UnitySendMessage("PluginAdapter", [messageName UTF8String], [param UTF8String]);
}

@end

#if defined(__cplusplus)
extern "C" {
#endif

char* _GetDeviceID()
{
    NSString *deviceID = [CommonUtility getDeviceID];
    return [CommonUtility makeStringCopy:deviceID];
}

char* _GetFullPath(const char* filename)
{
    NSString *fullPath = [CommonUtility getFullPath:[CommonUtility getStringParam:filename]];
    return [CommonUtility makeStringCopy:fullPath];
}

void _Log(const char* str)
{
    NSString *string = [CommonUtility getStringParam:str];
    NSLog(@"%@",string);
}

void _ClearApplicationIconBadge()
{
    [UIApplication sharedApplication].applicationIconBadgeNumber = 0;
}

void _SetCopyBoard(const char* newString)
{
    NSString *string = [CommonUtility getStringParam:newString];
    [UIPasteboard generalPasteboard].string = string;
}

int _GetGameViewChildCount()
{
    UIView *view = (UIView *)[[[UIApplication sharedApplication].keyWindow subviews] objectAtIndex:0];
    int viewCount = [[view subviews] count];
    return viewCount;
}

#if defined(__cplusplus)
}
#endif