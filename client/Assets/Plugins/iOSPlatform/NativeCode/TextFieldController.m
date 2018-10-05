//
//  TextFieldController.m
//  PlugInTextField
//
//  Created by LiK on 12-5-11.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "TextFieldController.h"
#define FONT_FAMILY_NAME            @"Arial"
#define TEXT_FIELD_DEFAULT_HEIGHT   31

@implementation TextFieldController

- (id)initWithText:(NSString *)text positionX:(float)x positionY:(float)y red:(float)r green:(float)g blue:(float)b alpha:(float)a fontSize:(float)size maxLength:(int)length alignment:(int)align width:(float)width
{
    if(self = [super initWithPositionX:x positionY:y width:width height:TEXT_FIELD_DEFAULT_HEIGHT]){
        _textField = [[UITextField alloc] initWithFrame:CGRectMake(0, 0, width, TEXT_FIELD_DEFAULT_HEIGHT)];
        [_textField setBorderStyle:UITextBorderStyleNone];
        [_view addSubview:_textField];
        [self setTextFieldText:text];
        [self setTextFieldFontColor:r green:g blue:b alpha:a];
        [self setTextFieldFontSize:size];
        maxLength = length;
        [self setTextFieldFontAlignment:align];
        _textField.delegate = self;
        [_textField release];
    }
    return self;
}

- (void)destroyTextField
{
    _textField.delegate = nil;
    [_textField removeFromSuperview];
    [super destory];
}
#pragma mark - Public Methods
- (void)setTextFieldFontSize:(float)size
{
    [_textField setFont:[UIFont fontWithName:FONT_FAMILY_NAME size:size]];
}

- (void)setTextFieldWidth:(float)width
{
    width = width;
    float x = _view.frame.origin.x;
    float y = _view.frame.origin.y;
    float height = _textField.frame.size.height;
    _textField.frame = CGRectMake(0, 0, width, height);
    _view.frame = CGRectMake(x, y, width, height);
}

- (NSString *)getTextFieldText
{
    return _textField.text;
}

- (void)setTextFieldText:(NSString *)text
{
    _textField.text = text;
}

- (void)setTextFieldFontColor:(float)r green:(float)g blue:(float)b alpha:(float)a
{
    _textField.textColor = [UIColor colorWithRed:r green:g blue:b alpha:a]; 
}

- (void)setTextFieldFontAlignment:(int)alignment
{
    _textField.textAlignment = alignment;
}

- (void)setTextFieldKeyboardType:(int)keyboardType
{
    _textField.keyboardType = keyboardType;
}

- (void)setTextFieldMaxLength:(int)length
{
    maxLength = length;
}
#pragma mark - Text Field Delegate

- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string {
    if(range.location >= maxLength){
        return NO;
    }
    int originalLength = [_textField.text length];
    int destLength = originalLength - range.length + [string length];
    if(destLength > maxLength){
        return NO;
    }
    return YES;
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField
{
    [_textField resignFirstResponder];
    return YES;
}

- (BOOL)textFieldShouldEndEditing:(UITextField *)textField
{
    return YES;
}
@end
