//
//  WebViewInterface.m
//  PlugInTextField
//
//  Created by LiK on 12-5-21.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "HtmlViewInterface.h"
#import "CommonUtility.h"

@interface HtmlViewInterface()
- (HtmlViewController *)getControllerAccordingToKey:(int)key;
@end

@implementation HtmlViewInterface
- (HtmlViewController *)getControllerAccordingToKey:(int)key
{
    NSString *keyString = [NSString stringWithFormat:@"%d",key];
    HtmlViewController *controller = [webViewControllerDict objectForKey:keyString];
    return controller;
}


- (int)createWebViewWithURL:(NSString *)url PositionX:(float)x positionY:(float)y width:(float)width height:(float)height
{
    HtmlViewController *controller = [[HtmlViewController alloc] initWithURL:url PositionX:x positionY:y width:width height:height];
    if(webViewControllerDict == nil){
        webViewControllerDict = [[NSMutableDictionary alloc] init];
    }
    int controllerId = [controller hash];
    NSString *contrllerKey = [NSString stringWithFormat:@"%d",controllerId];
    [webViewControllerDict setObject:controller forKey:contrllerKey];
    [controller release];
    return controllerId;
}

- (void)destroyWebView:(int)key
{
    NSString *keyString = [NSString stringWithFormat:@"%d",key];
    HtmlViewController *controller = [webViewControllerDict objectForKey:keyString];
    [controller destroyWebView];
    [webViewControllerDict removeObjectForKey:keyString];
}

- (void)setWebViewPosition:(int)key positionX:(float)x positionY:(float)y
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    [controller setPositionX:x positionY:y];
}

- (void)setWebViewWidth:(int)key width:(float)width
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    [controller setWebViewWidth:width];
}

- (void)setWebViewHeight:(int)key height:(float)height
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    [controller setWebViewHeight:height];
}

- (NSString *)getWebViewURL:(int)key
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    return [controller getWebViewURL];
}

- (NSString *)getRequestURL:(int)key
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    return [controller getRequestURL];
}

- (void)setWebViewURL:(int)key url:(NSString *)url
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    [controller setWebViewURL:url];
}

- (BOOL)getFocus:(int)key
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    return [controller getFocus];
}

- (void)setFocus:(int)key newValue:(BOOL)focused
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    [controller setFocus:focused];
}

- (WebLoadStatus)getWebViewStatus:(int)key
{
    HtmlViewController *controller = [self getControllerAccordingToKey:key];
    return controller.status;
}
@end

static HtmlViewInterface *_webViewInterface = nil;

int _CreateNativeHtmlView(const char *url, float positionX, float positionY, int width, int height)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    NSString *urlString = [CommonUtility getStringParam:url];
    int controlID = [_webViewInterface createWebViewWithURL:urlString PositionX:positionX/2 positionY:positionY/2 width:width/2 height:height/2];
    return controlID;
}

void _DestroyNativeHtmlView(int controlID)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    [_webViewInterface destroyWebView:controlID];
}


void _SetHtmlViewPosition(int controlID, float x, float y)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    [_webViewInterface setWebViewPosition:controlID positionX:x/2 positionY:y/2];
}

void _SetHtmlViewWidth(int controlID, int width)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    [_webViewInterface setWebViewWidth:controlID width:width/2];
}

void _SetHtmlViewHeight(int controlID, int height)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    [_webViewInterface setWebViewHeight:controlID height:height/2];
}

char* _GetHtmlViewURL(int controlID)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    NSString *url = [_webViewInterface getWebViewURL:controlID];
    return [CommonUtility makeStringCopy:url];
}

char* _GetHtmlViewRequestURL(int controlID)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    NSString *url = [_webViewInterface getRequestURL:controlID];
    return [CommonUtility makeStringCopy:url];
}

void _SetHtmlViewURL(int controlID, const char *url)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    NSString *urlString = [CommonUtility getStringParam:url];
    [_webViewInterface setWebViewURL:controlID url:urlString];
}

bool _GetHtmlViewFocus(int controlID)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    return [_webViewInterface getFocus:controlID];

}

void _SetHtmlViewFocus(int controlID,bool focusValue)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    [_webViewInterface setFocus:controlID newValue:focusValue];
}

int _GetHtmlViewStatus(int controlID)
{
    if(_webViewInterface == nil)
        _webViewInterface = [[HtmlViewInterface alloc] init];
    WebLoadStatus status = [_webViewInterface getWebViewStatus:controlID];
    return (int)status;
}


