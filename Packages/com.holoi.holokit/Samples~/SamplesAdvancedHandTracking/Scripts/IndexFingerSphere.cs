using UnityEngine;

namespace Holoi.HoloKit.Samples.AdvancedHandTracking
{
    public class IndexFingerSphere : MonoBehaviour
    {
        // Indicate which hand this sphere is attached to
        public int HandIndex = 0;

        private HoloKitHandTracker _handTracker;

        private void Start()
        {
            // Get the reference of the hand tracker singleton instance
            _handTracker = HoloKitHandTracker.Instance;
        }

        private void Update()
        {
            // Check whether the desired hand is detected in the current frame
            if (_handTracker.AvailableHandCount > HandIndex)
            {
                // Get the desired hand
                HoloKitHand hand = _handTracker.Hands[HandIndex];
                // Attach the sphere to the end of the index finger
                transform.position = hand.GetLandmarkPosition(LandmarkType.Index3);
            }
            else
            {
                // There is no hand detected in this frame
                // Move the sphere to the sky so the user cannot see it
                transform.position = new Vector3(0f, 99f, 0f);
            }
        }
    }
}
