using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [Header("Login UI")]
    public InputField playerNameInputField;

    #region UNITY_METHODS
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion


    #region UI Callback Methods

    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInputField.text;

        if(!string.IsNullOrEmpty(playerName))
        {
            if(!PhotonNetwork.IsConnected)
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
    }

    #endregion

}
