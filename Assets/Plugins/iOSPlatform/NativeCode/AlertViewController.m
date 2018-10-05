//
//  AlertViewController.m
//  222
//
//  Created by LiK on 12-12-3.
//  Copyright (c) 2012年 LiK. All rights reserved.
//

#import "AlertViewController.h"

AlertViewController *singletonController;

@implementation AlertViewController
@synthesize alerViewIsShown;

+ (AlertViewController *)sharedController
{
    if(singletonController == nil){
        singletonController = [[AlertViewController alloc] init];
    }
    return singletonController;
}

- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex
{
    self.alerViewIsShown = NO;
}

- (void)showAlertViewWithTitle:(NSString *)title description:(NSString *)des cancelButtonTitle:(NSString *)cancelTitle
{
    UIAlertView *alertView = [[UIAlertView alloc] initWithTitle:title message:des delegate:self cancelButtonTitle:cancelTitle otherButtonTitles:nil];
    [alertView show];
    self.alerViewIsShown = YES;
    [alertView release];
}

@end
