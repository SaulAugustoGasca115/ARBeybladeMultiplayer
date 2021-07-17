using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{

    ARPlaneManager arPlaneManager;
    ARPlacementManager arPlacementManager;


    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject searchForGameButton;

    public TextMeshProUGUI informUIPanelText;

    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlacementManager = GetComponent<ARPlacementManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        placeButton.SetActive(true);
        adjustButton.SetActive(false);

        searchForGameButton.SetActive(false);

        informUIPanelText.text = "Move Phone to detect planes and place the Battle Arena :) ";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableARPlacementAndPlaneDetection()
    {
        arPlaneManager.enabled = false;
        arPlacementManager.enabled = false;

        SetAllPlanesActiveOrDeactive(false);

        placeButton.SetActive(false);
        adjustButton.SetActive(true);

        searchForGameButton.SetActive(true);

        informUIPanelText.text = "Great , You placed the Arena, search for games to Battle !";

    }

    public void EnableARPlacementAndPlaneDetection()
    {
        arPlaneManager.enabled = true;
        arPlacementManager.enabled = true;

        SetAllPlanesActiveOrDeactive(true);

        placeButton.SetActive(true);
        adjustButton.SetActive(false);

        searchForGameButton.SetActive(false);

        informUIPanelText.text = "Move Phone to detect planes and place the Battle Arena :) ";

    }

    void SetAllPlanesActiveOrDeactive(bool value)
    {
        foreach(var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
