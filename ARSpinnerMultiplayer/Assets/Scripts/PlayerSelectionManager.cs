using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{

    public Transform playerSwitcherTransform;
    

    public int playerSelectionNumber;

    public GameObject[] spinnerTopModels;


    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;
    public Button btnNextButton;
    public Button btnPreviousButton;
    public GameObject uiSelection;
    public GameObject uiAfterSelection;

    // Start is called before the first frame update
    void Start()
    {
        uiSelection.SetActive(true);
        uiAfterSelection.SetActive(false);

        playerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region UI Callback Methods
    public void NextPlayer()
    {
        playerSelectionNumber += 1;

        if(playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }

        btnNextButton.enabled = false;
        btnPreviousButton.enabled = false;

        StartCoroutine(Rotate(Vector3.up,playerSwitcherTransform,90.0f,1.0f));
        Debug.Log(playerSelectionNumber);

        if(playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            //this means the players model type is attack
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";
        }


    }

    public void PreviousPlayer()
    {

        playerSelectionNumber -= 1;

        if(playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }

        btnNextButton.enabled = false;
        btnPreviousButton.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90.0f, 1.0f));
        Debug.Log(playerSelectionNumber);

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            //this means the players model type is attack
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";
        }
    }


    public void OnSelectedButtonClicked()
    {
        uiSelection.SetActive(false);
        uiAfterSelection.SetActive(true);

        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { {MultiplayerARSpinnetTop.PLAYER_SELECTION_NUMBER,playerSelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReSelectButtonClicked()
    {
        uiSelection.SetActive(true);
        uiAfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }


    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
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
