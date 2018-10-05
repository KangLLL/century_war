//
//  TextViewInterface.h
//  PlugInTextField
//
//  Created by LiK on 12-5-18.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface TextViewInterface : NSObject{
     NSMutableDictionary *textViewControllerDict;
}
- (int)createTextView:(NSString *)text positionX:(float) x positionY:(float) y red:(float)r green:(float)g blue:(float)b alpha:(float)a fontSize:(float)size maxLength:(int)length alignment:(int)align width:(float)width height:(float)height;
- (void)destoryTextView:(int)key;
- (void)setTextViewPosition:(int)key positionX:(float)x positionY:(float)y;
- (void)setTextViewFontSize:(int)key fontSize:(float)size;
- (void)setTextViewWidth:(int)key width:(float)width;
- (void)setTextViewHeight:(int)key height:(float)height;
- (void)setTextViewFontColor:(int)key red:(float)r green:(float)g blue:(float)b alpha:(float)a;
- (void)setTextViewFontAlignment:(int)key alignment:(int)alignment;
- (NSString *)getTextViewText:(int)key;
- (void)setTextViewText:(int)key text:(NSString *)newValue;
- (BOOL)getFocus:(int)key;
- (void)setFocus:(int)key newValue:(BOOL)focused;
@end
