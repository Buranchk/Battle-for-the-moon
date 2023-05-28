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
    public GameObject showCase;
    public SkinPreview skinPreview;
    public BuyOption buyOption;
    public GameObject ShopCanvasScaler;
    public GameObject DarkenClose;
    public GameObject Energy;
    public GameObject EnergyBar1;
    public GameObject EnergyBar2;
    public GameObject EnergyBar3;

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
        coinsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.gold.ToString();
        emeraldsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.ruby.ToString();
        if(sv.energy > 60)
            sv.energy = 60;
        powerSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (sv.energy.ToString() + "/60");
        SetEnergy(sv.energy);
        lvlSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (sv.lvl).ToString();
        SetSkinsActive(true);
    }

    public void SetEnergy(int energy)
    {

        switch (energy)
        {
            case int val when val < 20:
                print("ur shit is empty");
                break;
            case int val when val >= 20 && val < 40:
                EnergyBar1.SetActive(true);
                break;
            case int val when val >= 40 && val < 60:
                EnergyBar1.SetActive(true);
                EnergyBar2.SetActive(true);
                break;
            case int val when val == 60:
                EnergyBar1.SetActive(true);
                EnergyBar2.SetActive(true);
                EnergyBar3.SetActive(true);
                break;
            default:
                print("something is wrong");
                break;
        }
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

    public void HideCaseSkin()
    {
        ShopCanvasScaler.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        showCase.SetActive(false);
        DarkenClose.GetComponent<Image>().color = new Vector4(0f,0f,0f,0f);
    }
    
    public void ShowCaseSkin(int typeSkin)
    {
        DarkenClose.GetComponent<Image>().color = new Vector4(0f,0f,0f,0.3f);
        ShopCanvasScaler.GetComponent<RectTransform>().LeanScale(new Vector3(1f, 1f, 1f), 0.2f).setEase(LeanTweenType.easeOutCirc);
        showCase.SetActive(true);
        skinPreview.Appear(typeSkin);
        buyOption.SetUpPrice(typeSkin);
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




