
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
    private DataManager DataMan;
    private bool readyCheck=false;


    public Transform buttonContainer; // The parent transform of the room buttons
    public GameObject roomButtonPrefab; // Prefab for the room button

    private List<RoomInfo> activeRooms = new List<RoomInfo>();

    // Componets of UI that need to be shown/hide
    public GameObject roomContent;
    public GameObject roomList;

    public GameObject roomNameSpace;

    [SerializeField] string enemyNickName;

    public LobbyHelper helper;    

    void Update()
    {

    } 

    void Awake()
    {
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        PhotonNetwork.ConnectUsingSettings();
        photonView = GetComponent<PhotonView>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log ("You connected to server");
        if(!PhotonNetwork.InLobby)
        PhotonNetwork.JoinLobby();
    }

    // public static PhotonManager Instance;

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }

    // }


 //Creates room with defined parametres
    public void CreateRoomButton()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers =2;
        PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default );
        roomContent.SetActive(true);
        
    }
    
    //Joins existing room
    public void JoinRoomButton()
    {
        PhotonNetwork.JoinRoom(roomName.text );
    }
    public override void OnCreatedRoom()
    {
        helper.RoomInit();
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
     }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Check if the room is full
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers )
        {
            photonView.RPC("EnemyJoin", RpcTarget.All);
            //UpdateRoomListUI();
        }

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //UpdateRoomListUI();
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
            roomButton.GetComponentInChildren<TMP_Text>().text = room.Name;

            //Set random color for room button
            Color randomColor = GetRandomColor();
            roomButton.image.color = randomColor;



            // Add a click event to join the room
            roomButton.onClick.AddListener(() => JoinRoom(room.Name));
        }
        Debug.Log("RoomList UI has been updated");
    }

    private Color GetRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        return new Color(r, g, b);
    }


    public void RefreshRoomList()
    {
        if (PhotonNetwork.InLobby)
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinLobby();
    }

    private void JoinRoom(string nameOfExistingRoom)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers =2;
        PhotonNetwork.JoinOrCreateRoom (nameOfExistingRoom, roomOptions, TypedLobby.Default);
        //PhotonNetwork.JoinRoom(nameOfExistingRoom);
    }



    public override void OnJoinedRoom()
    {
        //Sets roomname in room UI
        string roomNameUI = PhotonNetwork.CurrentRoom.Name;
        helper.roomNameUI = roomNameUI;
        roomNameSpace.GetComponent<TMP_Text>().text= roomNameUI;

        Debug.Log ("You joined room: " + PhotonNetwork.CurrentRoom.Name );

        //Set room UI in active state
        roomContent.SetActive(true);
        roomList.SetActive(false);

        //Sets enemy name for other player in LobbyHelper
        photonView.RPC("SendNickName", RpcTarget.OthersBuffered);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room " + roomName.text);
    }

    public void LeaveRoomButton()
    {
        photonView.RPC("OpponentLeftRoom", RpcTarget.Others);
        PhotonNetwork.LeaveRoom();
        roomList.SetActive(true);
        roomContent.SetActive(false);
    }
    [PunRPC]
    public void OpponentLeftRoom()
    {
        readyCheck=false;
        helper.RoomInit();
        Debug.Log("RPC OpponentLeftRoom was sent");
    }

    public void ExitLobbyButton()
    {
        photonView.RPC("OpponentLeftRoom", RpcTarget.Others);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        readyCheck=false;
        Debug.Log("You left room");
    }
    
    //starts game if both player are ready
    public void StartGame()
    {
        helper.UserIsReady();

        if(readyCheck)
        {
            readyCheck = false;
            photonView.RPC("LoadScene", RpcTarget.All); //stars match if another player sent true value of readyCheck
        }
        else
        {
            photonView.RPC("Ready", RpcTarget.Others);  //sets value of readyCheck to true for another palyer
        }
        Debug.Log ("You are ready");
    }
    [PunRPC]
    public void LoadScene()
    {
        PhotonNetwork.LoadLevel("Multiplayer Game Board");
        AudioMusic.Instance.GameMusic();

    }
    [PunRPC]
    public bool Ready()
    {
        helper.EnemyIsReady();
        readyCheck=true;
        return readyCheck;
    }
    public bool NotReady()
    {
        return readyCheck;
    }

    [PunRPC]
    public void EnemyJoin()
    {
        helper.RoomInit();
        helper.RoomConnect();
        Debug.Log("Enemy joined room!");
    }

    [PunRPC]
    public void SendNickName()
    {
        helper.enemyNamePM = DataMan.GetName();
    }





}

