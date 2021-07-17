using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{


    ARRaycastManager arRayCastManager;
    static List<ARRaycastHit> raycastHit = new List<ARRaycastHit>();

    public Camera arCamera;

    public GameObject battleArenaGameObject;

    private void Awake()
    {
        arRayCastManager = GetComponent<ARRaycastManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 centerOfScreen = new Vector3(Screen.width / 2,Screen.height / 2);

        Ray ray = arCamera.ScreenPointToRay(centerOfScreen);

        if(arRayCastManager.Raycast(ray,raycastHit,TrackableType.PlaneWithinPolygon))
        {
            //Intersection
            Pose hitPose = raycastHit[0].pose;

            Vector3 positionToBePlaced = hitPose.position;

            battleArenaGameObject.transform.position = positionToBePlaced;
        }

    }
}
