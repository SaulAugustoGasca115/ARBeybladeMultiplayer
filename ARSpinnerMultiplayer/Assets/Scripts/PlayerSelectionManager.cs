using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{

    public Transform playerSwitcherTransform;
    public Button btnNextButton;
    public Button btnPreviousButton;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region UI Callback Methods
    public void NextPlayer()
    {
        btnNextButton.enabled = false;
        btnPreviousButton.enabled = false;

        StartCoroutine(Rotate(Vector3.up,playerSwitcherTransform,90.0f,1.0f));
        Debug.Log("ENTRYYYYYYYYYYY");
    }

    public void PreviousPlayer()
    {
        btnNextButton.enabled = false;
        btnPreviousButton.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90.0f, 1.0f));
        Debug.Log("NO ENTRYYYYYYYYYYY");
    }

    #endregion



    #region Private Methods

    IEnumerator Rotate(Vector3 axis,Transform tranform2Rotate,float angle,float duration = 1.0f)
    {
        Quaternion originalRotation = tranform2Rotate.rotation;
        Quaternion finalRotation = tranform2Rotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;

        while(elapsedTime < duration)
        {
            tranform2Rotate.rotation = Quaternion.Slerp(originalRotation,finalRotation,elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tranform2Rotate.rotation = finalRotation;

        btnNextButton.enabled = true;
        btnPreviousButton.enabled = true;
    }

    #endregion

}
