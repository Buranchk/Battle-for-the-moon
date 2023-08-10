
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
    private string secondPlayerNick;

    public LobbyHelper helper;    


    void Awake()
    {
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        PhotonNetwork.ConnectUsingSettings();
        photonView = GetComponent<PhotonView>();
        if(!PhotonNetwork.InLobby)
        PhotonNetwork.JoinLobby();
        PhotonNetwork.NickName=DataMan.GetName();
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
        if (roomName.text.Length > 0)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
            roomContent.SetActive(true);

            GameObject.Find("Room").SetActive(true);
            GameObject.Find("RoomList").SetActive(false);
            GameObject.Find("Controller").GetComponent<LobbyHelper>().RoomInit();
        }

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
            if (room.PlayerCount!=2)
            {
                GameObject button = Instantiate(roomButtonPrefab, buttonContainer);
                Button roomButton = button.GetComponent<Button>();

                // Set button label to room name and number of players
                roomButton.GetComponentInChildren<TMP_Text>().text = room.Name;

                //Set random color for room button
                Color randomColor = GetRandomColor();
                roomButton.GetComponentInChildren<TMP_Text>().color = randomColor;



                // Add a click event to join the room
                roomButton.onClick.AddListener(() => JoinRoom(room.Name));
            }
        }
        Debug.Log("RoomList UI has been updated");
    }

    private Color[] colorList = new Color[]
    {
        new Color(1f, 0f, 0f),          // Red
        new Color(0f, 1f, 0f),          // Lime
        new Color(0f, 0f, 1f),          // Blue
        new Color(1f, 1f, 0f),          // Yellow
        new Color(0f, 1f, 1f),          // Cyan
        new Color(1f, 0f, 1f),          // Magenta / Fuchsia
        new Color(1f, 0.27f, 0f),       // Orange Red
        new Color(1f, 0.55f, 0f),       // Dark Orange
        new Color(1f, 0.65f, 0f),       // Orange
        new Color(1f, 0.84f, 0f),       // Gold
        new Color(0.49f, 0.99f, 0f),    // Lawn Green
        new Color(0.5f, 1f, 0f),        // Chartreuse
        new Color(0.68f, 1f, 0.18f),    // Green Yellow
        new Color(0f, 0.73f, 1f),       // Deep Sky Blue
        new Color(0.12f, 0.57f, 1f),    // Dodger Blue
        new Color(0.25f, 0.41f, 0.88f), // Royal Blue
        new Color(0.54f, 0.17f, 0.89f), // Blue Violet
        new Color(0.85f, 0.44f, 0.84f), // Orchid
        new Color(0.78f, 0.08f, 0.52f), // Medium Violet Red
        new Color(0.86f, 0.44f, 0.58f), // Pale Violet Red
        new Color(1f, 0.08f, 0.58f),    // Deep Pink
        new Color(1f, 0.41f, 0.71f),    // Hot Pink
        new Color(0.82f, 0.41f, 0.12f), // Chocolate
        new Color(0.8f, 0.52f, 0.25f),  // Peru
        new Color(0.96f, 0.64f, 0.38f), // Sandy Brown
        new Color(0.58f, 0f, 0.83f),    // Dark Violet
        new Color(0.6f, 0.2f, 0.8f),    // Dark Orchid
        new Color(0.73f, 0.33f, 0.83f), // Medium Orchid
        new Color(0.5f, 0f, 0.5f)       // Purple
    };

    private Color GetRandomColor()
    {
        int randomIndex = Random.Range(0, colorList.Length);
        return colorList[randomIndex];
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

        // Check if the room is full
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers )
        {   
            photonView.RPC("EnemyJoin", RpcTarget.All);
        }
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
        if(!DataMan.GetPremiumStatus())
            DataMan.TakePower();
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
        //Sets enemy name for other player in LobbyHelper
        foreach (var player in PhotonNetwork.PlayerListOthers)
        {
            helper.enemyNamePM = player.NickName;
            break; 
        }
        
        helper.RoomInit();
        helper.RoomConnect();
        Debug.Log("Enemy joined room!");
    }

}

