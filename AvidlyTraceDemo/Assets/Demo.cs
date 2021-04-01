using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;
using UPTrace;
public class Demo : MonoBehaviour, IAppsFlyerConversionData
{
    //Demo ID please contact avidly project mangager for your ID
    private const string PRODUCTID = "xxxxxx";
    private const string CHANNELID = "xxxxx";


    private const string AF_DEV_KEY = "fZvuk792H9hJQKmaTwuXxA";
    private const string AF_APPID = "1382108510";

    public string PLAYERID = "player01";

    private bool HasSetAFID = false;

    public void Update()
    {
        //AF 与AvidlyTraceSDK 互传,只设置一次，且确保拿到的不为空
        if (!HasSetAFID)
        {
            //获取 afid
            string afid = AppsFlyer.getAppsFlyerId();
            // 获取openId
            string openId = UPTraceApi.getOpenId();
            if (afid == null || afid == "" || openId == null || openId == "")
            {
                return;
            }
            Debug.Log("afid=" + afid);
            Debug.Log("openId=" + openId);

            // 将openId赋值给AppsFlyer
            AppsFlyer.setCustomerUserId(openId);
            // 将afid赋值给统计包
            UPTraceApi.setAFId(afid);

            HasSetAFID = true;
        }
    }

    public void Init()
    {
        //GooglePlay以外平台如amzon平台，发布时需要调用该api
#if UNITY_ANDROID && !UNITY_EDITOR
        UPTraceApi.setCustomerIdForAndroid(GetAndroidID());
#endif

        //欧盟用户展示gdpr弹窗,并在用户拒绝时调用disableAccessPrivacyInformation()
        //UPTraceApi.disableAccessPrivacyInformation();

        //正式包请关闭该debug
        UPTraceApi.enalbeDebugMode(true);

        //init TraceSDK
        UPTraceApi.initTraceSDK(PRODUCTID, CHANNELID);

        //在线时长上报
        OnlineReport();


        //appsflyer
        AppsFlyer.setIsDebug(true);

        AppsFlyer.initSDK(AF_DEV_KEY, AF_APPID, this);


        //iOS 延迟调用上报，目的是确保首次上报在ATT弹窗获得结果之后
#if UNITY_IOS && !UNITY_EDITOR
 
        AppsFlyeriOS.waitForATTUserAuthorizationWithTimeoutInterval(60);
#endif
        AppsFlyer.startSDK();


    }

    public void LogEvent()
    {
        UPTraceApi.traceKey("T01");
    }

    public void GuestLogin()
    {
        UPTraceApi.guestLogin(PLAYERID);
    }

    public void OnlineReport()
    {
        UPTraceApi.initDurationReport("test_server", "america", PLAYERID, "");
    }


    //在此处调用这两个api来实现在线时长上报
    void OnApplicationPause(bool paused)
    {
        if (UPTraceApi.isTraceSDKInited())
        {
            if (paused)
            {
                //程序进入后台时执行
                UPTraceApi.resignActive();
            }
            else
            {
                //程序从后台进入前台时
                UPTraceApi.becomeActive();
            }
        }
    }

    public static string GetAndroidID()
    {
        string _strAndroidID = "none";
        if (string.IsNullOrEmpty(_strAndroidID))
        {
            _strAndroidID = "none";
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        using (AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver"))
                        {
                            using (AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure"))
                            {
                                _strAndroidID = secure.CallStatic<string>("getString", contentResolver, "android_id");
                                if (string.IsNullOrEmpty(_strAndroidID))
                                {
                                    _strAndroidID = "none";
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
            }
            return _strAndroidID;
        }

        return _strAndroidID;
    }

    //AF归因回调--start
    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("onConversionDataSuccess", conversionData);
        Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
        // add deferred deeplink logic here


        // 在这里获取 AvidlyTraceSDK 用户标签
        UPTraceApi.getConversionData(conversionData,
            new System.Action<string>(onAvidlyConversionDataSuccess),
            new System.Action<string>(onAvidlyConversionDataFail)
        );
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("onConversionDataFail", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }
    //AF归因回调--end


    //Avidly 用户标签回调--start
    private void onAvidlyConversionDataSuccess(string result)
    {
        Debug.Log("===> onConversionDataSuccess Callback at: " + result);
    }
    private void onAvidlyConversionDataFail(string result)
    {
        Debug.Log("===> onConversionDataFail Callback at: " + result);
    }
    //Avidly 用户标签回调--end
}


