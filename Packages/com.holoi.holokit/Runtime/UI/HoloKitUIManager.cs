using UnityEngine;
using UnityEngine.UI;

namespace Holoi.HoloKit.UI
{
    public class HoloKitUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _starPanel;

        [SerializeField] private RectTransform _phoneFrame;

        [SerializeField] private RectTransform _alignmenrMarker;

        [SerializeField] private RectTransform _starButtonFrame;

        private Text _starButtonText;

        /// <summary>
        /// The width of the phone frame in meters.
        /// </summary>
        private const float PHONE_FRAME_WIDTH_IN_METERS = 0.129871f;

        /// <summary>
        /// The height of the phone frame in meters.
        /// </summary>
        private const float PHONE_FRAME_HEIGHT_IN_METERS = 0.056322f;

        /// <summary>
        /// The ratio for converting meter to inch.
        /// </summary>
        private const float METER_TO_INCH_RATIO = 39.3701f;

        /// <summary>
        /// The ratio between the width of the star button and the width of the star button frame.
        /// </summary>
        private const float STAR_BUTTON_WIDTH_RATIO = 0.5187f;

        /// <summary>
        /// The ratio between the height of the star button and the height of the star button frame.
        /// </summary>
        private const float STAR_BUTTON_HEIGHT_RATIO = 0.5733f;

        private void Start()
        {
            SetupUI();

            _starPanel.SetActive(false);
        }

        private void SetupUI()
        {
            float screenWidth = HoloKitDeviceProfile.GetScreenWidth();
            float screenHeight = HoloKitDeviceProfile.GetScreenHeight();
            float screenDpi = HoloKitDeviceProfile.GetScreenDpi();
            float phoneFrameWidthInPixels = PHONE_FRAME_WIDTH_IN_METERS * METER_TO_INCH_RATIO * screenDpi;
            float phoneFrameHeightInPixels = PHONE_FRAME_HEIGHT_IN_METERS * METER_TO_INCH_RATIO * screenDpi;
            _phoneFrame.sizeDelta = new(phoneFrameWidthInPixels, phoneFrameHeightInPixels);

            float alignmentMarkerXInMeters = HoloKitDeviceProfile.GetHorizontalAlignmentMarkerOffset();
            float alignmentMarkerXInPixels = alignmentMarkerXInMeters * METER_TO_INCH_RATIO * screenDpi;
            _alignmenrMarker.anchoredPosition = new Vector2(alignmentMarkerXInPixels, 0f);
            _alignmenrMarker.sizeDelta = new(4f, screenHeight - phoneFrameHeightInPixels);

            float starButtonFrameWidthInPixels = screenWidth / 2f - alignmentMarkerXInPixels;
            float starButtonFrameHeightInPixels = screenHeight - phoneFrameHeightInPixels;
            _starButtonFrame.sizeDelta = new(starButtonFrameWidthInPixels, starButtonFrameHeightInPixels);

            RectTransform _starButtonRect = _starButtonFrame.GetComponentInChildren<Button>().GetComponent<RectTransform>();
            _starButtonRect.sizeDelta = new(STAR_BUTTON_WIDTH_RATIO * starButtonFrameWidthInPixels, STAR_BUTTON_HEIGHT_RATIO * starButtonFrameHeightInPixels);
            _starButtonText = _starButtonRect.GetComponentInChildren<Text>();
        }

        public void SwitchRenderMode()
        {
            if (HoloKitCamera.Instance.RenderMode == HoloKitRenderMode.Mono)
            {
                HoloKitCamera.Instance.RenderMode = HoloKitRenderMode.Stereo;
                _starPanel.SetActive(true);
                _starButtonText.text = "Mono";
            }
            else
            {
                HoloKitCamera.Instance.RenderMode = HoloKitRenderMode.Mono;
                _starPanel.SetActive(false);
                _starButtonText.text = "Stereo";
            }
        }
    }
}
