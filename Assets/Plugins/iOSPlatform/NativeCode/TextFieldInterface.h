//
//  TextFieldInterface.h
//  PlugInTextField
//
//  Created by LiK on 12-5-10.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface TextFieldInterface : NSObject{
    NSMutableDictionary *textFieldControllerDict;
}
- (int)createTextField:(NSString *)text positionX:(float) x positionY:(float) y red:(float)r green:(float)g blue:(float)b alpha:(float)a fontSize:(float)size maxLength:(int)length alignment:(int)align width:(float)width;
- (void)destoryTextField:(int)key;
- (void)setTextFieldPosition:(int)key positionX:(float)x positionY:(float)y;
- (void)setTextFieldFontSize:(int)key fontSize:(float)size;
- (void)setTextFieldWidth:(int)key width:(float)width;
- (void)setTextFieldFontColor:(int)key red:(float)r green:(float)g blue:(float)b alpha:(float)a;
- (void)setTextFieldFontAlignment:(int)key alignment:(int)alignment;
- (void)setTextFieldKeyboardType:(int)key keyboardType:(int)keyboardType;
- (NSString *)getTextFieldText:(int)key;
- (void)setTextFieldText:(int)key text:(NSString *)newValue;
- (BOOL)getFocus:(int)key;
- (void)setFocus:(int)key newValue:(BOOL)focused;
@end
