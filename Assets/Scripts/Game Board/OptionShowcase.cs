using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionShowcase : MonoBehaviour
{
    public Sprite rock;
    public Sprite paper;
    public Sprite scissors;

    public GameObject myPick;
    public GameObject enemyPick;

    public void SetRPSAnimation(string myStuff, string enemyStuff)
    {


        if(myPick == enemyPick)
        {

        }
    }

    public void ChangeSprite(GameObject reciver, string exporter)
    {  
        switch(exporter)
        {
            case "rock":
            reciver.GetComponent<SpriteRenderer>.sprite = rock;
            break;
        }

    }
}
