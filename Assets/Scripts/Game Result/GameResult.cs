using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    public Sprite Enemy;
    public Sprite Player;
    // Start is called before the first frame update
    void Start()
    {
        int gameResult = PlayerPrefs.GetInt("GameResult");
        if(gameResult == 1)
            GameObject.Find("Winner").GetComponent<Image>().sprite = Player;    
        else  
            GameObject.Find("Winner").GetComponent<Image>().sprite = Enemy;      
    }
}
