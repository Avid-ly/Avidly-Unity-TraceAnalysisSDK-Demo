using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Logger = TASDK.Util.Logger;

namespace UPTrace
{

    public class UPTraceApi
    {
        public readonly static string iOS_SDK_Version = "4.1.0.7";
        public readonly static string Android_SDK_Version = "4.1.1.1";
        public readonly static string Unity_Package_Version = "4.1.1.4";

        // 是否已经初始化
        private static bool isInited;
        private static UPTraceCall polyCall = null;


        private static void instanceOfCall()
        {
            if (polyCall == null)
            {
                polyCall = new UPTraceCall();
            }
        }


        /**
		 * 初始化统计包(Android与Ios均支持)
		 * @param productId 产品ID，必须正确指定且不能为空
		 * @param channelId 产品推广渠道 ，必须正确指定且不能为空
		 */
        public static void initTraceSDK(string productId, string channelId)
        {
            if (isInited)
            {
                return;
            }

            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.initTtraceSDK(productId, channelId);
            isInited = true;
            polyCall.enalbeDebugMode(true);

        }


        /**
		 * 初始化统计包(Android与Ios均支持) 带回调
		 * @param productId 产品ID，必须正确指定且不能为空
		 * @param channelId 产品推广渠道 ，必须正确指定且不能为空
		 * @param success 初始化成功回调
		 * @param fail 初始化失败回调，参数返回失败信息
		 */
        public static void initTraceSDKWithCallback(string productId, string channelId, Action<string> success, Action<string> fail)
        {
            if (isInited)
            {
                return;
            }

            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.initTtraceSDKWithCallback(productId, channelId, success   , fail);
            isInited = true;
            polyCall.enalbeDebugMode(true);

        }

        /**
		 * 初始化统计包(Android与Ios均支持)
		 * @param productId 产品ID，必须正确指定且不能为空
		 * @param channelId 产品推广渠道 ，必须正确指定且不能为空
		 * @param zone 枚举类型，分中国大陆以及海外两个区域
		 */
        public static void initTraceSDK(string productId, string channelId, UPTraceConstant.UPTraceSDKZoneEnum zone)
        {
            if (isInited)
            {
                return;
            }

            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.initTtraceSDK(productId, channelId, zone);
            isInited = true;
            polyCall.enalbeDebugMode(true);

        }


        /**
		 * 判断统计SDK是否被初始化(Android与Ios均支持)
		 */
        public static bool isTraceSDKInited()
        {
            return isInited;
        }

