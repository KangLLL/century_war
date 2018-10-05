//
//  TextFieldInterface.m
//  PlugInTextField
//
//  Created by LiK on 12-5-10.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "TextFieldInterface.h"
#import "TextFieldController.h"
#import "CommonUtility.h"

#define FONT_FAMILY_NAME        @"Arial"

@interface TextFieldInterface()

- (TextFieldController *)getControllerAccordingToKey:(int)key;

@end

@implementation TextFieldInterface

- (TextFieldController *)getControllerAccordingToKey:(int)key
{
    NSString *keyString = [NSString stringWithFormat:@"%d",key];
    TextFieldController *controller = [textFieldControllerDict objectForKey:keyString];
    return controller;
}

- (int)createTextField:(NSString *)text positionX:(float) x positionY:(float) y red:(float)r green:(float)g blue:(float)b alpha:(float)a fontSize:(float)size maxLength:(int)length alignment:(int)align width:(float)width
{
    TextFieldController *controller = [[TextFieldController alloc] initWithText:text positionX:x positionY:y  red:r green:g blue:b alpha:a fontSize:size maxLength:length alignment:align width:width];
    if(textFieldControllerDict == nil){
        textFieldControllerDict = [[NSMutableDictionary alloc] init];
    }
    int controllerId = [controller hash];
    NSString *contrllerKey = [NSString stringWithFormat:@"%d",controllerId];
    [textFieldControllerDict setObject:controller forKey:contrllerKey];
    [controller release];
    return controllerId;
}

- (void)destoryTextField:(int)key
{
    NSString *keyString = [NSString stringWithFormat:@"%d",key];
    TextFieldController *controller = [textFieldControllerDict objectForKey:keyString];
    [controller destroyTextField];
    [textFieldControllerDict removeObjectForKey:keyString];
}

- (void)setTextFieldPosition:(int)key positionX:(float)x positionY:(float)y
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setPositionX:x positionY:y];
}

- (void)setTextFieldFontSize:(int)key fontSize:(float)size
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setTextFieldFontSize:size];
}

- (void)setTextFieldWidth:(int)key width:(float)width
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setTextFieldWidth:width];
}

- (void)setTextFieldFontColor:(int)key red:(float)r green:(float)g blue:(float)b alpha:(float)a
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setTextFieldFontColor:r green:g blue:b alpha:a];
}

- (void)setTextFieldFontAlignment:(int)key alignment:(int)alignment
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setTextFieldFontAlignment:alignment];
}

- (void)setTextFieldKeyboardType:(int)key keyboardType:(int)keyboardType
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setTextFieldKeyboardType:keyboardType];
}

- (void)setTextFieldMaxLength:(int)key length:(int)length
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setTextFieldMaxLength:length];
}

- (NSString *)getTextFieldText:(int)key
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    return [controller getTextFieldText];
}

- (void)setTextFieldText:(int)key text:(NSString *)newValue
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setTextFieldText:newValue];
}

- (BOOL)getFocus:(int)key
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    return [controller getFocus];
}

- (void)setFocus:(int)key newValue:(BOOL)focused
{
    TextFieldController *controller = [self getControllerAccordingToKey:key];
    [controller setFocus:focused];

}
@end

static TextFieldInterface *_textFieldInterface = nil;

int _CreateNativeInputBox(const char* tex, float positionX, float positionY, float fontColorR, float fontColorG, float fontColorB, float fontColorA, int fontSize, int maxLength, int alignment, int width)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    NSString *text = [CommonUtility getStringParam:tex];
    int controlID = [_textFieldInterface createTextField:text positionX:positionX/2 positionY:positionY /2 red:fontColorR green:fontColorG blue:fontColorB alpha:fontColorA fontSize:fontSize/2 maxLength:maxLength alignment:alignment width:width/2];
    return controlID;
}

void _DestroyNativeInputBox(int controlID)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface destoryTextField:controlID];
}

void _SetInputBoxPosition(int controlID, float x, float y)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface setTextFieldPosition:controlID positionX:x/2 positionY:y/2];
}

void _SetInputBoxFontSize(int controlID, int fontSize)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface setTextFieldFontSize:controlID fontSize:fontSize/2];
}

void _SetInputBoxWidth(int controlID, int width)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface setTextFieldWidth:controlID width:width/2];
}

char* _GetInputBoxText(int controlID)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    NSString *text = [_textFieldInterface getTextFieldText:controlID];
    return [CommonUtility makeStringCopy:text];
}

void _SetInputBoxText(int controlID, const char* text)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    NSString *textString = [CommonUtility getStringParam:text];
    [_textFieldInterface setTextFieldText:controlID text:textString];
}


void _SetInputBoxFontColor(int controlID, float ColorR, float ColorG, float ColorB, float ColorA)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface setTextFieldFontColor:controlID red:ColorR green:ColorG blue:ColorB alpha:ColorA];
}

void _SetInputBoxAlignment(int controlID, int alignment)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface setTextFieldFontAlignment:controlID alignment:alignment];
}

void _SetInputBoxKeyboardType(int controlID, int keyboardType)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface setTextFieldKeyboardType:controlID keyboardType:keyboardType];
}

void _SetInputBoxMaxLength(int controlID, int maxLength)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface setTextFieldMaxLength:controlID length:maxLength];
}

bool _GetInputBoxFocus(int controlID)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    return [_textFieldInterface getFocus:controlID];
}

void _SetInputBoxFocus(int controlID,bool focusValue)
{
    if(_textFieldInterface == nil)
        _textFieldInterface = [[TextFieldInterface alloc] init];
    [_textFieldInterface setFocus:controlID newValue:focusValue];
}
