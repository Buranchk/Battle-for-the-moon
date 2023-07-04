
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string region;
    [SerializeField] public TMP_InputField roomName;
    [SerializeField] Room itemPrefab;
    [SerializeField] Transform content;
    private PhotonView photonView;
    public GameObject roomContent;
    

    void Update()
    {

    } 

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        photonView = GetComponent<PhotonView>();
        roomContent.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log ("You connected to server");
        if(!PhotonNetwork.InLobby)
        PhotonNetwork.JoinLobby();
    }

//Creates room with defined parametres
    public void CreateRoomButton()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers =2;
        PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default );
        
    }
    
    //Joins existing room
    public void JoinRoomButton()
    {
        PhotonNetwork.JoinRoom(roomName.text );
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room with name: "+ PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Multiplayer Game Board");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room");
    }
    
    // public override void OnRoomListUpdate(List<RoomInfo> roomList)
    // {
    //     foreach (RoomInfo info in roomList)
    //     {
    //         Room listItem =Instantiate(itemPrefab, content);
    //         if(listItem != null)
    //             listItem.SetInfo(info);
    //     }
    // }

    public override void OnJoinedRoom()
    {
        Debug.Log ("You joined room: " + PhotonNetwork.CurrentRoom.Name );
        PhotonNetwork.LoadLevel("Multiplayer Game Board");
        //photonView.RPC("LoadScene", RpcTarget.All);

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room " + roomName.text);
    }

    public void ExitButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("You left room");
    }
    [PunRPC]
    public void LoadScene()
    {
        PhotonNetwork.LoadLevel("Multiplayer Game Board");
    }
}
