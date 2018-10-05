//
//  WebViewInterface.h
//  PlugInTextField
//
//  Created by LiK on 12-5-21.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "HtmlViewController.h"

@interface HtmlViewInterface : NSObject{
    NSMutableDictionary *webViewControllerDict;
}

- (int)createWebViewWithURL:(NSString *)url PositionX:(float)x positionY:(float)y width:(float)width height:(float)height;
- (void)destroyWebView:(int)key;
- (void)setWebViewPosition:(int)key positionX:(float)x positionY:(float)y;
- (void)setWebViewWidth:(int)key width:(float)width;
- (void)setWebViewHeight:(int)key height:(float)height;
- (NSString *)getWebViewURL:(int)key;
- (NSString *)getRequestURL:(int)key;
- (void)setWebViewURL:(int)key url:(NSString *)url;
- (BOOL)getWebViewFocus:(int)key;
- (BOOL)getFocus:(int)key;
- (void)setFocus:(int)key newValue:(BOOL)focused;
- (WebLoadStatus)getWebViewStatus:(int)key;
@end
