using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceStore : MonoBehaviour
{
    public TextMeshProUGUI goldSpace;
    public TextMeshProUGUI rubySpace;


    public TextMeshProUGUI price1;
    public TextMeshProUGUI price2;
    public TextMeshProUGUI price3;
    public TextMeshProUGUI price4;
    public TextMeshProUGUI price5;

    public int goldPriceForLvlUp;
    public int rubyPriceForGold = 10;
    public int rubyPriceForEnergy = 10;
    public int moneyPriceForNoAds;
    public int moneyPriceForRuby;


    private DataManager DataMan;
    private SceneLoader SceneLoad;

    void Start()
    {

        //moneyPriceForNoAds GET

        //moneyPriceForRuby
        SceneLoad = GameObject.Find("Scene loader").GetComponent<SceneLoader>();
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();

        goldSpace.text = DataMan.GetGold().ToString();
        rubySpace.text = DataMan.GetRuby().ToString();

        goldPriceForLvlUp = (DataMan.GetXP() * 2);

        price1.text = (goldPriceForLvlUp).ToString();
        price2.text = (rubyPriceForGold).ToString();
        price3.text = (rubyPriceForEnergy).ToString();
        price4.text = (moneyPriceForNoAds).ToString();
        price5.text = (moneyPriceForRuby).ToString();

    }

    public void GoldToXP()
    {
        if(DataMan.CheckResources("gold", goldPriceForLvlUp))
        {
            DataMan.TakeResources("gold", goldPriceForLvlUp);
            DataMan.NextLvl();
            SceneLoad.ReloadScene();
        }
        else
        {
            //not enough!!!!
        }
    }

    public void RubyToGold()
    {
        if(DataMan.CheckResources("ruby", rubyPriceForGold))
        {
            DataMan.TakeResources("ruby", rubyPriceForGold);
            DataMan.GiveGold(rubyPriceForGold * 10);
            SceneLoad.ReloadScene();
        }
        else
        {

        }
    }

    public void RubyToEnergy()
    {
        if(DataMan.CheckResources("ruby", 5))
        {
            DataMan.TakeResources("ruby", 5); 
            DataMan.FillEnergy();
            SceneLoad.ReloadScene();
        }
        else
        {

        }

    }

    public void MonyNoAd()
    {
        //some function
    }

    public void MoneyToRuby()
    {
        //some function too
    }

}
