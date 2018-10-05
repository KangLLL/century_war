//
//  WebViewController.h
//  PlugInTextField
//
//  Created by LiK on 12-5-21.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PlugInController.h"

typedef enum{
    WebLoaded,
    WebLoadFailed,
    WebLoading
}WebLoadStatus;

@interface HtmlViewController : PlugInController<UIWebViewDelegate>{
    UIWebView *_webView;
    NSString *_requestURL;
}

@property (nonatomic, assign) WebLoadStatus status;

- (id)initWithURL:(NSString *)url PositionX:(float)x positionY:(float)y width:(float)width height:(float)height;
- (void)destroyWebView;
- (void)setWebViewWidth:(float)width;
- (void)setWebViewHeight:(float)height;
- (NSString *)getWebViewURL;
- (void)setWebViewURL:(NSString *)url;
- (NSString *)getRequestURL;

@end
