using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Profile : MonoBehaviour
{
    public GameObject SkinCaseMarine;
    public GameObject SkinCaseXP;
    public GameObject SkinCaseGold;
    public GameObject SkinCaseRuby;

    public TextMeshProUGUI nameSpace;

    private DataManager DataMan;
    

    void Start()
    {
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        nameSpace.text = DataMan.GetName();
        
        List<bool> skins = DataMan.GetSkins();
        List<GameObject> localSkins = new List<GameObject>{SkinCaseMarine, SkinCaseXP, SkinCaseGold, SkinCaseRuby};

        for(int i = 0; i < 4; i++)
        {
            localSkins[i].SetActive(!skins[i]);
        }
    }
}
