
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
    private PhotonView photonView;
    private bool readyCheck=false;


    public Transform buttonContainer; // The parent transform of the room buttons
    public GameObject roomButtonPrefab; // Prefab for the room button

    private List<RoomInfo> activeRooms = new List<RoomInfo>();


    // [SerializeField] Room itemPrefab;
    // [SerializeField] Transform content;



    // Componets of UI that need to be shown/hide
    public GameObject roomContent;
    public GameObject scrollView;


    

    void Update()
    {

    } 

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        photonView = GetComponent<PhotonView>();
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
        //PhotonNetwork.LoadLevel("Multiplayer Game Board");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room");
    }
    
     public override void OnRoomListUpdate(List<RoomInfo> roomList)
     {
        activeRooms.Clear();
        activeRooms.AddRange(roomList);
        UpdateRoomListUI();

        //  foreach (RoomInfo info in roomList)
        //  {
        //      Room listItem =Instantiate(itemPrefab, content);
        //      if(listItem != null)
        //          listItem.SetInfo(info);
        //}
     }

    private void UpdateRoomListUI()
    {
        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate buttons for each active room
        foreach (RoomInfo room in activeRooms)
        {
            GameObject button = Instantiate(roomButtonPrefab, buttonContainer);
            Button roomButton = button.GetComponent<Button>();

            // Set button label to room name and number of players
            roomButton.GetComponentInChildren<Text>().text = $"{room.Name}";

            // Add a click event to join the room
            roomButton.onClick.AddListener(() => JoinRoom(room.Name));
        }
    }

    private void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }



    public override void OnJoinedRoom()
    {
        Debug.Log ("You joined room: " + PhotonNetwork.CurrentRoom.Name );
        scrollView.SetActive(false);
        roomContent.SetActive(true);

        //PhotonNetwork.LoadLevel("Multiplayer Game Board");
        //photonView.RPC("LoadScene", RpcTarget.All);

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room " + roomName.text);
    }

    public void ExitButton()
    {
        PhotonNetwork.LeaveRoom();
        scrollView.SetActive(true);
        roomContent.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("You left room");
    }
    public void StartGame()
    {
        if(readyCheck)
        {
            readyCheck = false;
            photonView.RPC("LoadScene", RpcTarget.All);
        }
        else
        {
            photonView.RPC("Ready", RpcTarget.Others);
        }
        Debug.Log ("You are ready");
    }
    [PunRPC]
    public void LoadScene()
    {
        PhotonNetwork.LoadLevel("Multiplayer Game Board");
    }
    [PunRPC]
    public bool Ready()
    {
        readyCheck=true;
        return readyCheck;
    }
}

