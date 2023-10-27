// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchen@holoi.com>
// SPDX-License-Identifier: MIT

using UnityEngine;
using UnityEngine.UI;

namespace HoloInteractive.XR.HoloKit.Samples.StereoscopicRendering
{
    public class StereoscopicRenderingUI : MonoBehaviour
    {
        [SerializeField] Text m_BtnText;

        HoloKitCameraManager holokitCamera;

        private void Start()
        {
            // lock screen to landscape awlays
            //Screen.orientation = ScreenOrientation.LandscapeLeft;
            holokitCamera = FindObjectOfType<HoloKitCameraManager>();
        }

        public void SwitchRenderMode()
        {
            holokitCamera.ScreenRenderMode = holokitCamera.ScreenRenderMode == ScreenRenderMode.Mono ? ScreenRenderMode.Stereo : ScreenRenderMode.Mono;
            m_BtnText.text = holokitCamera.ScreenRenderMode == ScreenRenderMode.Mono ? "Stereo" : "Mono";
        }

        void Update()
        {
            if (holokitCamera.ScreenRenderMode == ScreenRenderMode.Mono)
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
        }
    }
}
