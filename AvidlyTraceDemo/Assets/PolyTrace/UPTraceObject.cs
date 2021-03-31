using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TraceXXJSON;
using System;

namespace UPTrace {
	public class UPTraceObject : MonoBehaviour
	{
		private static UPTraceObject instance = null;
		public static readonly string Unity_Callback_Class_Name = "UPTraceSDK_Callback_Object";
		public static readonly string Unity_Callback_Function_Name = "onNativeCallback";

		public static readonly string Unity_Callback_Message_Key_Function = "callbackMessageKeyFunctionName";
		public static readonly string Unity_Callback_Message_Key_Parameter = "callbackMessageKeyParameter";

        private readonly static string Unity_Callback_Message_Function_Appsflyer_OnConversionDataSuccess = "AF_OnConversionDataSuccess";
        private readonly static string Unity_Callback_Message_Function_Appsflyer_OnConversionDataFail    = "AF_OnConversionDataFail";

        private readonly static string Unity_Callback_Message_Function_OnPayUserLayerSuccess 	= "OnPayUserLayerSuccess";    // 获取payUserLayer成功
		private readonly static string Unity_Callback_Message_Function_OnPayUserLayerFail 		= "OnPayUserLayerFail";       // 获取payUserLayer失败

		// added in 4007
		private readonly static string Unity_Callback_Message_Function_onUserAdLayer_Success	= "OnUserAdLayerSuccess";    
		private readonly static string Unity_Callback_Message_Function_OnUserAdLayer_Fail 		= "OnUserAdLayerFail";       

		public static UPTraceObject getInstance()
		{
			if (instance == null) {
				GameObject polyCallback = new GameObject (Unity_Callback_Class_Name);
				polyCallback.hideFlags = HideFlags.HideAndDontSave;
				DontDestroyOnLoad (polyCallback);

				instance = polyCallback.AddComponent<UPTraceObject> ();
			}
			return instance;
		}

		Action<string> getConversionDataSucceedCallback;
		Action<string> getConversionDataFailCallback;

		Action<string> getPayUserLayerSucceedCallback;
		Action<string> getPayUserLayerFailCallback;

		Action<string> getUserAdLayerSucceedCallback;
		Action<string> getUserAdLayerFailCallback;

		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{

		}

		public void setGetConversionDataCallback(Action<string> success, Action<string> fail) {
			getConversionDataSucceedCallback = success;
			getConversionDataFailCallback = fail;
		}

		public void setGetPayUserLayerCallback(Action<string> success, Action<string> fail) {
			getPayUserLayerSucceedCallback = success;
			getPayUserLayerFailCallback = fail;
		}

		public void setUserAdLayerCallback(Action<string> success, Action<string> fail) {
			getUserAdLayerSucceedCallback = success;
			getUserAdLayerFailCallback = fail;
		}
  
        public void onNativeCallback(string message) {

        	Debug.Log (message);
			Hashtable jsonObj = (Hashtable)TraceXXJSON.MiniJSON.jsonDecode (message);

			if (jsonObj.ContainsKey (Unity_Callback_Message_Key_Function)) {

				string function = (string)jsonObj[Unity_Callback_Message_Key_Function];
				string msg = "";
				if (jsonObj.ContainsKey (Unity_Callback_Message_Key_Parameter)) {
					msg = (string)jsonObj[Unity_Callback_Message_Key_Parameter];
				}
                
                string strFu = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Debug.Log ("===> onTargetCallback function: " + function + ", msg:" + msg + ", time at:" + strFu);

				//callback
				if (function.Equals (Unity_Callback_Message_Function_Appsflyer_OnConversionDataSuccess)) {

					// 回调方法一：全局方法回调
					// if (UPTraceApi.UPTraceOnConversionDataSuccessCallback != null) {
					// 	Debug.Log ("===> UnityPlugin Run UPTraceApi.UPTraceOnConversionDataSuccessCallback():" + msg);
					// 	// UPTraceApi.UPTraceOnConversionDataSuccessCallback (msg);
					// }
                    
                    // 回调方法二：通过传入进来的callback回调
					if (getConversionDataSucceedCallback != null) {
						getConversionDataSucceedCallback (msg);
						getConversionDataSucceedCallback = null;
					}
					else {
						Debug.Log ("===> can't run getConversionDataSucceedCallback(), no delegate object.");
					}
				} 
				else if (function.Equals (Unity_Callback_Message_Function_Appsflyer_OnConversionDataFail)) {

					// 回调方法一：全局方法回调
					// if (UPTraceApi.UPTraceOnConversionDataFailCallback != null) {
					// 	Debug.Log ("UnityPlugin Run UPTraceApi.UPTraceOnConversionDataFailCallback():" + msg);
					// 	UPTraceApi.UPTraceOnConversionDataFailCallback (msg);
					// } 

					// 回调方法二：通过传入进来的callback回调
					if (getConversionDataFailCallback != null) {
						getConversionDataFailCallback (msg);
						getConversionDataFailCallback = null;
					}
					else {
						Debug.Log ("===> can't run getConversionDataFailCallback(), no delegate object.");
					}
				}
				else if (function.Equals (Unity_Callback_Message_Function_OnPayUserLayerSuccess)) {
					if (getPayUserLayerSucceedCallback != null) {
						getPayUserLayerSucceedCallback (msg);
						getPayUserLayerSucceedCallback = null;
					}
					else {
						Debug.Log ("===> can't run getPayUserLayerSucceedCallback(), no delegate object.");
					}
				}
				else if (function.Equals (Unity_Callback_Message_Function_OnPayUserLayerFail)) {
					if (getPayUserLayerFailCallback != null) {
						getPayUserLayerFailCallback (msg);
						getPayUserLayerFailCallback = null;
					}
					else {
						Debug.Log ("===> can't run getPayUserLayerFailCallback(), no delegate object.");
					}
				}
				else if (function.Equals (Unity_Callback_Message_Function_onUserAdLayer_Success)) {
					if (getUserAdLayerSucceedCallback != null) {
						getUserAdLayerSucceedCallback (msg);
						getUserAdLayerSucceedCallback = null;
					}
					else {
						Debug.Log ("===> can't run getUserAdLayerSucceedCallback(), no delegate object.");
					}
				}
				else if (function.Equals (Unity_Callback_Message_Function_OnUserAdLayer_Fail)) {
					if (getUserAdLayerFailCallback != null) {
						getUserAdLayerFailCallback (msg);
						getUserAdLayerFailCallback = null;
					}
					else {
						Debug.Log ("===> can't run getUserAdLayerFailCallback(), no delegate object.");
					}
				}
				//unkown call
				else {
					Debug.Log ("===> onTargetCallback unkown function:" + function);
				}
			}
        }
	}
}


