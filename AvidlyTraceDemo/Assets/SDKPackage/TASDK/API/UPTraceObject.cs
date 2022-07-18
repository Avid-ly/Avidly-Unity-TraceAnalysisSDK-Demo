using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TraceXXJSON;
using System;
using Logger = TASDK.Util.Logger;

namespace UPTrace
{
    public class UPTraceObject : MonoBehaviour
    {
        private static UPTraceObject instance = null;
        public static readonly string Unity_Callback_Class_Name = "UPTraceSDK_Callback_Object";
        public static readonly string Unity_Callback_Function_Name = "onNativeCallback";

        public static readonly string Unity_Callback_Message_Key_Function = "callbackMessageKeyFunctionName";
        public static readonly string Unity_Callback_Message_Key_Parameter = "callbackMessageKeyParameter";

        private readonly static string Unity_Callback_Message_Function_Appsflyer_OnConversionDataSuccess =
            "AF_OnConversionDataSuccess";

        private readonly static string Unity_Callback_Message_Function_Appsflyer_OnConversionDataFail =
            "AF_OnConversionDataFail";

        private readonly static string Unity_Callback_Message_Function_Appsflyer_OnDeepLinkDataSuccess =
            "OnUserDLDataSuccess";

        private readonly static string Unity_Callback_Message_Function_Appsflyer_OnDeepLinkDataFail =
            "OnUserDLDataFail";

        private readonly static string
            Unity_Callback_Message_Function_OnPayUserLayerSuccess = "OnPayUserLayerSuccess"; // 获取payUserLayer成功

        private readonly static string
            Unity_Callback_Message_Function_OnPayUserLayerFail = "OnPayUserLayerFail"; // 获取payUserLayer失败

        // added in 4007
        private readonly static string Unity_Callback_Message_Function_onUserAdLayer_Success = "OnUserAdLayerSuccess";
        private readonly static string Unity_Callback_Message_Function_OnUserAdLayer_Fail = "OnUserAdLayerFail";

        private readonly static string Unity_Callback_Message_Function_onABTestData_Success = "OnABTestDataSuccess";
        private readonly static string Unity_Callback_Message_Function_onABTestData_Fail = "OnABTestDataFail";


        private readonly static string Unity_Callback_Message_Function_onInit_Success = "OnTasdkInitSuccess";
        private readonly static string Unity_Callback_Message_Function_onInit_Fail = "OnTasdkInitFail";

        public static UPTraceObject getInstance()
        {
            if (instance == null)
            {
                GameObject polyCallback = new GameObject(Unity_Callback_Class_Name);
                polyCallback.hideFlags = HideFlags.HideAndDontSave;
                DontDestroyOnLoad(polyCallback);

                instance = polyCallback.AddComponent<UPTraceObject>();
            }

            return instance;
        }

        Action<string> getConversionDataSucceedCallback;
        Action<string> getConversionDataFailCallback;


        Action<string> getDeepLinkDataSucceedCallback;
        Action<string> getDeepLinkDataFailCallback;

        Action<string> getPayUserLayerSucceedCallback;
        Action<string> getPayUserLayerFailCallback;

        Action<string> getUserAdLayerSucceedCallback;
        Action<string> getUserAdLayerFailCallback;

        Action<string> getAbTestDataSucceedCallback;
        Action<string> getAbTestDataFailCallback;

        Action<string> getInitSuccessCallback;
        Action<string> getInitFailCallback;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void setInitCallback(Action<string> success, Action<string> fail)
        {
            getInitSuccessCallback = success;
            getInitFailCallback = fail;
        }

        public void setGetConversionDataCallback(Action<string> success, Action<string> fail)
        {
            getConversionDataSucceedCallback = success;
            getConversionDataFailCallback = fail;
        }

        public void setGetDeepLinkDataCallback(Action<string> success, Action<string> fail)
        {
            getDeepLinkDataSucceedCallback = success;
            getDeepLinkDataFailCallback = fail;
        }

        public void setGetABTestDataCallback(Action<string> success, Action<string> fail)
        {
            getAbTestDataSucceedCallback = success;
            getAbTestDataFailCallback = fail;
        }


        public void setGetPayUserLayerCallback(Action<string> success, Action<string> fail)
        {
            getPayUserLayerSucceedCallback = success;
            getPayUserLayerFailCallback = fail;
        }

        public void setUserAdLayerCallback(Action<string> success, Action<string> fail)
        {
            getUserAdLayerSucceedCallback = success;
            getUserAdLayerFailCallback = fail;
        }

