using System.Collections.Generic;
using UnityEngine;
using Holoi.HoloKit.Utils;

namespace Holoi.HoloKit
{
    /// <summary>
    /// There are 21 landmarks for a single detected hand. This enum represent the type of a single landmark.
    /// </summary>
    public enum LandmarkType
    {
        Wrist = 0,
        Thumb0 = 1,
        Thumb1 = 2,
        Thumb2 = 3,
        Thumb3 = 4,
        Index0 = 5,
        Index1 = 6,
        Index2 = 7,
        Index3 = 8,
        Middle0 = 9,
        Middle1 = 10,
        Middle2 = 11,
        Middle3 = 12,
        Ring0 = 13,
        Ring1 = 14,
        Ring2 = 15,
        Ring3 = 16,
        Little0 = 17,
        Little1 = 18,
        Little2 = 19,
        Little3 = 20
    }

    public class HoloKitHand : MonoBehaviour
    {
        public List<Transform> Landmarks => _landmarks;

        public float LastUpdateTime
        {
            get => _lastUpdateTime;
            set
            {
                _lastUpdateTime = value;
            }
        }

        /// <summary>
        /// References of landmarks in this hand.
        /// </summary>
        private readonly List<Transform> _landmarks = new();

        /// <summary>
        /// The timestamp of the last hand pose update.
        /// </summary>
        private float _lastUpdateTime;

        /// <summary>
        /// There are totally 21 landmarks for a hand.
        /// </summary>
        public const int MAX_LANDMARK_COUNT = 21;

        /// <summary>
        /// The delay before the hand changes to undetected state.
        /// </summary>
        private const float DISAPPEAR_DELAY = 0.2f;

        private void Awake()
        {
            for (int i = 0; i < MAX_LANDMARK_COUNT; i++)
            {
                _landmarks.Add(transform.GetChild(i));
            }
        }

        private void Start()
        {
            SetupLandmarksColor();

            if (PlatformChecker.IsRuntime)
            {
                gameObject.SetActive(false);
            } 
        }

        private void Update()
        {
            // We set the hand to inactive when it is not detected
            if (Time.time - _lastUpdateTime > DISAPPEAR_DELAY)
            {
                if (PlatformChecker.IsRuntime)
                {
                    gameObject.SetActive(false);
                }   
            }
        }

        /// <summary>
        /// Get the position of a specific landmark.
        /// </summary>
        /// <param name="landmarkType">The type of the desired landmark</param>
        /// <returns>The position of the landmark</returns>
        public Vector3 GetLandmarkPosition(LandmarkType landmarkType)
        {
            return _landmarks[(int)landmarkType].position;
        }

        private void SetupLandmarksColor()
        {
            for (int i = 0; i < MAX_LANDMARK_COUNT; i++)
            {
                LandmarkType joint = (LandmarkType)i;
                switch (joint)
                {
                    case LandmarkType.Wrist:
                        _landmarks[i].GetComponent<MeshRenderer>().material.color = Color.red;
                        break;
                    case LandmarkType.Thumb0:
                    case LandmarkType.Index0:
                    case LandmarkType.Middle0:
                    case LandmarkType.Ring0:
                    case LandmarkType.Little0:
                        _landmarks[i].GetComponent<MeshRenderer>().material.color = Color.yellow;
                        break;
                    case LandmarkType.Thumb1:
                    case LandmarkType.Index1:
                    case LandmarkType.Middle1:
                    case LandmarkType.Ring1:
                    case LandmarkType.Little1:
                        _landmarks[i].GetComponent<MeshRenderer>().material.color = Color.green;
                        break;
                    case LandmarkType.Thumb2:
                    case LandmarkType.Index2:
                    case LandmarkType.Middle2:
                    case LandmarkType.Ring2:
                    case LandmarkType.Little2:
                        _landmarks[i].GetComponent<MeshRenderer>().material.color = Color.cyan;
                        break;
                    case LandmarkType.Thumb3:
                    case LandmarkType.Index3:
                    case LandmarkType.Middle3:
                    case LandmarkType.Ring3:
                    case LandmarkType.Little3:
                        _landmarks[i].GetComponent<MeshRenderer>().material.color = Color.blue;
                        break;
                }
            }
        }

        /// <summary>
        /// Setting to true to make the hand landmarks visible in debug mode.
        /// </summary>
        /// <param name="visible">Landmark visibility</param>
        public void SetLandmarksVisible(bool visible)
        {
            for (int i = 0; i < MAX_LANDMARK_COUNT; i++)
            {
                _landmarks[i].GetComponent<MeshRenderer>().enabled = visible;
            }
        }
    }
}
