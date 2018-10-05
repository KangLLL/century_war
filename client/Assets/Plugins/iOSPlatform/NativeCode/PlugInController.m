//
//  PlugInController.m
//  PlugInTextField
//
//  Created by LiK on 12-5-18.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "PlugInController.h"

@implementation PlugInController

- (id)initWithPositionX:(float)x positionY:(float)y width:(float)width height:(float)height
{
    if(self = [super init]){
        _view = [[UIView alloc] initWithFrame:CGRectMake(0, 0, width, height)];
        [[UIApplication sharedApplication].keyWindow addSubview:_view];
        [self getOrientation];
        [self setPositionX:x positionY:y];
        [self adjustViewRotationAccordingToOritation];
        [_view release];
    }
    return self;
}

- (void)destory
{
    [_view removeFromSuperview];
}

- (void)getOrientation
{
    NSArray *supportedOrientations = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"UISupportedInterfaceOrientations"];
    NSString *orientation = [supportedOrientations objectAtIndex:0];
    if([orientation compare:@"UIInterfaceOrientationPortrait"] == NSOrderedSame){
        _orientation = UIDeviceOrientationPortrait;
    }
    else if([orientation compare:@"UIInterfaceOrientationPortraitUpsideDown"] == NSOrderedSame){
        _orientation = UIDeviceOrientationPortraitUpsideDown;
    }
    else if([orientation compare:@"UIInterfaceOrientationLandscapeLeft"] == NSOrderedSame){
        _orientation = UIDeviceOrientationLandscapeLeft;
    }
    else {
        _orientation = UIDeviceOrientationLandscapeRight;
    }
}

- (void)setPositionX:(float)x positionY:(float)y
{
    _view.center = CGPointMake(x, y);
    [self adjustViewPositionAccordingToOritation];
    [self flipAxisY];
}

- (void)adjustViewPositionAccordingToOritation
{
    float originalX = _view.center.x;
    float originalY = _view.center.y;
    float windowWidth = [UIApplication sharedApplication].keyWindow.frame.size.width;
    float windowHeight = [UIApplication sharedApplication].keyWindow.frame.size.height;
    float destX = originalX;
    float destY = originalY;
    
    switch (_orientation) {
        case UIDeviceOrientationPortraitUpsideDown:
            destY = windowHeight - originalY;
            break;
        case UIDeviceOrientationLandscapeLeft:
            destX = originalY;
            destY = windowHeight - originalX;
            break;
        case UIDeviceOrientationLandscapeRight:
            destX = windowWidth - originalY;
            destY = originalX;
            break;
        default:
            break;
    }
    
    _view.center = CGPointMake(destX, destY);
}

- (void)adjustViewRotationAccordingToOritation
{
    switch (_orientation) {
        case UIDeviceOrientationPortraitUpsideDown:
            _view.transform = CGAffineTransformMakeRotation(M_PI);
            [[UIApplication sharedApplication] setStatusBarOrientation:UIInterfaceOrientationPortraitUpsideDown animated:NO];
            break;
        case UIDeviceOrientationLandscapeLeft:
            _view.transform = CGAffineTransformMakeRotation(-M_PI_2);
            [[UIApplication sharedApplication] setStatusBarOrientation:UIInterfaceOrientationLandscapeLeft animated:NO];
            break;
        case UIDeviceOrientationLandscapeRight:
            _view.transform = CGAffineTransformMakeRotation(M_PI_2);
            [[UIApplication sharedApplication] setStatusBarOrientation:UIInterfaceOrientationLandscapeRight animated:NO];
            break;
        default:
            break;
    }
}

- (void)flipAxisY
{
    float originalX = _view.center.x;
    float originalY = _view.center.y;
    float windowWidth = [UIApplication sharedApplication].keyWindow.frame.size.width;
    float windowHeight = [UIApplication sharedApplication].keyWindow.frame.size.height;
    float destX = originalX;
    float destY = originalY;
    switch (_orientation) {
        case UIDeviceOrientationPortrait:
        case UIDeviceOrientationPortraitUpsideDown:
            destY = windowHeight - originalY;
            break;
        case UIDeviceOrientationLandscapeLeft:
        case UIDeviceOrientationLandscapeRight:
            destX = windowWidth - originalX;
            break;
        default:
            break;
    }
    _view.center = CGPointMake(destX, destY);
}

- (BOOL)getFocus
{
    return [[_view.subviews objectAtIndex:0] isFirstResponder];
}

- (void)setFocus:(BOOL)focused
{
    if(focused){
        [[_view.subviews objectAtIndex:0] becomeFirstResponder];
    }
    else{
        [[_view.subviews objectAtIndex:0] resignFirstResponder];
    }
}

@end