        public void onNativeCallback(string message)
        {
            Logger.LogInfo(message);
            Hashtable jsonObj = (Hashtable) TraceXXJSON.MiniJSON.jsonDecode(message);

            if (!jsonObj.ContainsKey(Unity_Callback_Message_Key_Function))
            {
                Logger.LogError("json string don't include func key :" + Unity_Callback_Message_Key_Function);
                return;
            }

            //parse func &args from android or ios

            string function = (string) jsonObj[Unity_Callback_Message_Key_Function];
            string msg = "";
            if (jsonObj.ContainsKey(Unity_Callback_Message_Key_Parameter))
            {
                msg = (string) jsonObj[Unity_Callback_Message_Key_Parameter];
            }

            // string strFu = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Logger.LogInfo("===> onTargetCallback function: " + function + ", msg:" + msg);
            // Logger.LogInfo("===> onTargetCallback function: " + function + ", msg:" + msg + ", time at:" + strFu);

            //callback
            if (function.Equals(Unity_Callback_Message_Function_Appsflyer_OnConversionDataSuccess))
            {
                // 回调方法一：全局方法回调
                // if (UPTraceApi.UPTraceOnConversionDataSuccessCallback != null) {
                // 	Debug.Log ("===> UnityPlugin Run UPTraceApi.UPTraceOnConversionDataSuccessCallback():" + msg);
                // 	// UPTraceApi.UPTraceOnConversionDataSuccessCallback (msg);
                // }

                // 回调方法二：通过传入进来的callback回调
                if (getConversionDataSucceedCallback != null)
                {
                    getConversionDataSucceedCallback(msg);
                    getConversionDataSucceedCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getConversionDataSucceedCallback(), no delegate object.");
                }
            }
            else if (function.Equals(Unity_Callback_Message_Function_Appsflyer_OnConversionDataFail))
            {
                // 回调方法一：全局方法回调
                // if (UPTraceApi.UPTraceOnConversionDataFailCallback != null) {
                // 	Debug.Log ("UnityPlugin Run UPTraceApi.UPTraceOnConversionDataFailCallback():" + msg);
                // 	UPTraceApi.UPTraceOnConversionDataFailCallback (msg);
                // } 

                // 回调方法二：通过传入进来的callback回调
                if (getConversionDataFailCallback != null)
                {
                    getConversionDataFailCallback(msg);
                    getConversionDataFailCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getConversionDataFailCallback(), no delegate object.");
                }
            }
            else if (function.Equals(Unity_Callback_Message_Function_Appsflyer_OnDeepLinkDataSuccess))
            {
                Logger.LogInfo("Unity_Callback_Message_Function_Appsflyer_OnDeepLinkDataSuccessk");


                if (getDeepLinkDataSucceedCallback != null)
                {
                    Logger.LogInfo("deeplink getDeepLinkDataSucceedCallback");

                    getDeepLinkDataSucceedCallback(msg);
                    getDeepLinkDataSucceedCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getDeepLinkDataSucceedCallback(), no delegate object.");
                }
            }
            else if (function.Equals(Unity_Callback_Message_Function_Appsflyer_OnDeepLinkDataFail))
            {
                if (getDeepLinkDataFailCallback != null)
                {
                    getDeepLinkDataFailCallback(msg);
                    getDeepLinkDataFailCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getDeepLinkDataFailCallback(), no delegate object.");
                }
            }

            else if (function.Equals(Unity_Callback_Message_Function_OnPayUserLayerSuccess))
            {
                if (getPayUserLayerSucceedCallback != null)
                {
                    getPayUserLayerSucceedCallback(msg);
                    getPayUserLayerSucceedCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getPayUserLayerSucceedCallback(), no delegate object.");
                }
            }
            else if (function.Equals(Unity_Callback_Message_Function_onABTestData_Success))
            {
                if (getAbTestDataSucceedCallback != null)
                {
                    getAbTestDataSucceedCallback(msg);
                    getAbTestDataSucceedCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getAbTestDataSucceedCallback(), no delegate object.");
                }
            }

            else if (function.Equals(Unity_Callback_Message_Function_onABTestData_Fail))
            {
                if (getAbTestDataFailCallback != null)
                {
                    getAbTestDataFailCallback(msg);
                    getAbTestDataFailCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getAbTestDataFailCallback(), no delegate object.");
                }
            }

            else if (function.Equals(Unity_Callback_Message_Function_onInit_Success))
            {
                if (getInitSuccessCallback != null)
                {
                    getInitSuccessCallback("init success");
                    getInitSuccessCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getInitSuccessCallback(), no delegate object.");
                }
            }

            else if (function.Equals(Unity_Callback_Message_Function_onInit_Fail))
            {
                if (getInitFailCallback != null)
                {
                    getInitFailCallback(msg);
                    getInitFailCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getInitFailCallback(), no delegate object.");
                }
            }

            else if (function.Equals(Unity_Callback_Message_Function_OnPayUserLayerFail))
            {
                if (getPayUserLayerFailCallback != null)
                {
                    getPayUserLayerFailCallback(msg);
                    getPayUserLayerFailCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getPayUserLayerFailCallback(), no delegate object.");
                }
            }
            else if (function.Equals(Unity_Callback_Message_Function_onUserAdLayer_Success))
            {
                if (getUserAdLayerSucceedCallback != null)
                {
                    getUserAdLayerSucceedCallback(msg);
                    getUserAdLayerSucceedCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getUserAdLayerSucceedCallback(), no delegate object.");
                }
            }
            else if (function.Equals(Unity_Callback_Message_Function_OnUserAdLayer_Fail))
            {
                if (getUserAdLayerFailCallback != null)
                {
                    getUserAdLayerFailCallback(msg);
                    getUserAdLayerFailCallback = null;
                }
                else
                {
                    Logger.LogError("===> can't run getUserAdLayerFailCallback(), no delegate object.");
                }
            }
            //unkown call
            else
            {
                Logger.LogError("===> onTargetCallback unkown function:" + function);
            }
        }
    }
}