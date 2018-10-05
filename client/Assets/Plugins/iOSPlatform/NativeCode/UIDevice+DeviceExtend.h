//
//  UIDevice+DeviceExtend.h
//  Unity-iPhone
//
//  Created by LiK on 12-5-29.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface UIDevice (DeviceExtend)

- (NSString *)uniqueGlobalDeviceIdentifier;
- (NSString *)macaddress;

@end
