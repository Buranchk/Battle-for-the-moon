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
    private Save sv = new Save();


    // Start is called before the first frame update
    void Start()
    {
        Data = GameObject.Find("Data Manager").GetComponent<DataManager>();
        int gameResult = PlayerPrefs.GetInt("GameResult");
        if(gameResult == 1){
            GameObject.Find("Winner").GetComponent<Image>().sprite = Player;
            GameObject.Find("Result").GetComponent<Image>().sprite = Win;
            //20 2 5
            Data.EndGame(true);
        }
        else 
        {
            GameObject.Find("Winner").GetComponent<Image>().sprite = Enemy;
            GameObject.Find("Result").GetComponent<Image>().sprite = Loose;
            //5 0 2
            Data.EndGame(false);
        }
        LoadUI();
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
