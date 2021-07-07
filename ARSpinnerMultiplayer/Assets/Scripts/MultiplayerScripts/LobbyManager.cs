using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject uiLoginGameObject;

    [Header("Lobby UI")]
    public GameObject uiLobbyGameObject;
    public GameObject ui3DGameObject;

    [Header("Connection Status UI")]
    public GameObject uiConnectionStatusGameObject;
    public Text connectionStatusText;
    public bool showConnectionStatus = false;


    #region UNITY_METHODS
    // Start is called before the first frame update
    void Start()
    {
        uiLobbyGameObject.SetActive(false);
        ui3DGameObject.SetActive(false);
        uiConnectionStatusGameObject.SetActive(false);

        uiLoginGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(showConnectionStatus)
        {
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
        }

       
    }
    #endregion


    #region UI Callback Methods

    public void OnEnterGameButtonClicked()
    {
       

       

        string playerName = playerNameInputField.text;

        if(!string.IsNullOrEmpty(playerName) && playerName.Length >= 6)
        {
            uiLobbyGameObject.SetActive(false);
            ui3DGameObject.SetActive(false);
            uiLoginGameObject.SetActive(false);


            uiConnectionStatusGameObject.SetActive(true);

            showConnectionStatus = true;

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;

                PhotonNetwork.ConnectUsingSettings(); //->this method will connect us to photon
            }
        }
        else
        {
            Debug.Log("<color=red> Player Name is Invalid </color>");
        }
    }

    public void OnQuickMatchButtonClicked()
    {
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }

    #endregion



    #region PHOTON Callbacks Methods

    //this method is called when the intercet conection is established. 
    public override void OnConnected()
    {
        Debug.Log("We Connect To internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("<color=green> " + PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server </color>");

       
        uiLoginGameObject.SetActive(false);
        uiConnectionStatusGameObject.SetActive(false);

        uiLobbyGameObject.SetActive(true);
        ui3DGameObject.SetActive(true);
    }

    #endregion

}
