using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public int rubyPriceForEnergy = 5;
    public float moneyPriceForNoAds = 1.99f;
    public float moneyPriceForRuby = 0.99f;
    private float updXP;
    public Slider XPSlider;

    public GameObject powerSpace;
    public GameObject EnergyBar1;
    public GameObject EnergyBar2;
    public GameObject EnergyBar3;
    public GameObject lvlSpace;


    private DataManager DataMan;
    private SceneLoader SceneLoad;

    void Start()
    {
        
        //moneyPriceForNoAds GET

        //moneyPriceForRuby
        SceneLoad = GameObject.Find("Scene loader").GetComponent<SceneLoader>();
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();

        lvlSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (DataMan.GetLvl()).ToString();

        goldSpace.text = DataMan.GetGold().ToString();
        rubySpace.text = DataMan.GetRuby().ToString();

        goldPriceForLvlUp = (DataMan.GetXP() * 2);

        price1.text = (goldPriceForLvlUp).ToString();
        price2.text = (rubyPriceForGold).ToString();
        price3.text = (rubyPriceForEnergy).ToString();
        price4.text = (moneyPriceForNoAds).ToString() + " $";
        price5.text = (moneyPriceForRuby).ToString() + " $";


        
        //XPSlider.value = 0.5f;
        FillXP(0);

        ShowEnergy();

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
        if(DataMan.CheckResources("ruby", 5) && !DataMan.CheckResources("energy", 60))
        {
            DataMan.TakeResources("ruby", 5); 
            DataMan.FillEnergy();
            SceneLoad.ReloadScene();
        }
        else if (DataMan.CheckResources("energy", 60))
        {
            print("ur energy is full");
        }
        else if (!DataMan.CheckResources("ruby", 5))
        {
            print("not enough ruby");
        }
        ShowEnergy();

    }

    public void ShowEnergy()
    {
        int energy = DataMan.GetPower();
        if(energy > 60)
            energy = 60;
        powerSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (energy.ToString() + "/60");
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

    public void FillXP(int xp)
    {
        int[] lvls = new int[12]{0, 5, 10, 15, 25, 35, 50, 70, 90, 120, 150, 100000};
        bool nextLevel = false;


        if(DataMan.GetLvl() == 10)
        {
            //ShowFullXPBar + MaxLVL
        }
        else
        {
            float first = (DataMan.GetXP() - lvls[DataMan.GetLvl()]);
            float second = (lvls[DataMan.GetLvl() + 1] - lvls[DataMan.GetLvl()]);

            updXP = first / second;
            print(first);
            print(second);
            print(updXP);
        }
    }


    private void Update()
    {
        XPSlider.value = updXP;
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
