using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpinningTopGameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject uiInformPanelGameobject;
    public TextMeshProUGUI uiInformText;
    public GameObject searchForGamesButtonGameobject;

    private void Start()
    {
        uiInformPanelGameobject.SetActive(true);
        uiInformText.text = "Search For Games to Battle!";
    }


    #region UI Callbacks Methods
    public void JoinRandomRoom()
    {

        uiInformText.text = "Searching For available rooms...";

        PhotonNetwork.JoinRandomRoom();

        searchForGamesButtonGameobject.SetActive(false);

    }

    #endregion

    #region Photon Callback Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);

        //Debug.Log(message);
        uiInformText.text = message;
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uiInformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + " . Waiting For Other players...";
        }
        else
        {
            uiInformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(uiInformPanelGameobject, 2.0f));
        }


        //Debug.Log(PhotonNetwork.NickName + " Joined to " + PhotonNetwork.CurrentRoom.Name);
        //Debug.Log(PhotonNetwork.NickName + " Joined to with " + PhotonNetwork.CurrentRoom.PlayerCount + " of players");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //base.OnPlayerEnteredRoom(newPlayer);
        //Debug.Log(newPlayer  + " Join to " + PhotonNetwork.CurrentRoom.Name + " Player Count " + PhotonNetwork.CurrentRoom.PlayerCount);

        uiInformText.text = newPlayer + " Join to " + PhotonNetwork.CurrentRoom.Name + " Player Count " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactivateAfterSeconds(uiInformPanelGameobject,2.0f));
    }

    #endregion

    #region PRIVATE METHODS

    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room: " + Random.Range(0,1000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        

        //creating the room
        PhotonNetwork.CreateRoom(randomRoomName,roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject gameObject,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    #endregion
}
