
#if UNITY_EDITOR 
using System.IO;
using System.Xml;
using UnityEditor.Build;

#if (UNITY_2018_1_OR_NEWER)
using UnityEditor.Build.Reporting;
#else
using UnityEditor;
#endif


using UnityEngine;
using UPTrace;


#if (UNITY_2018_1_OR_NEWER)
public class TraceAndroidPostBuild : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
#elif(UNITY_5_6_OR_NEWER)
public class AndroidPostProcessBuild : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }

#endif


    private string TAG = "TASDK_Plugin==>";
 
    private string fileName = "tasdk_version.xml";


#if (UNITY_2018_1_OR_NEWER)
    public void OnPreprocessBuild(BuildReport report)
    {
        string verFilePath = CreateVersionXml();
        WriteVersionToXml(verFilePath);
    }
#elif(UNITY_5_6_OR_NEWER)

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        string verFilePath = CreateVersionXml();
        WriteVersionToXml(verFilePath);
    }
#endif


    private string CreateVersionXml()
    {
        string dirPath = Application.dataPath + Path.DirectorySeparatorChar + "Plugins" + Path.DirectorySeparatorChar +
            "Android" + Path.DirectorySeparatorChar + "res" + Path.DirectorySeparatorChar + "xml";
        bool hasDir = Directory.Exists(dirPath);
        if (!hasDir)
        {
            Debug.Log(TAG + dirPath + " not exist, create now");
            Directory.CreateDirectory(dirPath);
        }
        string versionFilePath = dirPath + Path.DirectorySeparatorChar + fileName;
        if (!System.IO.File.Exists(versionFilePath))
        {
            System.IO.File.Create(versionFilePath).Dispose();
        }
        else
        {
            Debug.Log(TAG + "delete old mssdk version xml");

            File.Delete(versionFilePath);
            System.IO.File.Create(versionFilePath).Dispose();
        }



        return versionFilePath;
    }


    private void WriteVersionToXml(string xmlFilePath)
    {
        Debug.Log(TAG + "writing ver xml for android to file :" + xmlFilePath);


        // version
        string iOS_SDK_Version = UPTraceApi.iOS_SDK_Version;
        string Android_SDK_Version = UPTraceApi.Android_SDK_Version;
        string Unity_Package_Version = UPTraceApi.Unity_Package_Version;
        Debug.Log(TAG + "iOS_SDK_Version=" + iOS_SDK_Version);
        Debug.Log(TAG + "Android_SDK_Version=" + Android_SDK_Version);
        Debug.Log(TAG + "Unity_Package_Version=" + Unity_Package_Version);


        XmlDocument myXmlDoc = new XmlDocument();
        XmlElement rootElement = myXmlDoc.CreateElement("version");
        myXmlDoc.AppendChild(rootElement);


        //初始化第一层的第一个子节点
        XmlElement firstLevelElement1 = myXmlDoc.CreateElement("tasdk_version");
        //填充第一层的第一个子节点的属性值（SetAttribute）
        firstLevelElement1.SetAttribute("android_ver", Android_SDK_Version);
        firstLevelElement1.SetAttribute("ios_ver", iOS_SDK_Version);

        firstLevelElement1.SetAttribute("unity_ver", Unity_Package_Version);

        rootElement.AppendChild(firstLevelElement1);
        //将xml文件保存到指定的路径下
        myXmlDoc.Save(xmlFilePath);
    }
}
#endif