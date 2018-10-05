//
//  TextFieldController.h
//  PlugInTextField
//
//  Created by LiK on 12-5-11.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PlugInController.h"

@interface TextFieldController : PlugInController<UITextFieldDelegate>{
    UITextField *_textField;
    int maxLength;
}
- (id)initWithText:(NSString *)text positionX:(float)x positionY:(float)y red:(float)r green:(float)g blue:(float)b alpha:(float)a fontSize:(float)size maxLength:(int)length alignment:(int)align width:(float)width;
- (void)destroyTextField;
- (void)setTextFieldFontSize:(float)size;
- (void)setTextFieldWidth:(float)width;
- (NSString *)getTextFieldText;
- (void)setTextFieldText:(NSString *)text;
- (void)setTextFieldFontColor:(float)r green:(float)g blue:(float)b alpha:(float)a;
- (void)setTextFieldFontAlignment:(int)alignment;
- (void)setTextFieldKeyboardType:(int)keyboardType;
- (void)setTextFieldMaxLength:(int)length;
@end
