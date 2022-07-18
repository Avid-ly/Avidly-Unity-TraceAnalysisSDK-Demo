using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TASDK.Util {
    /// <summary>
    /// logger util 
    /// </summary>
    public class Logger
    {
        private readonly static string TAG = "TASDK-unity-plugin==>";

        //log info 
        public static void LogInfo(string msg)
        {
            if (msg == null)
                return;
            Debug.Log(TAG + msg);
        }

        //log warning info
        public static void LogWarning(string msg)
        {
            if (msg == null)
                return;
            Debug.LogWarning(TAG + msg);
        }

        //log error msg that can't be ignored by developer
        public static void LogError(string msg)
        {
            if (msg == null)
                return;
            Debug.LogError(TAG + msg);
        }

    }

}
