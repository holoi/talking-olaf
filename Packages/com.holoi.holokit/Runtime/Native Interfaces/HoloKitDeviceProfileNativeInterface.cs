using System.Runtime.InteropServices;
using UnityEngine;
using Holoi.HoloKit.Utils;

namespace Holoi.HoloKit.NativeInterface
{
    public static class HoloKitDeviceProfileNativeInterface
    {
        /// <summary>
		/// Check if the current device is supported by HoloKit SDK.
		/// </summary>
		/// <returns>Whether the device is supported</returns>
        [DllImport("__Internal")]
        private static extern bool HoloKitSDK_IsSupported();

        /// <summary>
		/// Check if the current device is an iPad.
		/// </summary>
		/// <returns>Whether the device is an iPad</returns>
        [DllImport("__Internal")]
        private static extern bool HoloKitSDK_IsIpad();

        /// <summary>
		/// Check if the current device supports LiDAR sensor.
		/// </summary>
		/// <returns>Whether the device supports LiDAR</returns>
        [DllImport("__Internal")]
        private static extern bool HoloKitSDK_SupportsLiDAR();

        /// <summary>
		/// Get the horizontal alighment marker offset in meters.
		/// </summary>
		/// <returns>Offset in meters</returns>
        [DllImport("__Internal")]
        private static extern float HoloKitSDK_GetHorizontalAlignmentMarkerOffset();

        /// <summary>
		/// Get the screen dpi of the current device.
		/// </summary>
		/// <returns>The device screen dpi</returns>
        [DllImport("__Internal")]
        private static extern float HoloKitSDK_GetScreenDpi();

        /// <summary>
        /// Check if the current device is supported by HoloKit SDK.
        /// </summary>
        /// <returns>Returns true if supported</returns>
        public static bool IsSupported()
        {
            if (PlatformChecker.IsRuntime)
            {
                return HoloKitSDK_IsSupported();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check if the current device is an iPad supported by HoloKit SDK.
        /// </summary>
        /// <returns>Returns true for iPads</returns>
        public static bool IsIpad()
        {
            if (PlatformChecker.IsRuntime)
            {
                return HoloKitSDK_IsIpad();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the current device is equipped with LiDAR sensor.
        /// </summary>
        /// <returns>Returns true for supported</returns>
        public static bool SupportsLiDAR()
        {
            if (PlatformChecker.IsRuntime)
            {
                return HoloKitSDK_SupportsLiDAR();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Get the horizontal alignment marker offset in meters. The horizontal alignment marker offset
        /// is the horizontal distance between the middle of the HoloKit headset to the alignment marker.
        /// </summary>
        /// <returns>Offset in meters</returns>
        public static float GetHorizontalAlignmentMarkerOffset()
        {
            if (PlatformChecker.IsRuntime)
            {
                return HoloKitSDK_GetHorizontalAlignmentMarkerOffset();
            }
            else
            {
                return 0.05075f;
            }
        }

        /// <summary>
        /// Get the screen dpi of the current device.
        /// </summary>
        /// <returns>The screen dpi</returns>
        public static float GetScreenDpi()
        {
            if (PlatformChecker.IsRuntime)
            {
                return HoloKitSDK_GetScreenDpi();
            }
            else
            {
                return Screen.dpi;
            }
        }
    }
}
