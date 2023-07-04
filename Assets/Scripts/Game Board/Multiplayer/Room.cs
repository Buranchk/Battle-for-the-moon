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

    public void SetInfo(RoomInfo info)
    {
        textName.text = info.Name;
    }
}
