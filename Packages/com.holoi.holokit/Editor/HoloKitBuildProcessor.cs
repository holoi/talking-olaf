#if UNITY_IOS
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;

namespace HoloKit.Editor
{
    /// <summary>Processes the project files after the build is performed.</summary>
    class HoloKitBuildProcessor
    {
        class PostProcessor : IPostprocessBuildWithReport
        {
            // NB: Needs to be > 0 to make sure we remove the shader since the
            //     Input System overwrites the preloaded assets array
            public int callbackOrder => 1;

            public void OnPostprocessBuild(BuildReport report)
            {
                PostprocessBuild(report);
            }

            void PostprocessBuild(BuildReport report)
            {
                AddXcodeBuildSettings(report.summary.outputPath);
            }

            private static void AddXcodeBuildSettings(string pathToBuiltProject)
            {
                string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
                PBXProject proj = new();
                proj.ReadFromString(File.ReadAllText(projPath));

                string mainTargetGuid = proj.GetUnityMainTargetGuid();
                string unityFrameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();

                proj.SetBuildProperty(mainTargetGuid, "SUPPORTED_PLATFORMS", "iphonesimulator iphoneos");
                proj.SetBuildProperty(unityFrameworkTargetGuid, "SUPPORTED_PLATFORMS", "iphonesimulator iphoneos");
                proj.SetBuildProperty(mainTargetGuid, "ENABLE_BITCODE", "NO");
                proj.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");
                //proj.AddBuildProperty(unityFrameworkTargetGuid, "LIBRARY_SEARCH_PATHS", "$(SDKROOT)/usr/lib/swift");
                proj.SetBuildProperty(mainTargetGuid, "SUPPORTS_MAC_DESIGNED_FOR_IPHONE_IPAD", "NO");

                proj.WriteToFile(projPath);
            }
        }
    }
}
#endif