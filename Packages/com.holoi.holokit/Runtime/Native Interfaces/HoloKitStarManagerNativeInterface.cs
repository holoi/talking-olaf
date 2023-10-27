using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Holoi.HoloKit.Utils;

namespace Holoi.HoloKit.NativeInterface
{
    /// <summary>
    /// The data necessary to setup the stereo cameras.
    /// </summary>
    public struct HoloKitCameraData
    {
        public Rect LeftViewportRect;
        public Rect RightViewportRect;
        public float NearClipPlane;
        public float FarClipPlane;
        public Matrix4x4 LeftProjectionMatrix;
        public Matrix4x4 RightProjectionMatrix;
        public Vector3 CameraToCenterEyeOffset;
        public Vector3 CameraToScreenCenterOffset;
        public Vector3 CenterEyeToLeftEyeOffset;
        public Vector3 CenterEyeToRightEyeOffset;
    }

    public static class HoloKitStarManagerNativeInterface
    {
        /// <summary>
        /// Get the camera data from the native SDK code.
        /// </summary>
        /// <param name="ipd">The ipd of the user</param>
        /// <param name="farClipPlane">The far clip plane</param>
        /// <returns>The pointer of the camera data</returns>
        [DllImport("__Internal")]
        private static extern IntPtr HoloKitSDK_GetHoloKitCameraData(float ipd, float farClipPlane);

        /// <summary>
        /// Release the pointer of the camera data.
        /// </summary>
        /// <param name="ptr">The pointer of the camera data</param>
        [DllImport("__Internal")]
        private static extern void HoloKitSDK_ReleaseHoloKitCameraData(IntPtr ptr);

        /// <summary>
        /// Get the parsed camera data.
        /// </summary>
        /// <param name="ipd">The ipd of the user</param>
        /// <param name="farClipPlane">The far clip plane</param>
        /// <returns>The parsed camera data</returns>
        public static HoloKitCameraData GetHoloKitCameraData(float ipd, float farClipPlane)
        {
            if (PlatformChecker.IsEditor)
                return new HoloKitCameraData();

            IntPtr cameraDataPtr = HoloKitSDK_GetHoloKitCameraData(ipd, farClipPlane);

            float[] result = new float[54];
            Marshal.Copy(cameraDataPtr, result, 0, 54);
            HoloKitSDK_ReleaseHoloKitCameraData(cameraDataPtr);

            Rect leftViewportRect = Rect.MinMaxRect(result[0], result[1], result[2], result[3]);
            Rect rightViewportRect = Rect.MinMaxRect(result[4], result[5], result[6], result[7]);
            Matrix4x4 leftProjectionMatrix = Matrix4x4.zero;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    leftProjectionMatrix[i, j] = result[10 + 4 * i + j];
                }
            }
            Matrix4x4 rightProjectionMatrix = Matrix4x4.zero;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    rightProjectionMatrix[i, j] = result[26 + 4 * i + j];
                }
            }

            return new HoloKitCameraData
            {
                LeftViewportRect = leftViewportRect,
                RightViewportRect = rightViewportRect,
                NearClipPlane = result[8],
                FarClipPlane = result[9],
                LeftProjectionMatrix = leftProjectionMatrix,
                RightProjectionMatrix = rightProjectionMatrix,
                CameraToCenterEyeOffset = new Vector3(result[42], result[43], result[44]),
                CameraToScreenCenterOffset = new Vector3(result[45], result[46], result[47]),
                CenterEyeToLeftEyeOffset = new Vector3(result[48], result[49], result[50]),
                CenterEyeToRightEyeOffset = new Vector3(result[51], result[52], result[53])
            };
        }
    }
}
