using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem.XR;
using Holoi.HoloKit.NativeInterface;
using Holoi.HoloKit.Utils;

namespace Holoi.HoloKit
{
    /// <summary>
    /// Used to determine the render mode of the app.
    /// </summary>
    public enum HoloKitRenderMode
    {
        Mono = 0,
        Stereo = 1
    }

    /// <summary>
    /// Used to determine the image quality of the ARKit background video feed.
    /// </summary>
    public enum BackgroundVideoFormat
    {
        VideoFormat2K = 0,
        VideoFormat4K = 1,
        VideoFormat4KHDR = 2
    }

    /// <summary>
    /// Used to indicate the camera tracking state of the current ARCamera.
    /// </summary>
    public enum CameraTrackingState
    {
        NotAvailable = 0,
        LimitedWithReasonNone = 1,
        LimitedWithReasonInitializing = 2,
        LimitedWithReasonExcessiveMotion = 3,
        LimitedWithReasonInsufficientFeatures = 4,
        LimitedWithReasonRelocalizing = 5,
        Normal = 6
    }

    public class HoloKitCamera : MonoBehaviour
    {
        // This class is a singleton so you can easily reference.
        public static HoloKitCamera Instance { get { return _instance; } }

        private static HoloKitCamera _instance;

        /// <summary>
        /// In stereo mode, it represents the pose of the point between the user's two eyes.
        /// In mono mode, it represents the pose of the device camera.
        /// </summary>
        [SerializeField] private Transform _centerEyePose;

        /// <summary>
        /// The camera used in mono mode.
        /// </summary>
        [SerializeField] private Camera _monoCamera;

        /// <summary>
        /// The left eye camera used in stereo mode.
        /// </summary>
        [SerializeField] private Camera _leftEyeCamera;

        /// <summary>
        /// The right eye camera used in stereo mode.
        /// </summary>
        [SerializeField] private Camera _rightEyeCamera;

        /// <summary>
        /// The background black camera used in stereo mode to cover the background image.
        /// </summary>
        [SerializeField] private Camera _blackCamera;

        [Tooltip("Interpupillary distance, which is the distance between the user's two eyes")]
        [SerializeField] [Range(0.054f, 0.074f)]
        private float _ipd = 0.064f;

        [Tooltip("The maximum rendering distance of the camera")]
        [SerializeField] private float _farClipPlane = 50f;

        [Tooltip("The background video format used for the current ARSession")]
        [SerializeField] private BackgroundVideoFormat _backgroundVideoFormat = BackgroundVideoFormat.VideoFormat2K;

        [Tooltip("Setting to true to attempt relocalization after an interruption of the ARSession")]
        [SerializeField] private bool _sessionShouldAttemptRelocalization = false;

        public Transform CenterEyePose => _centerEyePose;

        public HoloKitRenderMode RenderMode
        {
            get => _renderMode;
            set
            {
                _renderMode = value;
                OnRenderModeChangedInternal();
                OnRenderModeChanged?.Invoke(HoloKitRenderMode.Mono);
            }
        }

        public BackgroundVideoFormat BackgroundVideoFormat
        {
            get => _backgroundVideoFormat;
            set
            {
                _backgroundVideoFormat = value;
            }
        }

        public bool SessionShouldAttemptRelocalization
        {
            get => _sessionShouldAttemptRelocalization;
            set
            {
                _sessionShouldAttemptRelocalization = value;
                HoloKitARSessionManagerNativeInterface.SetSessionShouldAttemptRelocalization(_sessionShouldAttemptRelocalization);
            }
        }

        public float ARSessionStartTime => _arSessionStartTime;

        /// <summary>
        /// Represents the current render mode.
        /// </summary>
        private HoloKitRenderMode _renderMode = HoloKitRenderMode.Mono;

        /// <summary>
        /// The offset from the device camera to the user's center eye point.
        /// </summary>
        private Vector3 _cameraToCenterEyeOffset;

        /// <summary>
        /// The start time of the current ARSession.
        /// </summary>
        private float _arSessionStartTime;

        private ARCameraBackground _arCameraBackground;

        /// <summary>
        /// ARFoundation component used to track the camera pose in mono mode.
        /// </summary>
        private TrackedPoseDriver _defaultTrackedPoseDriver;

        /// <summary>
        /// Used to track the camera pose in stereo mode.
        /// </summary>
        private HoloKitTrackedPoseDriver _holokitTrackedPoseDriver;

        /// <summary>
        /// For some unknown reason, we must gradually increase iOS screen brightness in steps.
        /// This is the unit of each step.
        /// </summary>
        private const float SCREEN_BRIGHTNESS_INCREASE_STEP = 0.005f;

        /// <summary>
        /// Invoked when the render mode changes.
        /// </summary>
        public static event Action<HoloKitRenderMode> OnRenderModeChanged;

        /// <summary>
        /// Invoked when the camera changes its tracking state.
        /// </summary>
        public static event Action<CameraTrackingState> OnCameraChangedTrackingState;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            if (PlatformChecker.IsRuntime)
            {
                // Hide the iOS home button
                UnityEngine.iOS.Device.hideHomeButton = true;
                // Prevent the device from sleep
                Screen.sleepTimeout = SleepTimeout.NeverSleep;

                // Get the camera data from the native SDK, which is necessary for setting up the stereo cameras
                SetupHoloKitCameraData(HoloKitStarManagerNativeInterface.GetHoloKitCameraData(_ipd, _farClipPlane));
            }

