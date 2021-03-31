//
//  TraceAnalysisDebug.h

//
//  Created by steve on 2017/7/14.
//  Copyright © 2017年 samliu. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSUInteger, TraceAnalysisDebugLevel) {
    TraceAnalysisDebugLevelNone = 0,      //控制台不输出log
    TraceAnalysisDebugLevelLog = 1,       //控制台输出打印log
};


@interface TraceAnalysisDebug : NSObject

+ (void)setDebugLevel:(TraceAnalysisDebugLevel)level;

@end
