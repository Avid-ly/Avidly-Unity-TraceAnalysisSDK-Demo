using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPTrace;
public class Demo : MonoBehaviour
{
    //Demo ID
    private const string PRODUCTID = "999999";
    private const string CHANNELID = "32407";
    private const string PLAYERID = "player01";



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
}
