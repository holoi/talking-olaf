using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Events;
using HoloInteractive.XR.HoloKit;

public class ARRaycastPlacementSpawn : MonoBehaviour
{
    public bool isCreateNorEnable = true;
    public GameObject SpawnedPrefab;
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

    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // need to update placement indicator, placement pose and spawn 
    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlacementPose.position = Vector3.zero;
                PlacementPose.rotation = Quaternion.Euler(0, 180, 0);
                ARPlaceObject(InitPosition);
            }
        }
        else
        {
            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                ARPlaceObject();
            }
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();

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

        
        if (isCreateNorEnable)
        {
            if (spawnedInstance)
            {
                spawnedInstance.transform.position = PlacementPose.position;
                spawnedInstance.transform.rotation = PlacementPose.rotation;
            }
            else
            {
                spawnedInstance = Instantiate(SpawnedPrefab, PlacementPose.position, PlacementPose.rotation);
                placementEvent?.Invoke();
            }

        }
        else
        {
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

        }

        spawnedInstance.transform.LookAt(spawnedInstance.transform.position - (direction.normalized * 0.1f));
    }

    void ARPlaceObject(Vector3 arPos)
    {
        
        if (isCreateNorEnable)
        {
            placementEvent?.Invoke();
            spawnedInstance = Instantiate(SpawnedPrefab, arPos, PlacementPose.rotation);
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


