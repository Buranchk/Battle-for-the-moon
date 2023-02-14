using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    public Sprite Enemy;
    public Sprite Player;
    public Sprite Win;
    public Sprite Loose;
    public DataManager Data;
    public GameObject coinsSpace;
    public GameObject emeraldsSpace;
    public GameObject powerSpace;
    public GameObject xpSpace;
    public GameObject coinsRewards;
    public GameObject emeraldsRewards;
    public GameObject xpRewards;

    private Save sv = new Save();


    // Start is called before the first frame update
    void Start()
    {
        Data = GameObject.Find("Data Manager").GetComponent<DataManager>();
        int gameResult = PlayerPrefs.GetInt("GameResult");

        LoadUI();
        RewardCalculation(gameResult);
        
        if(gameResult == 1){
            GameObject.Find("Winner").GetComponent<Image>().sprite = Player;
            GameObject.Find("Result").GetComponent<Image>().sprite = Win;
            //20 2 5
            Data.EndGame(true, Data.GiveSelectedSkin());
        }
        else 
        {
            GameObject.Find("Winner").GetComponent<Image>().sprite = Enemy;
            GameObject.Find("Result").GetComponent<Image>().sprite = Loose;
            //5 0 2
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
        //xpRewards.GetComponent<TMPro.TextMeshProUGUI>().text = ("+ " + xpReward.ToString());
    }


    public void LoadUI()
    {
        sv = Data.GetSave();
        coinsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.coins.ToString();
        emeraldsSpace.GetComponent<TMPro.TextMeshProUGUI>().text = sv.emeralds.ToString();
        if(sv.power > 100)
            sv.power = 100;
        powerSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (sv.power.ToString() + "/100");
        xpSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (sv.xp).ToString();
    }


}
