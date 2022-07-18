#if UNITY_EDITOR && !UNITY_WEBPLAYER && UNITY_IOS
#if UNITY_2019_3_OR_NEWER   // https://docs.unity3d.com/cn/2019.3/Manual/StructureOfXcodeProject.html
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections.Generic;

public class CocoaPodsRmoveepeatTarget : MonoBehaviour
{
    [PostProcessBuildAttribute(42)]//must be between 40 and 50 to ensure that it's not overriden by Podfile generation (40) and that it's added before "pod install" (50)
    public static void OnPostProcessBuild(BuildTarget target2, string path)
    {
        if (target2 == BuildTarget.iOS)
        {
            
            using (StreamWriter sw = File.AppendText(path + "/Podfile"))
            {
                //in this example I'm adding an app extension
                sw.WriteLine("post_install do |installer|\n    applicationTargets = [\n        'Pods-Unity-iPhone',\n    ]\n    libraryTargets = [\n        'Pods-UnityFramework',\n    ]\n\n    embedded_targets = installer.aggregate_targets.select { |aggregate_target|\n        libraryTargets.include? aggregate_target.name\n    }\n    embedded_pod_targets = embedded_targets.flat_map { |embedded_target| embedded_target.pod_targets }\n    host_targets = installer.aggregate_targets.select { |aggregate_target|\n        applicationTargets.include? aggregate_target.name\n    }\n\n    # We only want to remove pods from Application targets, not libraries\n    host_targets.each do |host_target|\n        host_target.xcconfigs.each do |config_name, config_file|\n            host_target.pod_targets.each do |pod_target|\n                if embedded_pod_targets.include? pod_target\n                    pod_target.specs.each do |spec|\n                        if spec.attributes_hash['ios'] != nil\n                            frameworkPaths = spec.attributes_hash['ios']['vendored_frameworks']\n                            else\n                            frameworkPaths = spec.attributes_hash['vendored_frameworks']\n                        end\n                        if frameworkPaths != nil\n                            frameworkNames = Array(frameworkPaths).map(&:to_s).map do |filename|\n                                extension = File.extname filename\n                                File.basename filename, extension\n                            end\n                            frameworkNames.each do |name|\n                                puts \"Removing #{name} from OTHER_LDFLAGS of target #{host_target.name}\"\n                                config_file.frameworks.delete(name)\n                            end\n                        end\n                    end\n                end\n            end\n            xcconfig_path = host_target.xcconfig_path(config_name)\n            config_file.save_as(xcconfig_path)\n        end\n    end\nend");
            }
        }
    }
}
#endif
#endif