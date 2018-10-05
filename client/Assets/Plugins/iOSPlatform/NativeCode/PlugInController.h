//
//  PlugInController.h
//  PlugInTextField
//
//  Created by LiK on 12-5-18.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface PlugInController : NSObject{
    UIView *_view;
    int _orientation;
}

- (id)initWithPositionX:(float)x positionY:(float)y width:(float)width height:(float)height;
- (void)destory;
- (void)setPositionX:(float)x positionY:(float)y;

- (void)getOrientation;
- (void)adjustViewPositionAccordingToOritation;
- (void)adjustViewRotationAccordingToOritation;
- (void)flipAxisY;

- (BOOL)getFocus;
- (void)setFocus:(BOOL)focused;
@end
