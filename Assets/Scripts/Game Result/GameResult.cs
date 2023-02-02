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
    // Start is called before the first frame update
    void Start()
    {
        Data = GameObject.Find("Data Manager").GetComponent<DataManager>();
        int gameResult = PlayerPrefs.GetInt("GameResult");
        if(gameResult == 1){
            GameObject.Find("Winner").GetComponent<Image>().sprite = Player;
            GameObject.Find("Result").GetComponent<Image>().sprite = Win;
            Data.EndGame(true);
        }
        else 
        {
            GameObject.Find("Winner").GetComponent<Image>().sprite = Enemy;
            GameObject.Find("Result").GetComponent<Image>().sprite = Loose;
            Data.EndGame(false);
        }
    }
}
