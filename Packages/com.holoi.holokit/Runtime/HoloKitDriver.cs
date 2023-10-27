using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using Holoi.HoloKit.NativeInterface;
using Holoi.HoloKit.Utils;

namespace Holoi.HoloKit
{
    /// <summary>
    /// This component should be put into the first scene of every HoloKit project.
    /// It is responsible for initializing HoloKit SDK native callbacks and handling
    /// the lifecycle of the native ARSession.
    /// </summary>
    public class HoloKitDriver : MonoBehaviour
    {
        [Tooltip("This is a list of scenes in your build which have an ARSession. " +
            "We need these scene references to refresh the native ARKit ARSession when you switch scenes.")]
        [SerializeField] private SceneField[] _arScenes;

        private void Awake()
        {
            // Check whether the current device is supported by HoloKit SDK
            if (!HoloKitDeviceProfile.IsSupported())
            {
                Debug.Log("[HoloKitSDK] The current device is not supported by HoloKit SDK");
                return;
            }

            DontDestroyOnLoad(gameObject);
            
            if (PlatformChecker.IsRuntime)
            {
                InitializeHoloKitSDK();
            }
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        /// <summary>
        /// Register native callbacks and intercept the Unity ARSessionDelegate.
        /// </summary>
        private void InitializeHoloKitSDK()
        {
            HoloKitARSessionManagerNativeInterface.RegisterARSessionDelegates();
            HoloKitARSessionManagerNativeInterface.InterceptUnityARSessionDelegates();

            HoloKitIOSManagerNativeInterface.RegisterIOSNativeDelegates();
            HoloKitHandTrackerNativeInterface.RegisterHandTrackerDelegates();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            foreach (var arScene in _arScenes)
            {
                if (scene.name.Equals(arScene.SceneName))
                {
                    // When unloading an AR scene, we need to refresh the native ARSession.
                    // Failing to do this will cause the old ARSession to be used in the next AR scene.
                    LoaderUtility.Deinitialize();
                    LoaderUtility.Initialize();
                    // We need to intercept the Unity ARSessionDelegate again every time we refresh the native ARSession.
                    HoloKitARSessionManagerNativeInterface.InterceptUnityARSessionDelegates();
                    return;
                }
            }
        }
    }
}
