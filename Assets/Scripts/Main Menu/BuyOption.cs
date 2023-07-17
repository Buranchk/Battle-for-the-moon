using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyOption : MonoBehaviour
{
    public Sprite gold;
    public Sprite ruby;
    public Sprite XP;
    public GameObject Price;
    public GameObject Description;
    public int skinType;
    private DataManager DataMan;
    public int amount;
    public string currency;

    public void Start()
    {
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
    }

    public void SetUpPrice(int type)
    {
        skinType = type;
        switch(type)
        {
            case 1:
            Description.GetComponent<TMPro.TextMeshProUGUI>().text = "XP is doubled";
            gameObject.GetComponent<Image>().sprite = gold;
            Price.GetComponent<TMPro.TextMeshProUGUI>().text = "550";
            amount = 550;
            currency = "gold";
            break;

            case 2:
            Description.GetComponent<TMPro.TextMeshProUGUI>().text = "Gold is doubled";
            gameObject.GetComponent<Image>().sprite = XP;
            Price.GetComponent<TMPro.TextMeshProUGUI>().text = "5";
            amount = 5;
            currency = "lvl";
            break;

            case 3:
            Description.GetComponent<TMPro.TextMeshProUGUI>().text = "Rubins are doubled";
            gameObject.GetComponent<Image>().sprite = ruby;
            Price.GetComponent<TMPro.TextMeshProUGUI>().text = "150";
            amount = 150;
            currency = "ruby";
            break;
        }

    }

    public void BuySkin()
    {
        if(DataMan.CheckResources(currency, amount))
        {
            DataMan.OpenSkin(skinType);
            ShowPurchase();
            DataMan.TakeResources(currency, amount);
            GameObject.Find("Scene loader").GetComponent<SceneLoader>().ReloadScene();
        } else 
        {
            ShowFail();
            GameObject.Find("Scene loader").GetComponent<SceneLoader>().LoadShop();
        }
    }

    private void ShowPurchase()
    {
        //play some anim? mb unity shitnimator
        //mb just button collapce and smth like OWNED on the character?
    }

    private void ShowFail()
    {
        // not enough resources sign, button goes red? 
    }


}
