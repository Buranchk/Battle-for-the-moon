using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    public Sprite Enemy;
    public Sprite Player;
    public GameObject Result;
    public DataManager Data;
    public GameObject coinsSpace;
    public GameObject emeraldsSpace;
    public GameObject powerSpace;
    public GameObject xpSpace;
    public GameObject coinsRewards;
    public GameObject emeraldsRewards;
    public Slider XPSlider;
    public Animator AnimatedPart;
    private float updXP;

    private Save sv = new Save();


    // Start is called before the first frame update
    void Start()
    {
        Data = GameObject.Find("Data Manager").GetComponent<DataManager>();
        int gameResult = PlayerPrefs.GetInt("GameResult");


        LoadUI();
        RewardCalculation(gameResult);
        
        if(gameResult == 1){
            //20 2 5
            Result.SetActive(true);
            Data.EndGame(true, Data.GiveSelectedSkin());
        }
        else 
        {
            //5 0 2
            Result.SetActive(false);
            Data.EndGame(false, Data.GiveSelectedSkin());
        }

    }


    private void RewardCalculation(int gameResult)
    {
        int coinReward = 0;
        int emeraldReward = 0;
        int xpReward = 0;

        if(gameResult == 1)
        {
            print("u won");
            coinReward = 20;
            emeraldReward = 2;
            xpReward = 5;
                

            switch (Data.GiveSelectedSkin()){
                case 1:
                xpReward += 5;
                break;

                case 2:
                coinReward += 20;
                break;

                case 3:
                emeraldReward += 2;
                break;
            }
        }
        else
        {
            print("u lost");
            coinReward = 5;
            xpReward = 2;
            emeraldReward = 0;
            switch (Data.GiveSelectedSkin()){
                case 1:
                xpReward += 2;
                break;

                case 2:
                coinReward += 5;
                break;

                case 3:
                emeraldReward += 1;
                break;
            }  
        }

        coinsRewards.GetComponent<TMPro.TextMeshProUGUI>().text = ("+ " + coinReward.ToString());
        emeraldsRewards.GetComponent<TMPro.TextMeshProUGUI>().text = ("+ " + emeraldReward.ToString());
        FillXP(xpReward);
        xpSpace.GetComponent<TMPro.TextMeshProUGUI>().text = ("+ " + xpReward.ToString());
    }




    private void SetUpXPSlider(int lvl, int xp)
    {
        int[] lvls = new int[12]{0, 5, 10, 15, 25, 35, 50, 70, 90, 120, 150, 100000};
        

        XPSlider.maxValue = lvls[lvl + 1] - lvls[lvl];
        print("maxValue is " + (lvls[lvl + 1] - lvls[lvl]) + " lvl is " + lvl);
        XPSlider.value = xp - lvls[lvl];
        print("value is " + XPSlider.value + " xp is " + xp);

    }

    public void FillXP(int xp)
    {
        int[] lvls = new int[12]{0, 5, 10, 15, 25, 35, 50, 70, 90, 120, 150, 100000};
        sv = Data.GetSave();

        LeanTween.value(GameObject.Find("XPSlider"), XPSlider.value, XPSlider.value + xp, 1.0f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) => {
            print(val);
            updXP = val;
            if(val >= XPSlider.maxValue)
            {
               //lvlUpAnimation.Play(true);
            }
        });



        // AnimatedPart.GetComponent<AnimationClip>().events
        // XPSlider.GetComponent<Slider>().value + xp
    }


    private void Update()
    {
        XPSlider.value = updXP;
    }

    public void LoadUI()
    {
        sv = Data.GetSave();
        coinsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.coins.ToString();
        emeraldsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.emeralds.ToString();
        if(sv.power > 100)
            sv.power = 100;
        powerSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (sv.power.ToString() + "/100");
        SetUpXPSlider(sv.lvl, sv.xp);
    }


}
