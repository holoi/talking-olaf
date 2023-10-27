using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using Holoi.HoloKit.Utils;

namespace Holoi.HoloKit.NativeInterface
{
    /// <summary>
    /// This native interface wraps a corresponding native class for ARSession configuration and delegate.
    /// </summary>
    public static class HoloKitARSessionManagerNativeInterface
    {
        /// <summary>
        /// This function needs to be called before the first ARSession arrives so that
        /// we can receive native callbacks. This function only needs to be called once.
        /// </summary>
        /// <param name="OnARSessionUpdatedFrame">Invoked when ARSession updates a new frame</param>
        /// <param name="OnCameraChangedTrackingState">Invoked when the camera changes its tracking state</param>
        /// <param name="OnRelocalizationSucceeded">Invoked when relocalization succeeds</param>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_RegisterARSessionDelegates(Action<double, IntPtr> OnARSessionUpdatedFrame,
                                                                         Action<int> OnCameraChangedTrackingState,
                                                                         Action OnRelocalizationSucceeded);

        /// <summary>
        /// This function also needs to be called before the first ARSession arrives so that
        /// we can intercept ARSessionDelegate from ARFoundation. This function needs to be called before every new ARSession.
        /// </summary>
        /// <param name="nativeARSessionPtr">The pointer of the native ARSession</param>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_InterceptUnityARSessionDelegates(IntPtr nativeARSessionPtr);

        /// <summary>
        /// Setting to true to force the device to relocalize after the ARSession has been interrupted.
        /// When setting to false, the ARSession will set its origin to the current pose after it resumes.
        /// </summary>
        /// <param name="shouldRelocalize">Whether to relocalize after an interruption</param>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_SetSessionShouldAttemptRelocalization(bool shouldRelocalize);

        /// <summary>
        /// Set the image quality of the background video feet.
        /// </summary>
        /// <param name="videoFormat">Background video format index</param>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_SetBackgroundVideoFormat(int videoFormat);

        /// <summary>
        /// Pause the current ARSession.
        /// </summary>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_PauseCurrentARSession();

        /// <summary>
        /// Resume the paused ARSession.
        /// </summary>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_ResumeCurrentARSession();

        /// <summary>
        /// Links to a native callback which is invoked when ARSession updates a new frame. 
        /// </summary>
        /// <param name="timestamp">The timestamp of the ARFrame</param>
        /// <param name="matrixPtr">The pointer of the camera pose matrix</param>
        [AOT.MonoPInvokeCallback(typeof(Action<double, IntPtr>))]
        private static void OnARSessionUpdatedFrameDelegate(double timestamp, IntPtr matrixPtr)
        {
            if (OnARSessionUpdatedFrame == null)
                return;

            float[] matrixData = new float[16];
            Marshal.Copy(matrixPtr, matrixData, 0, 16);
            Matrix4x4 matrix = new();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix[i, j] = matrixData[(4 * i) + j];
                }
            }
            OnARSessionUpdatedFrame(timestamp, matrix);
        }

        /// <summary>
        /// Links to a native callback which is invoked when the camera changes its tracking state.
        /// </summary>
        /// <param name="state">The index of the new camera tracking state</param>
        [AOT.MonoPInvokeCallback(typeof(Action<int>))]
        private static void OnCameraChangedTrackingStateDelegate(int state)
        {
            OnCameraChangedTrackingState?.Invoke((CameraTrackingState)state);
        }

        /// <summary>
        /// Links to a native callback which is invoked when relocalization succeeds.
        /// </summary>
        [AOT.MonoPInvokeCallback(typeof(Action))]
        private static void OnRelocalizationSucceededDelegate()
        {
            OnRelocalizationSucceeded?.Invoke();
        }

        /// <summary>
        /// Invoked when ARSession updates a new frame.
        /// The first parameter is the timestamp of the frame and the second parameter is the camera pose matrix.
        /// </summary>
        public static event Action<double, Matrix4x4> OnARSessionUpdatedFrame;

        /// <summary>
        /// Invoked when the camera changes its tracking state.
        /// The parameter is the new camera tracking state.
        /// </summary>
        public static event Action<CameraTrackingState> OnCameraChangedTrackingState;

        /// <summary>
        /// Invoked when relocalization succeeds.
        /// </summary>
        public static event Action OnRelocalizationSucceeded;

        /// <summary>
        /// Needs to be called before the first frame of the ARSession to register native callbacks.
        /// Only needs to be called once in the app lifecycle.
        /// </summary>
        public static void RegisterARSessionDelegates()
        {
            if (PlatformChecker.IsRuntime)
            {
                HoloKitSDK_RegisterARSessionDelegates(OnARSessionUpdatedFrameDelegate,
                                                      OnCameraChangedTrackingStateDelegate,
                                                      OnRelocalizationSucceededDelegate);
            }
        }

        /// <summary>
        /// A helper function to get the XRSessionSubsystem.
        /// </summary>
        /// <returns>XRSessionSubsystem</returns>
        private static XRSessionSubsystem GetLoadedXRSessionSubsystem()
        {
            List<XRSessionSubsystem> xrSessionSubsystems = new();
            SubsystemManager.GetSubsystems(xrSessionSubsystems);
            foreach (var subsystem in xrSessionSubsystems)
            {
                return subsystem;
            }
            Debug.Log("[HoloKitSDK] Failed to get loaded xr session subsystem");
            return null;
        }

        /// <summary>
        /// Needs to be called before the first frame of each ARSession. It will pass the native
        /// ARSession pointer to the native SDK code and allow the native SDK code to relay the ARSessionDelegate.
        /// </summary>
        public static void InterceptUnityARSessionDelegates()
        {
            if (PlatformChecker.IsEditor)
            {
                return;
            }

            var xrSessionSubsystem = GetLoadedXRSessionSubsystem();
            if (xrSessionSubsystem != null)
            {
                HoloKitSDK_InterceptUnityARSessionDelegates(xrSessionSubsystem.nativePtr);
                Debug.Log("[HoloKitSDK] Unity ARSessionDelegates intercepted");
            }
        }

        /// <summary>
        /// Setting to true to force the ARSession to relocalize after being interrupted.
        /// Otherwise, the ARSession will set its current pose as its origin after the interruption ends.
        /// </summary>
        /// <param name="shouldRelocalize"></param>
        public static void SetSessionShouldAttemptRelocalization(bool shouldRelocalize)
        {
            if (PlatformChecker.IsRuntime)
            {
                HoloKitSDK_SetSessionShouldAttemptRelocalization(shouldRelocalize);
            }
        }

        /// <summary>
        /// Set the format of the background video feed.
        /// </summary>
        /// <param name="videoFormat"></param>
        public static void SetBackgroundVideoFormat(BackgroundVideoFormat videoFormat)
        {
            if (PlatformChecker.IsRuntime)
            {
                HoloKitSDK_SetBackgroundVideoFormat((int)videoFormat);
            }
        }

        /// <summary>
        /// Pause the current ARSession.
        /// </summary>
        public static void PauseCurrentARSession()
        {
            if (PlatformChecker.IsRuntime)
            {
                HoloKitSDK_PauseCurrentARSession();
            }
        }

        /// <summary>
        /// Resume the paused ARSession.
        /// </summary>
        public static void ResumeCurrentARSession()
        {
            if (PlatformChecker.IsRuntime)
            {
                HoloKitSDK_ResumeCurrentARSession();
            }
        }
    }
}
