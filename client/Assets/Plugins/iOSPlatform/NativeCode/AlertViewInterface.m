//
//  AlertViewInterface.m
//  222
//
//  Created by LiK on 12-12-3.
//  Copyright (c) 2012年 LiK. All rights reserved.
//

#import "AlertViewController.h"
#import "CommonUtility.h"

bool _AlertViewIsShown()
{
    return [AlertViewController sharedController].alerViewIsShown;
}

void _ShowAlertView(const char* title, const char* description, const char* cancelTitle)
{
    NSString *titleString = [CommonUtility getStringParam:title];
    NSString *descriptionString = [CommonUtility getStringParam:description];
    NSString *canelTitleString = [CommonUtility getStringParam:cancelTitle];
    
    [[AlertViewController sharedController] showAlertViewWithTitle:titleString description:descriptionString cancelButtonTitle:canelTitleString];
}