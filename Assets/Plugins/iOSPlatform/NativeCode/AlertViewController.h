//
//  AlertViewController.h
//  222
//
//  Created by LiK on 12-12-3.
//  Copyright (c) 2012年 LiK. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface AlertViewController : NSObject<UIAlertViewDelegate>

@property (nonatomic, assign) BOOL alerViewIsShown;

+ (AlertViewController *)sharedController;
- (void)showAlertViewWithTitle:(NSString *)title description:(NSString *)des cancelButtonTitle:(NSString *)cancelTitle;
@end