            // Get the reference of two tracked pose drivers
            _defaultTrackedPoseDriver = GetComponent<TrackedPoseDriver>();
            _holokitTrackedPoseDriver = GetComponent<HoloKitTrackedPoseDriver>();

            // Set the initial ARKit background video format
            HoloKitARSessionManagerNativeInterface.SetBackgroundVideoFormat(_backgroundVideoFormat);
            // Set ARKit property
            HoloKitARSessionManagerNativeInterface.SetSessionShouldAttemptRelocalization(_sessionShouldAttemptRelocalization);

            // Get the reference of the ARCameraBackground
            _arCameraBackground = GetComponent<ARCameraBackground>();
            // Setpu the camera
            OnRenderModeChangedInternal();

            // Record the time when the ARSession starts
            _arSessionStartTime = Time.time;

            // Register the native callback which is invoked when there is a new ARSession frame
            HoloKitARSessionManagerNativeInterface.OnCameraChangedTrackingState += OnCameraChangedTrackingState;
        }

        private void OnDestroy()
        {
            // Unregister the native callback
            HoloKitARSessionManagerNativeInterface.OnCameraChangedTrackingState -= OnCameraChangedTrackingState;
        }

        private void Update()
        {
            if (_renderMode == HoloKitRenderMode.Stereo)
            {
                if (PlatformChecker.IsRuntime)
                {
                    // We want to keep the maximum screen brightness in stereo mode
                    var screenBrightness = HoloKitIOSManagerNativeInterface.GetScreenBrightness();
                    if (screenBrightness < 1f)
                    {
                        // We uses a trick here to properly set the screen brightness to 1. Actually, I think it's an iOS bug.
                        var newScreenBrightness = screenBrightness + SCREEN_BRIGHTNESS_INCREASE_STEP;
                        if (newScreenBrightness > 1f)
                            newScreenBrightness = 1f;
                        HoloKitIOSManagerNativeInterface.SetScreenBrightness(newScreenBrightness);
                        HoloKitIOSManagerNativeInterface.SetScreenBrightness(1f);
                    }
                }

                // We force the screen orientation to be LandscapeLeft in stereo mode
                if (Screen.orientation != ScreenOrientation.LandscapeLeft)
                {
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                }  
            }
        }

        /// <summary>
        /// We use the parsed camera data to setup the stereo cameras.
        /// </summary>
        /// <param name="holokitCameraData">The parsed camera data</param>
        private void SetupHoloKitCameraData(HoloKitCameraData holokitCameraData)
        {
            // Set the local position of the left eye camera, relative to the center eye
            _leftEyeCamera.transform.localPosition = holokitCameraData.CenterEyeToLeftEyeOffset;
            // Set the local position of the right eye camera, relative to the center eye
            _rightEyeCamera.transform.localPosition = holokitCameraData.CenterEyeToRightEyeOffset;

            // Setup the left eye camera
            _leftEyeCamera.nearClipPlane = holokitCameraData.NearClipPlane;
            _leftEyeCamera.farClipPlane = holokitCameraData.FarClipPlane;
            _leftEyeCamera.rect = holokitCameraData.LeftViewportRect;
            _leftEyeCamera.projectionMatrix = holokitCameraData.LeftProjectionMatrix;
            // Setup the right eye camera
            _rightEyeCamera.nearClipPlane = holokitCameraData.NearClipPlane;
            _rightEyeCamera.farClipPlane = holokitCameraData.FarClipPlane;
            _rightEyeCamera.rect = holokitCameraData.RightViewportRect;
            _rightEyeCamera.projectionMatrix = holokitCameraData.RightProjectionMatrix;

            // Save the camera to center eye offset
            _cameraToCenterEyeOffset = holokitCameraData.CameraToCenterEyeOffset;
        }

        /// <summary>
        /// This internal function is called to setup camera properties when the render mode changes.
        /// </summary>
        private void OnRenderModeChangedInternal()
        {
            // When render mode switched to stereo
            if (_renderMode == HoloKitRenderMode.Stereo)
            {
                // Disable camera background
                _arCameraBackground.enabled = false;
                // Disable mono camera and enable stereo ones
                _monoCamera.enabled = false;
                _leftEyeCamera.gameObject.SetActive(true);
                _rightEyeCamera.gameObject.SetActive(true);
                _blackCamera.gameObject.SetActive(true);
                // Set center eye pose to the center of the user's two eyes
                _centerEyePose.localPosition = _cameraToCenterEyeOffset;
                // Switch tracked pose drivers
                _defaultTrackedPoseDriver.enabled = false;
                _holokitTrackedPoseDriver.IsActive = true;
            }
            // When render mode switched to mono
            else
            {
                // Enable camera background
                _arCameraBackground.enabled = true;
                // Enable mono camera and disable stereo ones
                _monoCamera.enabled = true;
                _leftEyeCamera.gameObject.SetActive(false);
                _rightEyeCamera.gameObject.SetActive(false);
                _blackCamera.gameObject.SetActive(false);
                // Set center eye pose to the device camera
                _centerEyePose.localPosition = Vector3.zero;
                // Switch tracked pose drivers
                _defaultTrackedPoseDriver.enabled = true;
                _holokitTrackedPoseDriver.IsActive = false;
            }
        }
    }
}