        /**
		 * 无参数事件打点方法(Android与Ios均支持)
		 * @param key, 打点事件id
		 */
        public static void traceKey(string key)
        {
            if (isInited)
            {
                polyCall.logKey(key);
            }
            else
            {
                Debug.Log("Fail to call traceKey(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 单参数事件打点方法(Android与Ios均支持)
		 * @param key, 打点事件id
		 * @param value, string类型参数内容
		 */
        public static void traceString(string key, string value)
        {
            if (isInited)
            {
                polyCall.logString(key, value);
            }
            else
            {
                Debug.Log("Fail to call traceString(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 多参数事件打点方法(Android与Ios均支持)
		 * @param key, 打点事件id
		 * @param dic, string类型的打点参数以Dictionary类型存储
		 */
        public static void traceDictionary(string key, Dictionary<string, string> dic)
        {
            if (isInited)
            {
                polyCall.logDictionary(key, dic);
            }
            else
            {
                Debug.Log("Fail to call traceDictionary(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 计数事件，用于只用记录事件的次数的场景(Android与Ios均支持)
		 * @param key, 打点事件的id
		 */
        public static void countKey(string key)
        {
            if (isInited)
            {
                polyCall.countKey(key);
            }
            else
            {
                Debug.Log("Fail to call countKey(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 支付上报，仅用于IOS平台，非Appstore的第三方支付数据上报
		 * @param playerId, 打点事件的id
		 * traceThirdpartyPaymentForIos -> traceThirdpartyZFLogReportForIos
		 */
        public static void traceThirdpartyZFLogReportForIos(string playerId, string thirdparty, string receiptDataString)
        {
            if (isInited)
            {
#if UNITY_IOS && !UNITY_EDITOR
					polyCall.logReport (playerId, thirdparty, receiptDataString, null);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceThirdpartyZFLogReportForIos(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 支付上报，仅用于IOS平台，非Appstore的第三方支付数据上报
		 * @param playerId, 打点事件的id
		 * traceThirdpartyPaymentWithServerForIos -> traceThirdpartyZFLogReportWithServerForIos
		 */
        public static void traceThirdpartyZFLogReportWithServerForIos(string playerId, string gameAccountServer, string thirdparty, string receiptDataString)
        {
            if (isInited)
            {
#if UNITY_IOS && !UNITY_EDITOR
					polyCall.logReportWithServer (playerId, gameAccountServer, thirdparty, receiptDataString, null);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceThirdpartyZFLogReportWithServerForIos(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 支付上报，仅用于IOS平台，方法内部做了平台检查，IOS会自动忽略
		 * @param playerId, 游戏用户ID
		 * @param receiptDataString IAP收据
		 * @param isbase64 receiptDataString 是否已经base64编码处理，如果没有方法内部完成base64处理
		 * tracePaymentWithPlayerIdForIos -> traceZFLogReportWithPlayerIdForIos
		 */
        public static void traceZFLogReportWithPlayerIdForIos(string playerId, string receiptDataString, bool isbase64)
        {
            if (isInited)
            {
#if UNITY_IOS && !UNITY_EDITOR
				polyCall.logReportPlayIdForIap (playerId, receiptDataString, isbase64, null);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceZFLogReportWithPlayerIdForIos(), please initialize the analysis SDK first!!!");
            }
        }

        public static void traceZFLogReportWithPlayerIdForIos(string playerId, string receiptDataString, bool isbase64, Dictionary<string, string> dic)
        {
            if (isInited)
            {
#if UNITY_IOS && !UNITY_EDITOR
				polyCall.logReportPlayIdForIap (playerId, receiptDataString, isbase64, dic);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceZFLogReportWithPlayerIdForIos(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 支付上报，仅用于IOS平台，方法内部做了平台检查，IOS会自动忽略
		 * @param playerId, 游戏用户ID
		 * @param gameAccountServer, 游戏区/服ID
		 * @param receiptDataString IAP收据
		 * @param isbase64 receiptDataString 是否已经base64编码处理，如果没有方法内部完成base64处理
		 * tracePaymentWithServerForIos -> traceZFLogReportWithServerForIos
		 */
        public static void traceZFLogReportWithServerForIos(string playerId, string gameAccountServer, string receiptDataString, bool isbase64)
        {
            if (isInited)
            {
#if UNITY_IOS && !UNITY_EDITOR
				polyCall.logReportServerForIap (playerId, gameAccountServer, receiptDataString, isbase64, null);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceZFLogReportWithServerForIos(), please initialize the analysis SDK first!!!");
            }
        }

        public static void traceZFLogReportWithServerForIos(string playerId, string gameAccountServer, string receiptDataString, bool isbase64, Dictionary<string, string> dic)
        {
            if (isInited)
            {
#if UNITY_IOS && !UNITY_EDITOR
				polyCall.logReportServerForIap (playerId, gameAccountServer, receiptDataString, isbase64, dic);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceZFLogReportWithServerForIos(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 支付上报，仅用于Android平台，方法内部做了平台检查，IOS会自动忽略
		 * @param playerId, 游戏用户ID，请传入CP方自己的player ID（请确认同登录上报的playerId保持一致）
		 * @param iabPurchaseOriginalJson
		 * @param iabPurchaseSignature
		 * 详细请参考统计包Android说明文档的ZFLogReport.logReport()方法介绍
		 * https://coding.net/u/holaverse/p/HolaAnalysisSDK/git/blob/master/android/doc/integration.md?public=true
		 * tracePaymentForAndroid -> traceZFLogReportForAndroid
		 */
        public static void traceZFLogReportForAndroid(string gameAccountId, string iabPurchaseOriginalJson, string iabPurchaseSignature)
        {
            if (isInited)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
					polyCall.logReport (gameAccountId, iabPurchaseOriginalJson, iabPurchaseSignature, null);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceZFLogReportForAndroid(), please initialize the analysis SDK first!!!");
            }
        }

        public static void traceZFLogReportForAndroid(string gameAccountId, string iabPurchaseOriginalJson, string iabPurchaseSignature, Dictionary<string, string> dic)
        {
            if (isInited)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
				polyCall.logReport (gameAccountId, iabPurchaseOriginalJson, iabPurchaseSignature, dic);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceZFLogReportForAndroid(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * 支付上报，可以区分游戏的服务器分区，仅用于Android平台，方法内部做了平台检查，IOS会自动忽略
		 * 详细请参考统计包Android说明文档的ZFLogReport.logReportWithServer()方法介绍
		 * https://coding.net/u/holaverse/p/HolaAnalysisSDK/git/blob/master/android/doc/integration.md?public=true
		 * tracePaymentWithServerForAndroid -> traceZFLogReportWithServerForAndroid
		 */
        public static void traceZFLogReportWithServerForAndroid(string gameAccountId, string gameAccountServer, string iabPurchaseOriginalJson,
            string iabPurchaseSignature)
        {
            if (isInited)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
					polyCall.logReportWithServer (gameAccountId, gameAccountServer, iabPurchaseOriginalJson, iabPurchaseSignature, null);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceZFLogReportWithServerForAndroid(), please initialize the analysis SDK first!!!");
            }
        }

        public static void traceZFLogReportWithServerForAndroid(string gameAccountId, string gameAccountServer, string iabPurchaseOriginalJson,
            string iabPurchaseSignature, Dictionary<string, string> dic)
        {
            if (isInited)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
					polyCall.logReportWithServer (gameAccountId, gameAccountServer, iabPurchaseOriginalJson, iabPurchaseSignature, dic);
#endif
            }
            else
            {
                Debug.Log("Fail to call traceZFLogReportWithServerForAndroid(), please initialize the analysis SDK first!!!");
            }
        }

        /**
		 * Twitter登录日志上报(Android与Ios均支持)
		 * 详细请参考统计包说明文档的HolaPay.twitterLogin()方法介绍
		 * https://coding.net/u/holaverse/p/HolaAnalysisSDK/git/blob/master/android/doc/integration.md?public=true
		 */
        public static void twitterLogin(string playerId, string twitterId, string twitterUserName, string twitterAuthToken)
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.twitterLogin(playerId, twitterId, twitterUserName, twitterAuthToken);
        }

        /**
		 * 平台账号登录日志上报(Android与Ios均支持)
		 * 详细请参考统计包说明文档的HolaPay.portalLogin()方法介绍
		 * https://coding.net/u/holaverse/p/HolaAnalysisSDK/git/blob/master/android/doc/integration.md?public=true
		 */
        public static void portalLogin(string playerId, string portalId)
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.portalLogin(playerId, portalId);
        }

        /**
		 * Facebook登录日志上报(Android与Ios均支持)
		 * 详细请参考统计包说明文档的HolaPay.facebookLogin()方法介绍
		 * https://coding.net/u/holaverse/p/HolaAnalysisSDK/git/blob/master/android/doc/integration.md?public=true
		 */
        public static void facebookLogin(string playerId, string openId, string openToken)
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.facebookLogin(playerId, openId, openToken);
        }

        /**
		 * 游客登录日志上报(Android与Ios均支持)
		 * 详细请参考统计包说明文档的HolaPay.guestLogin()方法介绍
		 * https://coding.net/u/holaverse/p/HolaAnalysisSDK/git/blob/master/android/doc/integration.md?public=true
		 */
        public static void guestLogin(string playerId)
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.guestLogin(playerId);
        }

        /**
		 * 华为登录上报(Android支持)
		 * 不需要对登录结果进行验签调用此方法
		 * appId：游戏的应用ID，在创建应用后由华为开发者联盟为应用分配的唯一标识。
		 * cpId：商户ID，即华为开发者联盟给开发者分配的“支付ID”。
		 * gameServerId：游戏自己的ID，一般由游戏服务器自己生成。
		 * playerId：玩家ID，原始值取自login接口返回的playerId参数。
		 */
        public static void huaWeiLoginNonAuth(string appId, string cpId, string gameServerId, string playerId)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
			if (null == polyCall) {
				instanceOfCall ();
			}
			polyCall.huaWeiLoginNonAuth (appId, cpId, gameServerId, playerId);
#endif
        }

        /**
		 * 华为登录上报(Android支持)
		 * 需要根据登录结果进行登录验签
		 * appId：游戏的应用ID，在创建应用后由华为开发者联盟为应用分配的唯一标识。
		 * cpId：商户ID，即华为开发者联盟给开发者分配的“支付ID”。
		 * gameServerId：游戏自己的ID，一般由游戏服务器自己生成。
		 * playerId：玩家ID，原始值取自login接口返回的playerId参数。
		 * playerSSign：登录签，名原始值取自login接口返回的gameAuthSign参数。
		 * playerLevel：玩家等级，原始值取自login接口返回的playerLevel参数。
		 * ts：时间戳，原始值取自login接口返回的ts参数
		 * @param diplayName：用户昵称。只有isAuth为1时才返回
		 */
        public static void huaweiLogin(string appId, string cpId, string gameServerId, string playerId, string playerSSign,
            string playerLevel, string ts, string diplayName)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
			if (null == polyCall) {
				instanceOfCall ();
			}
			polyCall.huaweiLogin (appId, cpId, gameServerId, playerId, playerSSign, playerLevel, ts, diplayName);
#endif
        }

        /**
		 * 通用登录上报(Android与Ios均支持)
		 * loginType：第三方sdk的类型标识，如'facebook', 'huawei'。
		 * playerId：玩家ID。
		 * loginToken：登录成功后，第三方sdk返回的可用于服务端签名检验的参数, 没有时传入 ''，不要传null
         * extension：扩展透传参数，默认传null
		 */
        public static void logCommonLogin(string loginType, string playerId, string loginToken, Dictionary<string, string> extension)
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            string extensionjson = "";
            if (extension != null && extension.Count > 0)
            {
                extensionjson = dicationaryToString(extension);
            }
            polyCall.logCommonLogin(loginType, playerId, loginToken, extensionjson);
        }

        /**
         * AASDK登录上报(Android与Ios均支持)
         * loginType：AASDK返回的登录平台标识
         * playerId：玩家ID。
         * loginToken：登录成功后，第三方sdk返回的可用于服务端签名检验的参数
         * ggid：AASDK返回的用户唯一标识
         * extension：扩展透传参数，默认传null
         */
        public static string LoginTypeGuest = "guest";
        public static string LoginTypeAas = "aas";
        public static string LoginTypeFacebook = "facebook";
        public static string LoginTypeGoogleplay = "googleplay";
        public static string LoginTypeTwitter = "twitter";
        public static string LoginTypeInstagram = "instagram";
        public static string LoginTypeGamecenter = "gamecenter";
        public static string LoginTypeUlt = "ult";
        public static string LoginTypeApple = "apple";
        public static string LoginTypeOther = "other";

        public static void logAASLogin(string loginType, string playerId, string loginToken, string ggid, Dictionary<string, string> extension)
        {
            string extensionjson = "";
            if (null == polyCall)
            {
                instanceOfCall();
            }
            if (extension != null && extension.Count > 0)
            {
                extensionjson = dicationaryToString(extension);
            }

            polyCall.logAASLogin(loginType, playerId, loginToken, ggid, extensionjson);
        }

        /**
		 * 获取统计包的UserId(Android与Ios均支持)
		 * 如果返回值为空，建议延迟多次获取
		 */
        public static string getUserId()
        {
            if (isInited)
            {
                return polyCall.getUserId();
            }
            else
            {
                Debug.Log("Fail to call getUserId(), please initialize the analysis SDK first!!!");
                return "";
            }
        }

        /**
		 * 获取统计包的OpenId(仅Android支持)
		 * 可在初始化SDK之前调用
		 */
        public static string getOpenIdForAndroid()
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            return polyCall.getOpenId();
        }

        /**
		 * 获取统计包的OpenId(Android与Ios均支持)
		 * 可在初始化SDK之前调用
		 */
        public static string getOpenId()
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            return polyCall.getOpenId();
        }

        /**
		 * 获取向统计包的传递的CustomerId(仅Android支持)
		 * 可在初始化SDK之前调用
		 */
        public static string getCustomerIdForAndroid()
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            return polyCall.getCustomerId();
        }

        /**
		 * 向统计包的传递CustomerId(仅Android支持)
		 * 请在初始化SDK之前调用
		 * 对于非GP的包，对于非欧盟用户可以传androidid，欧盟用户传统计包的openid
		 */
        public static void setCustomerIdForAndroid(string curstomerId)
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.setCustomerId(curstomerId);
        }

        /**
		 * 欧盟用户且明确拒绝GDPR授权申请后调用此方法(Android与Ios均支持)
		 * 可在初始化SDK之前调用
		 */
        public static void disableAccessPrivacyInformation()
        {
            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.disableAccessPrivacyInformation();
        }

        /**
		 * 初始化前台活跃时长统计
		 * 可在初始化SDK时同步初始化
		 */
        public static void initDurationReport(string serverName, string serverZone, string uid, string ggid)
        {

            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.initDurationReport(serverName, serverZone, uid, ggid);
        }

        /**
		 * 游戏回到前台
		 * 游戏回到前台调用
		 */
        public static void becomeActive()
        {

            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.becomeActive();
        }

        /**
		 * 游戏回到后台
		 * 游戏回到后台调用
		 */
        public static void resignActive()
        {

            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.resignActive();
        }

        public static void enalbeDebugMode(bool isOpen)
        {

            if (null == polyCall)
            {
                instanceOfCall();
            }
            polyCall.enalbeDebugMode(isOpen);
        }


        /**
		 * 获取campaign
		 * 需传入af sdk didReceiveConversionData()回调中获取到的ConversionData，以及成功和失败的callback
		 */
        public static void getConversionData(string afConversionData, Action<string> success, Action<string> fail)
        {
            if (polyCall != null && afConversionData != null && afConversionData.Length > 0)
            {
                polyCall.getConversionData(afConversionData, success, fail);
            }
        }

        /**
	     * 获取deep link 数据
	     * 需传入af sdk didReceiveConversionData()回调中获取到的ConversionData，以及成功和失败的callback
	    */
        public static void getDeepLinkData(string afConversionData, Action<string> success, Action<string> fail)
        {
            if (polyCall != null && afConversionData != null && afConversionData.Length > 0)
            {
                polyCall.getDeepLinknData(afConversionData, success, fail);
            }
        }

        /*
		 * 获取付费用户标签
		 */
        public static void getPayUserLayer(Action<string> success, Action<string> fail)
        {
            if (polyCall != null)
            {
                polyCall.getPayUserLayer(success, fail);
            }
        }

        /*
		 * 获取用户广告标签
		 */
        public static void getUserAdLayer(Action<string> success, Action<string> fail)
        {
            if (polyCall != null)
            {
                polyCall.getUserAdLayer(success, fail);
            }
        }

        /*
        * 获取 AB Test数据 
        */
        public static void getABTestData(Action<string> success, Action<string> fail)
        {
            if (polyCall != null)
            {
                polyCall.getABTestData(success, fail);
            }
        }

        private static string dicationaryToString(Dictionary<string, string> dic)
        {
            if (dic == null || dic.Count == 0)
            {
                return null;
            }

            string str = "{ \"array\":[";
            int len = dic.Count;
            int i = 0;
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                str += "{\"k\":\"" + kvp.Key + "\",";
                str += "\"v\":\"" + kvp.Value + "\"}";
                if (i < len - 1)
                {
                    str += ",";
                }
                else
                {
                    str += "]}";
                }
                i++;
            }

            //Debug.Log ("dicationaryToString:" + str);
            return str;
        }

        public static void showHelper(string pid, string channel)
        {
            if (polyCall != null)
            {
                polyCall.showHelper(pid, channel);
            }
        }

        /// <summary>
        /// 上报ios设备的 att状态
        /// </summary>
        public static void logATTStatusForIos()
        {
            if (polyCall != null)
            {
                polyCall.LogATTStatus();
            }
        }


        public static void setAFId(string afid)
        {
            if (polyCall != null)
            {
                polyCall.setAFId(afid);
            }
        }

        public static void setFirebaseId(string firebaseId)
        {
            if (string.IsNullOrEmpty(firebaseId))
            {
                Logger.LogError("firebaseId can not be null or error");
                return;
            }
            if (polyCall != null)
            {
                polyCall.setFirebaseId(firebaseId);
            }
        }
    }



}
