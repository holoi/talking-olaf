using UnityEngine;
using Holoi.HoloKit.NativeInterface;

namespace Holoi.HoloKit
{
    /// <summary>
    /// This component is similar to TrackedPoseDriver provided by ARFoundation.
    /// It provides the poses of the ARCamera in stereo mode.
    /// </summary>
    public class HoloKitTrackedPoseDriver : MonoBehaviour
    {
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
            }
        }

        /// <summary>
        /// Setting to true to apply the poses from the native SDK to the ARCamera.
        /// </summary>
        private bool _isActive = false;

        /// <summary>
        /// The rotation matrix to rotate the pose matrix from portrait mode to landscape mode.
        /// </summary>
        private readonly Matrix4x4 RotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, -90f));

        private void Awake()
        {
            // Register the native callback which is invoded when there is a new ARSession frame
            HoloKitARSessionManagerNativeInterface.OnARSessionUpdatedFrame += OnARSessionUpdatedFrame;
        }

        private void OnDestroy()
        {
            // Unregister the callback
            HoloKitARSessionManagerNativeInterface.OnARSessionUpdatedFrame -= OnARSessionUpdatedFrame;
        }

        /// <summary>
        /// Delegate function invoked when there is a new ARSession frame
        /// </summary>
        /// <param name="timestamp">The timestamp of the frame</param>
        /// <param name="matrix">The pose matrix of the camera</param>
        private void OnARSessionUpdatedFrame(double timestamp, Matrix4x4 matrix)
        {
            if (_isActive)
            {
                // Rotate the matrix from portrait mode to landscape mode
                matrix *= RotationMatrix;
                // Apply the pose to the ARCamera
                transform.SetPositionAndRotation(matrix.GetPosition(), matrix.rotation);
            }
        }
    }
}
