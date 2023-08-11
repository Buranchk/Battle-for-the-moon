using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameChecker : MonoBehaviour
{
    public GameObject createRoom;

    public void CheckName(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Length >= 1)
        {
            createRoom.SetActive(true);
        }
        else
        {
            createRoom.SetActive(false);
        }
    }
}
