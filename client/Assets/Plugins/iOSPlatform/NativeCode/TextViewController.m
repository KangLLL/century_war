//
//  TextViewController.m
//  PlugInTextField
//
//  Created by LiK on 12-5-18.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "TextViewController.h"
#define FONT_FAMILY_NAME            @"Arial"

@implementation TextViewController

#pragma mark - Public Methods
- (id)initWithText:(NSString *)text positionX:(float)x positionY:(float)y red:(float)r green:(float)g blue:(float)b alpha:(float)a fontSize:(float)size maxLength:(int)length alignment:(int)align width:(float)width height:(float)height
{
    if(self = [super initWithPositionX:x positionY:y width:width height:height])
    {
        _textView = [[UITextView alloc] initWithFrame:CGRectMake(0, 0, width, height)];
        //_textView.opaque = NO;
        //_textView.backgroundColor = [UIColor redColor];
        _textView.backgroundColor=[UIColor colorWithRed:0 green:0 blue:0 alpha:0];
        [_view addSubview:_textView];
        [self setTextViewText:text];
        [self setTextViewFontColor:r green:g blue:b alpha:a];
        [self setTextViewFontSize:size];
        maxLength = length;
        [self setTextViewFontAlignment:align];
        _textView.delegate = self;
        [_textView release];
    }
    return self;
}

- (void)destoryTextView
{
    _textView.delegate = nil;
    [_textView removeFromSuperview];
    [super destory];
}

- (void)setTextViewFontSize:(float)size
{
    [_textView setFont:[UIFont fontWithName:FONT_FAMILY_NAME size:size]];
}

- (void)setTextViewWidth:(float)width
{
    float x = _view.frame.origin.x;
    float y = _view.frame.origin.y;
    float height = _textView.frame.size.height;
    _textView.frame = CGRectMake(0, 0, width, height);
    _view.frame = CGRectMake(x, y, width, height);
}

- (void)setTextViewHeight:(float)height
{
    float x = _view.frame.origin.x;
    float y = _view.frame.origin.y;
    float width = _textView.frame.size.width;
    _textView.frame = CGRectMake(0, 0, width, height);
    _view.frame = CGRectMake(x, y, width, height);
}

- (NSString *)getTextViewText
{
    return _textView.text;
}

- (void)setTextViewText:(NSString *)text
{
    _textView.text = text;
}

- (void)setTextViewFontColor:(float)r green:(float)g blue:(float)b alpha:(float)a
{
    _textView.textColor = [UIColor colorWithRed:r green:g blue:b alpha:a];
}

- (void)setTextViewFontAlignment:(int)alignment
{
    _textView.textAlignment = alignment;
}

- (void)setTextViewKeyboardType:(int)keyboardType
{
    _textView.keyboardType = keyboardType;
}

- (void)setTextViewMaxLength:(int)length
{
    maxLength = length;
}
#pragma mark - Text View Delegate

- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)text
{
    if(range.location >= maxLength){
        return NO;
    }
    int originalLength = [_textView.text length];
    int destLength = originalLength - range.length + [text length];
    if(destLength > maxLength){
        return NO;
    }
    return YES;
}

- (BOOL)textViewShouldEndEditing:(UITextView *)textView
{
    [_textView resignFirstResponder];
    return YES;
}

@end
