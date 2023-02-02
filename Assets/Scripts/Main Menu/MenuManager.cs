using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject Skins;
    public GameObject nameEntry;
    public GameObject applyButton;

    public GameObject nameSpace;
    public GameObject coinsSpace;
    public GameObject emeraldsSpace;
    public GameObject powerSpace;
    public GameObject lvlSpace;

    public GameObject Ruby;
    public GameObject XP;


    private DataManager DataMan;
    private string naming;
    private Save sv = new Save();

    private void Start()
    {
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        if (Data().LoadSave())
        {
            Data().LoadSave();
            LoadMenuValues();
        }
        else
        {
            SetSkinsActive(false);
            nameEntry.SetActive(true);
        }
    }

    //remake this
    public void LoadMenuValues()
    {
        sv = Data().GetSave();
        nameSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.name;
        coinsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.coins.ToString();
        emeraldsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.emeralds.ToString();
        if(sv.power > 100)
            sv.power = 100;
        powerSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (sv.power.ToString() + "/100");
        lvlSpace.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)sv.lvl).ToString();
        Data().SetSave(sv);
        SetSkinsActive(true);
    }

    public void CheckName(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Length >= 3)
        {
            applyButton.SetActive(true);
            naming = name;
        }
        else
        {
            applyButton.SetActive(false);
            Debug.Log("Enter a proper name!");
        }
    }

    public void NewAccount()
    {
        //find the data manager
        Data().InitializeAccount(naming);
    }

    //take to the data manager
    public void TakePower()
    {
        Data().TakePower();
    }

    public void SetSkinsActive(bool result)
    {
        Skins.SetActive(result);
    }
    
    private DataManager Data()
    {
        return DataMan;
    }
/*  
++    //Add coin/emerald/power/lvl data
++    //Initialize coin/emerald/power/lvl data
++    //Load coin/emerald/power/lvl data
--    //Load unlocked skins, Last selected skin, some other data
--    //Load info bout previous games, W|L
    
++    //Change power lvl, when pressed "Play"
--    //Change last selected skin
    //Change unlocked skins
    
*/
}




