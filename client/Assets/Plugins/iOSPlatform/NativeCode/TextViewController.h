//
//  TextViewController.h
//  PlugInTextField
//
//  Created by LiK on 12-5-18.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PlugInController.h"

@interface TextViewController : PlugInController<UITextViewDelegate>{
    UITextView *_textView;
    int maxLength;
}

- (id)initWithText:(NSString *)text positionX:(float)x positionY:(float)y red:(float)r green:(float)g blue:(float)b alpha:(float)a fontSize:(float)size maxLength:(int)length alignment:(int)align width:(float)width height:(float)height;
- (void)destoryTextView;
- (void)setTextViewFontSize:(float)size;
- (void)setTextViewWidth:(float)width;
- (void)setTextViewHeight:(float)height;
- (NSString *)getTextViewText;
- (void)setTextViewText:(NSString *)text;
- (void)setTextViewFontColor:(float)r green:(float)g blue:(float)b alpha:(float)a;
- (void)setTextViewFontAlignment:(int)alignment;
- (void)setTextViewKeyboardType:(int)keyboardType;
- (void)setTextViewMaxLength:(int)length;

@end
