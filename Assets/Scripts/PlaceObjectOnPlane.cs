
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour


{
    private bool isPlaced = false;
    public GameObject ObjectToPlace;
    public GameObject placementIndicator;
    private Pose placementPose;
    private Transform placementTransform;
    private bool placementPoseIsValid = false;
    private TrackableId placedPlaneId = TrackableId.invalidId;

    ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    // Start is called before the first frame update
    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced) return;
        UpdatePlacementIndicator();
        UpdatePlacementPosistion();
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }

    }

    private void PlaceObject()
    {
        Instantiate(ObjectToPlace, placementPose.position, ObjectToPlace.transform.rotation);
        isPlaced=true;
        placementIndicator.SetActive(false);
    }

    private void UpdatePlacementPosistion()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
            placementPoseIsValid = s_Hits.Count > 0;
        if (placementPoseIsValid)
            placementPose = s_Hits[0].pose;
        placedPlaneId = s_Hits[0].trackableId;
        var planeManager = GetComponent<ARPlaneManager>();
        ARPlane arPlane = planeManager.GetPlane(placedPlaneId);
        placementTransform = arPlane.transform;
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, Quaternion.identity);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
}