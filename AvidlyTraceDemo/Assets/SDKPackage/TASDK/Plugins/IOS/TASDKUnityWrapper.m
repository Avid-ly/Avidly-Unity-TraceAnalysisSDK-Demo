//
//  TASDKUnityWrapper.m
//  TASDK
//
//  Created by steve on 2021/11/10.
//  Copyright © 2021 Avidly Technology Co.,Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

extern void UnitySendMessage(const char* callbackClassName, const char* callbackFunctionName, const char* param);
extern UIViewController* UnityGetGLViewController(void);

@interface TASDKUnityWrapper : NSObject

@end

@implementation TASDKUnityWrapper

+ (void)TASDKUnitySendMessageWithCallbackClassName:(const char*)callbackClassName callbackFunctionName:(const char*)callbackFunctionName param:(const char*)param {
    UnitySendMessage(callbackClassName,callbackFunctionName,param);
}

+ (UIViewController *)TASDKUnityGetGLViewController {
    return UnityGetGLViewController();
}

@end
