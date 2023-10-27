using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Holoi.HoloKit.NativeInterface;

namespace Holoi.HoloKit
{
    /// <summary>
    /// The maximum number of hands to detect at the same time. Setting this to
    /// OneHand can reduce computational power.
    /// </summary>
    public enum MaxHandCount
    {
        OneHand = 0,
        BothHands = 1
    }

    public class HoloKitHandTracker : MonoBehaviour
    {
        // This class is a singleton so you can reference it easily.
        public static HoloKitHandTracker Instance { get { return _instance; } }

        private static HoloKitHandTracker _instance;

        [Tooltip("Setting to true to enable the hand tracking algorithm")]
        [SerializeField] private bool _enabled = true;

        [Tooltip("Setting this to OneHand to save computational power when you don't need two")]
        [SerializeField] private MaxHandCount _maxHandCount = MaxHandCount.BothHands;

        [Tooltip("Setting this to true to make landmarks visible")]
        [SerializeField] private bool _debugMode = true;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                SetHandTrackerEnabledInternal(_enabled);
            }
        }

        public MaxHandCount MaxHandCount
        {
            get => _maxHandCount;
            set
            {
                _maxHandCount = value;
                SetMaxHandCountInternal(_maxHandCount);
            }
        }

        public bool DebugMode
        {
            get => _debugMode;
            set
            {
                _debugMode = value;
                SetHandsVisible(value); 
            }
        }

        public int AvailableHandCount
        {
            get
            {
                int count = 0;
                foreach (var hand in _hands)
                {
                    if (hand.gameObject.activeSelf)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public List<HoloKitHand> Hands => _hands;

        private readonly List<HoloKitHand> _hands = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            if (!HoloKitDeviceProfile.SupportsLiDAR())
            {
                Debug.LogError("[HoloKitSDK] The current device doesn't support hand tracking since it doesn't have LiDAR sensor");
                return;
            }

            // Get the reference of each hand
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.TryGetComponent<HoloKitHand>(out var hand))
                {
                    _hands.Add(hand);
                }
            }

            // Register the native callback which is called when a new hand pose is detected
            HoloKitHandTrackerNativeInterface.OnHandPoseUpdated += OnHandPoseUpdated;
            HoloKitHandTrackerNativeInterface.RegisterHandTrackerDelegates();
            SetMaxHandCountInternal(_maxHandCount);
            SetHandTrackerEnabledInternal(_enabled);
            SetHandsVisible(_debugMode);
        }

        private void OnDestroy()
        {
            // Turn off the hand tracking algorithm
            HoloKitHandTrackerNativeInterface.SetHandTrackerEnabled(false);
            // Unregister the callback
            HoloKitHandTrackerNativeInterface.OnHandPoseUpdated -= OnHandPoseUpdated;
        }

        /// <summary>
        /// Turn on and off the hand tracking algorithm.
        /// </summary>
        /// <param name="enabled"></param>
        private void SetHandTrackerEnabledInternal(bool enabled)
        {
            if (enabled)
            {
                // There must be an AROcclusionManager before you can enable hand tracking
                var arOcclusionManager = FindObjectOfType<AROcclusionManager>();
                if (arOcclusionManager == null || arOcclusionManager.enabled == false)
                {
                    Debug.LogError("[HoloKitSDK] You must have an AROcclusionManager before turn on hand tracking");
                    return;
                }
            }

            HoloKitHandTrackerNativeInterface.SetHandTrackerEnabled(_enabled);
        }

        /// <summary>
        /// Set the maxinum detected hand count.
        /// </summary>
        /// <param name="maxHandCount"></param>
        private void SetMaxHandCountInternal(MaxHandCount maxHandCount)
        {
            if (maxHandCount == MaxHandCount.OneHand)
            {
                HoloKitHandTrackerNativeInterface.SetMaxHandCount(1);
            }
            else if (maxHandCount == MaxHandCount.BothHands)
            {
                HoloKitHandTrackerNativeInterface.SetMaxHandCount(2);
            }
        }

        /// <summary>
        /// Passing true to set the detected landmarks visible.
        /// </summary>
        /// <param name="visible"></param>
        private void SetHandsVisible(bool visible)
        {
            foreach (var hand in _hands)
            {
                hand.SetLandmarksVisible(visible);
            }
        }

        /// <summary>
        /// Delegate function invoked when is a new hand pose detected.
        /// </summary>
        /// <param name="handIndex">The index of the detected hand</param>
        /// <param name="poses">The poses of landmarks</param>
        private void OnHandPoseUpdated(int handIndex, float[] poses)
        {
            HoloKitHand hand = _hands[handIndex];
            hand.LastUpdateTime = Time.time;
            if (!hand.gameObject.activeSelf)
            {
                hand.gameObject.SetActive(true);
            }
            for (int i = 0; i < HoloKitHand.MAX_LANDMARK_COUNT; i++) {
                hand.Landmarks[i].position = new Vector3(poses[i * 3], poses[i * 3 + 1], poses[i * 3 + 2]);
            }
        }
    }
}
