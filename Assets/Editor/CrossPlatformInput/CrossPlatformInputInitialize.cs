using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace UnityStandardAssets.CrossPlatformInput.Inspector
{
    [InitializeOnLoad]
    public class CrossPlatformInitialize
    {
        static CrossPlatformInitialize()
        {
            var defines = GetDefinesList(NamedBuildTarget.Standalone);
            if (!defines.Contains("CROSS_PLATFORM_INPUT"))
            {
                SetEnabled("CROSS_PLATFORM_INPUT", true, false);
                SetEnabled("MOBILE_INPUT", true, true);
            }
        }

        [MenuItem("Mobile Input/Enable")]
        private static void Enable()
        {
            SetEnabled("MOBILE_INPUT", true, true);
        }

        [MenuItem("Mobile Input/Enable", true)]
        private static bool EnableValidate()
        {
            var defines = GetDefinesList(NamedBuildTarget.Android);
            return !defines.Contains("MOBILE_INPUT");
        }

        [MenuItem("Mobile Input/Disable")]
        private static void Disable()
        {
            SetEnabled("MOBILE_INPUT", false, true);
        }

        [MenuItem("Mobile Input/Disable", true)]
        private static bool DisableValidate()
        {
            var defines = GetDefinesList(NamedBuildTarget.Android);
            return defines.Contains("MOBILE_INPUT");
        }

        private static void SetEnabled(string defineName, bool enable, bool mobile)
        {
            var groups = mobile ? mobileBuildTargets : allBuildTargets;
            foreach (var target in groups)
            {
                var defines = GetDefinesList(target);
                if (enable)
                {
                    if (defines.Contains(defineName)) continue;
                    defines.Add(defineName);
                }
                else
                {
                    while (defines.Remove(defineName)) { }
                }
                string definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbols(target, definesString);
            }
        }

        private static List<string> GetDefinesList(NamedBuildTarget target)
        {
            return PlayerSettings.GetScriptingDefineSymbols(target).Split(';').ToList();
        }

        private static NamedBuildTarget[] allBuildTargets = new NamedBuildTarget[]
        {
            NamedBuildTarget.Standalone,
            NamedBuildTarget.Android,
            NamedBuildTarget.iOS,
        };

        private static NamedBuildTarget[] mobileBuildTargets = new NamedBuildTarget[]
        {
            NamedBuildTarget.Android,
            NamedBuildTarget.iOS,
        };
    }
}