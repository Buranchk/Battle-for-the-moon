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

    public GameObject selectButton;
    public GameObject tutorialButton;

    public GameObject Energy;
    public GameObject EnergyBar1;
    public GameObject EnergyBar2;
    public GameObject EnergyBar3;


    public GameObject soundIcon;
    public GameObject noSoundIcon;

    public bool tutorialState = false;

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
            selectButton.SetActive(false);
            tutorialButton.SetActive(false);
            nameEntry.SetActive(true);
        }


    }

    public void TutorialOff()
    {
        tutorialState = false;
    }

    //remake this
    public void LoadMenuValues()
    {
        sv = Data().GetSave();
        nameSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.name;
        coinsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.gold.ToString();
        emeraldsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.ruby.ToString();
        SetEnergy(sv.energy);
        lvlSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (sv.lvl).ToString();
        SetSkinsActive(true);

        SoundSwitch();
        SoundSwitch();
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

    public void OpenInfo()
    {
        //Application.OpenURL("https://en.wikipedia.org/wiki/End-user_license_agreement");
    }

    public void OpenLisenbartGameStudio()
    {
        //Application.OpenURL("http://lisenbart.com/");
    }

    public void SoundSwitch()
    {
        if(DataMan.GetSound())
        {
            noSoundIcon.transform.localScale = noSoundIcon.transform.localScale * 1.3f;
            DataMan.SetSound(false);

            soundIcon.SetActive(false);
            noSoundIcon.SetActive(true);

            LeanTween.scale(noSoundIcon,new Vector3(1.85f, 1.85f, 1.85f), 0.1f).setEaseOutCirc();
        } 
        else if(!(DataMan.GetSound()))
        {
            
            soundIcon.transform.localScale = noSoundIcon.transform.localScale * 1.3f;
            DataMan.SetSound(true);

            soundIcon.SetActive(true);
            noSoundIcon.SetActive(false);

            LeanTween.scale(soundIcon, new Vector3(1.85f, 1.85f, 1.85f), 0.1f).setEaseOutCirc();
        }
    }


}




