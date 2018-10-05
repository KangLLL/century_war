//
//  WebViewController.m
//  PlugInTextField
//
//  Created by LiK on 12-5-21.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import "HtmlViewController.h"

@implementation HtmlViewController

@synthesize status;

- (id)initWithURL:(NSString *)url PositionX:(float)x positionY:(float)y width:(float)width height:(float)height
{
    if(self = [super initWithPositionX:x positionY:y width:width height:height])
    {
        _webView = [[UIWebView alloc] initWithFrame:CGRectMake(0, 0, width, height)];
        [_view addSubview:_webView];
        [_webView.scrollView setBackgroundColor:[UIColor blackColor]];
        _webView.delegate = self;
        [_webView loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:url]]];
        self.status = WebLoading;
        _webView.dataDetectorTypes = UIDataDetectorTypeNone;
        [_webView release];
    }
    return self;
}

- (void)destroyWebView
{
    [_requestURL release];
    _webView.delegate = nil;
    [_webView removeFromSuperview];
    [super destory];
}

- (void)setWebViewWidth:(float)width
{
    float x = _view.frame.origin.x;
    float y = _view.frame.origin.y;
    float height = _webView.frame.size.height;
    _webView.frame = CGRectMake(0, 0, width, height);
    _view.frame = CGRectMake(x, y, width, height);
}
- (void)setWebViewHeight:(float)height
{
    float x = _view.frame.origin.x;
    float y = _view.frame.origin.y;
    float width = _webView.frame.size.width;
    _webView.frame = CGRectMake(0, 0, width, height);
    _view.frame = CGRectMake(x, y, width, height);
}


- (NSString *)getWebViewURL
{
    return [_webView.request URL].absoluteString;
}

- (NSString *)getRequestURL
{
    if(_requestURL == nil)
        return @"";
    return _requestURL;
}

- (void)setWebViewURL:(NSString *)url
{
    [_webView loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:url]]];
    self.status = WebLoading;
}

#pragma mark -
#pragma mark Web View Delegate

- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType
{
    _requestURL = [request URL].absoluteString;
    [_requestURL retain];
    return YES;
}

- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error
{
    self.status = WebLoadFailed;
}

- (void)webViewDidFinishLoad:(UIWebView *)webView
{
    self.status = WebLoaded;
}

@end
