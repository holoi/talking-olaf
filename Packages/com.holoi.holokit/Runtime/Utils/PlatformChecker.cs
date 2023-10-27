using UnityEngine;

namespace Holoi.HoloKit.Utils
{
    /// <summary>
    /// This is a helper class which indicates the current platform.
    /// </summary>
    public static class PlatformChecker
    {
        /// <summary>
        /// Is the app currently running on Unity editor?
        /// </summary>
        public static bool IsEditor => Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer;

        /// <summary>
        /// Is the app currently running on an iOS device?
        /// </summary>
        public static bool IsRuntime => Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
