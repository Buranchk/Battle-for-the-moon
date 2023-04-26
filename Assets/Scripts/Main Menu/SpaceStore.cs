using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceStore : MonoBehaviour
{
    public TextMeshProUGUI goldSpace;
    public TextMeshProUGUI rubySpace;

    private DataManager DataMan;
    

    void Start()
    {
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        goldSpace.text = DataMan.GetGold().ToString();
        rubySpace.text = DataMan.GetRuby().ToString();
    }
}
