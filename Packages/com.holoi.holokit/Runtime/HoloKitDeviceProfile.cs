using UnityEngine;
using Holoi.HoloKit.NativeInterface;

namespace Holoi.HoloKit
{
    public static class HoloKitDeviceProfile
    {
        /// <summary>
        /// Returns true if the current device is supported by HoloKit SDK.
        /// </summary>
        /// <returns></returns>
        public static bool IsSupported()
        {
            return HoloKitDeviceProfileNativeInterface.IsSupported();
        }

        /// <summary>
        /// Returns true if the current device is an iPad supported by HoloKit SDK.
        /// </summary>
        /// <returns></returns>
        public static bool IsIpad()
        {
            return HoloKitDeviceProfileNativeInterface.IsIpad();
        }

        /// <summary>
        /// Returns true if the current device is equipped with LiDAR sensor.
        /// </summary>
        /// <returns></returns>
        public static bool SupportsLiDAR()
        {
            return HoloKitDeviceProfileNativeInterface.SupportsLiDAR();
        }

        /// <summary>
        /// Get the horizontal alignment marker offset in meters. Horizontal alignment marker offset
        /// is the horizontal distance between the center of the HoloKit headset to its alignment marker.
        /// </summary>
        /// <returns>Horizontal alignment marker offset in meters</returns>
        public static float GetHorizontalAlignmentMarkerOffset()
        {
            return HoloKitDeviceProfileNativeInterface.GetHorizontalAlignmentMarkerOffset();
        }

        /// <summary>
        /// Get the screen dpi of the current device. DPI stands for 'dots per inch', which can be used to
        /// convert a distance between meter-based unit and pixel-based unit.
        /// </summary>
        /// <returns>The screen dpi of the current device</returns>
        public static float GetScreenDpi()
        {
            return HoloKitDeviceProfileNativeInterface.GetScreenDpi();
        }

        /// <summary>
        /// Get the screen width in pixels. This should always be the longer side of the screen.
        /// </summary>
        /// <returns>Screen width in pixels</returns>
        public static float GetScreenWidth()
        {
            return Screen.width > Screen.height ? Screen.width : Screen.height;
        }

        /// <summary>
        /// Get the screen height in pixels. This should always be the shorter side of the screen.
        /// </summary>
        /// <returns>Screen height in pixels</returns>
        public static float GetScreenHeight()
        {
            return Screen.width < Screen.height ? Screen.width : Screen.height;
        }
    }
}
