//
//  TextViewInterface.m
//  PlugInTextField
//
//  Created by LiK on 12-5-18.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "TextViewInterface.h"
#import "TextViewController.h"
#import "CommonUtility.h"

#define FONT_FAMILY_NAME        @"Arial"

@interface TextViewInterface()

- (TextViewController *)getControllerAccordingToKey:(int)key;

@end

@implementation TextViewInterface

- (TextViewController *)getControllerAccordingToKey:(int)key
{
    NSString *keyString = [NSString stringWithFormat:@"%d",key];
    TextViewController *controller = [textViewControllerDict objectForKey:keyString];
    return controller;
}

- (int)createTextView:(NSString *)text positionX:(float) x positionY:(float) y red:(float)r green:(float)g blue:(float)b alpha:(float)a fontSize:(float)size maxLength:(int)length alignment:(int)align width:(float)width height:(float)height
{
    TextViewController *controller = [[TextViewController alloc] initWithText:text positionX:x positionY:y  red:r green:g blue:b alpha:a fontSize:size maxLength:length alignment:align width:width height:height];
    if(textViewControllerDict == nil){
        textViewControllerDict = [[NSMutableDictionary alloc] init];
    }
    int controllerId = [controller hash];
    NSString *contrllerKey = [NSString stringWithFormat:@"%d",controllerId];
    [textViewControllerDict setObject:controller forKey:contrllerKey];
    [controller release];
    return controllerId;
}

- (void)destoryTextView:(int)key
{
    NSString *keyString = [NSString stringWithFormat:@"%d",key];
    TextViewController *controller = [textViewControllerDict objectForKey:keyString];
    [controller destoryTextView];
    [textViewControllerDict removeObjectForKey:keyString];
}

- (void)setTextViewPosition:(int)key positionX:(float)x positionY:(float)y
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setPositionX:x positionY:y];
}

- (void)setTextViewFontSize:(int)key fontSize:(float)size
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setTextViewFontSize:size];
}

- (void)setTextViewWidth:(int)key width:(float)width
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setTextViewWidth:width];
}

- (void)setTextViewHeight:(int)key height:(float)height
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setTextViewHeight:height];
}

- (void)setTextViewFontColor:(int)key red:(float)r green:(float)g blue:(float)b alpha:(float)a
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setTextViewFontColor:r green:g blue:b alpha:a];
}

- (void)setTextViewFontAlignment:(int)key alignment:(int)alignment
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setTextViewFontAlignment:alignment];
}

- (void)setTextViewMaxLength:(int)key length:(int)length
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setTextViewMaxLength:length];
}

- (NSString *)getTextViewText:(int)key
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    return [controller getTextViewText];
}

- (void)setTextViewText:(int)key text:(NSString *)newValue
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setTextViewText:newValue];
}

- (BOOL)getFocus:(int)key
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    return [controller getFocus];
}

- (void)setFocus:(int)key newValue:(BOOL)newValue
{
    TextViewController *controller = [self getControllerAccordingToKey:key];
    [controller setFocus:newValue];
    
}
@end

static TextViewInterface *_textViewInterface = nil;

int _CreateNativeInputArea(const char* tex, float positionX, float positionY, float fontColorR, float fontColorG, float fontColorB, float fontColorA, int fontSize, int maxLength, int alignment, int width, int height)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    NSString *text = [CommonUtility getStringParam:tex];
    int controlID = [_textViewInterface createTextView:text positionX:positionX/2 positionY:positionY/2 red:fontColorR green:fontColorG blue:fontColorB alpha:fontColorA fontSize:fontSize/2 maxLength:maxLength alignment:alignment width:width/2 height:height/2];
    return controlID;
}

void _DestroyNativeInputArea(int controlID)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface destoryTextView:controlID];
}

void _SetInputAreaPosition(int controlID, float x, float y)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface setTextViewPosition:controlID positionX:x/2 positionY:y/2];
}

void _SetInputAreaFontSize(int controlID, int fontSize)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface setTextViewFontSize:controlID fontSize:fontSize/2];
}

void _SetInputAreaWidth(int controlID, int width)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface setTextViewWidth:controlID width:width/2];
}

void _SetInputAreaHeight(int controlID, int height)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface setTextViewHeight:controlID height:height/2];
}

char* _GetInputAreaText(int controlID)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    NSString *text = [_textViewInterface getTextViewText:controlID];
    return [CommonUtility makeStringCopy:text];
}

void _SetInputAreaText(int controlID, const char* text)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    NSString *textString = [CommonUtility getStringParam:text];
    [_textViewInterface setTextViewText:controlID text:textString];
}


void _SetInputAreaFontColor(int controlID, float ColorR, float ColorG, float ColorB, float ColorA)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface setTextViewFontColor:controlID red:ColorR green:ColorG blue:ColorB alpha:ColorA];
}

void _SetInputAreaAlignment(int controlID, int alignment)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface setTextViewFontAlignment:controlID alignment:alignment];
}

void _SetInputAreaMaxLength(int controlID, int maxLength)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface setTextViewMaxLength:controlID length:maxLength];
}

bool _GetInputAreaFocus(int controlID)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    return [_textViewInterface getFocus:controlID];
}

void _SetInputAreaFocus(int controlID, bool focusValue)
{
    if(_textViewInterface == nil)
        _textViewInterface = [[TextViewInterface alloc] init];
    [_textViewInterface setFocus:controlID newValue:focusValue];
}

