using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Room : MonoBehaviour
{
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text playerCount;

    public void SetInfo(RoomInfo info)
    {
        textName.text = info.Name;
        playerCount.text = info.PlayerCount +"/" + info.MaxPlayers;
    }
}
