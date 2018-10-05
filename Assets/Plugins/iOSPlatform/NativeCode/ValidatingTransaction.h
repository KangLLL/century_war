//
//  ValidatingTransaction.h
//  Unity-iPhone
//
//  Created by LiK on 12-5-26.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef enum{
    Validating,
    ValidateSuccess,
    ValidateFail
}ValidateState;

@interface ValidatingTransaction : NSObject

@property (nonatomic, assign) NSString *transactionID;
@property (nonatomic, assign) ValidateState state;

@end
