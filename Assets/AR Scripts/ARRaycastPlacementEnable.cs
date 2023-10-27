using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Events;
using HoloInteractive.XR.HoloKit;

public class ARRaycastPlacementEnable : MonoBehaviour
{
    public GameObject placementIndicator;

    [SerializeField]
    private GameObject spawnedInstance;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    [SerializeField]
    UnityEvent placementEvent;
    [SerializeField]
    Vector3 InitPosition;

    HoloKitCameraManager holokitCamera;

    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        holokitCamera = FindObjectOfType<HoloKitCameraManager>();
    }

    /// <returns>true if mouse or first touch is over any event system object ( usually gui elements )</returns>
    public static bool IsPointerOverGameObject()
    {
        //check mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }

        return false;
    }

    // need to update placement indicator, placement pose and spawn 
    void Update()
    {
        if (holokitCamera.ScreenRenderMode == ScreenRenderMode.Stereo)
        {

        }
        else
        {

            if (Application.isEditor)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {

                    int id = Input.GetTouch(0).fingerId;
                    if (IsPointerOverGameObject())
                    {
                        // ui touched
                    }
                    else
                    {
                        // touched on empty screen
                        PlacementPose.position = Vector3.zero;
                        PlacementPose.rotation = Quaternion.Euler(0, 180, 0);
                        ARPlaceObject(InitPosition);
                    }
                }
            }
            else
            {
                if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {

                    int id = Input.GetTouch(0).fingerId;
                    if (IsPointerOverGameObject())
                    {
                        // ui touched
                    }
                    else
                    {
                        // touched on empty screen
                        ARPlaceObject();
                    }
                }
            }

            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }

    }
    void UpdatePlacementIndicator()
    {
        if (spawnedInstance == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void ARPlaceObject()
    {
        var direction = Vector3.ProjectOnPlane(PlacementPose.position - FindObjectOfType<HoloKitCameraManager>().CenterEyePose.position, Vector3.up);

        if (spawnedInstance.activeSelf)
        {
            spawnedInstance.transform.position = PlacementPose.position;
            spawnedInstance.transform.rotation = PlacementPose.rotation;
        }
        else
        {
            placementEvent?.Invoke();
            spawnedInstance.SetActive(true);
            spawnedInstance.transform.position = PlacementPose.position;
            spawnedInstance.transform.rotation = PlacementPose.rotation;
        }

        spawnedInstance.transform.LookAt(spawnedInstance.transform.position - (direction.normalized * 0.1f));
    }

    void ARPlaceObject(Vector3 arPos)
    {
        if (spawnedInstance.activeSelf)
        {
            spawnedInstance.transform.position = arPos;
            spawnedInstance.transform.rotation = PlacementPose.rotation;
        }
        else
        {
            placementEvent?.Invoke();
            spawnedInstance.SetActive(true);
            spawnedInstance.transform.position = arPos;
            spawnedInstance.transform.rotation = PlacementPose.rotation;
        }
    }
}