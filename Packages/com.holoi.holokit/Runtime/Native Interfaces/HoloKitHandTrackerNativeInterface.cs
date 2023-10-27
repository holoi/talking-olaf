using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Holoi.HoloKit.Utils;

namespace Holoi.HoloKit.NativeInterface
{
    public static class HoloKitHandTrackerNativeInterface
    {
        /// <summary>
        /// Needs to be called before enabling the hand tracking algorithm.
        /// This function only needs to be called once in the app's lifecycle.
        /// </summary>
        /// <param name="OnHandPoseUpdated">Invoked when a new hand pose is detected</param>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_RegisterHandTrackerDelegates(Action<int, IntPtr> OnHandPoseUpdated);

        /// <summary>
        /// Set the maxinum number of hands to be detected. The value can only be either 1 or 2. The default count is 1.
        /// </summary>
        /// <param name="maxHandCount">The maximum number of hands to be detected</param>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_SetMaxHandCount(int maxHandCount);

        /// <summary>
        /// Get the maxinum number of hands to be detected.
        /// </summary>
        /// <returns>The maximum number of hands to be detected</returns>
        [DllImport("__Internal")]
        private static extern int HoloKitSDK_GetMaxHandCount();

        /// <summary>
        /// Turn on or off the hand tracking algorithm.
        /// </summary>
        /// <param name="enabled">Set true to turn on and false to turn off</param>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_SetHandTrackerEnabled(bool enabled);

        /// <summary>
        /// Links to a native callback which is invoked when a new hand pose is detected.
        /// </summary>
        /// <param name="handIndex">The index of the detected hand</param>
        /// <param name="handDataPtr">The pointer pointing to the hand pose data</param>
        [AOT.MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnHandPoseUpdatedDelegate(int handIndex, IntPtr handDataPtr)
        {
            float[] handData = new float[63];
            Marshal.Copy(handDataPtr, handData, 0, 63);
            OnHandPoseUpdated?.Invoke(handIndex, handData);
        }

        /// <summary>
        /// Invoked when a new hand pose is detected.
        /// The first parameter is the hand index and the second parameter is the position array of landmarks.
        /// </summary>
        public static event Action<int, float[]> OnHandPoseUpdated;

        /// <summary>
        /// Needs to be called before enabling the hand tracking algorithm to register native callbacks.
        /// Only needs to be called once in the app's lifecycle.
        /// </summary>
        public static void RegisterHandTrackerDelegates()
        {
            if (PlatformChecker.IsRuntime)
            {
                HoloKitSDK_RegisterHandTrackerDelegates(OnHandPoseUpdatedDelegate);
            }
        }

        /// <summary>
        /// Set the maximum number of hands to be detected. Can either be 1 or 2.
        /// </summary>
        /// <param name="maxHandCount">The maximum number of hands to be detected</param>
        public static void SetMaxHandCount(int maxHandCount)
        {
            if (PlatformChecker.IsRuntime)
            {
                if (maxHandCount == 1 || maxHandCount == 2)
                {
                    HoloKitSDK_SetMaxHandCount(maxHandCount);
                }
                else
                {
                    Debug.Log("[HoloKitSDK] MaxHandCount can either be 1 or 2");
                }
            }
        }

        /// <summary>
        /// Get the current maximum number of hands to be detected.
        /// </summary>
        /// <returns>The maximum number of hands to be detected</returns>
        public static int GetMaxHandCount()
        {
            if (PlatformChecker.IsRuntime)
            {
                return HoloKitSDK_GetMaxHandCount();
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// Setting true to enable hand tracking and false to disable.
        /// </summary>
        /// <param name="enabled">True to enable and false to disable</param>
        public static void SetHandTrackerEnabled(bool enabled)
        {
            if (PlatformChecker.IsRuntime)
            {
                HoloKitSDK_SetHandTrackerEnabled(enabled);
            }
        }
    }
}