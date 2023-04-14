using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitch : MonoBehaviour
{
    public bool isFlag;

    public Sprite marineFlag;
    public Sprite marineDecoy;
    public Sprite goldFlag;
    public Sprite goldDecoy;
    public Sprite xp2Flag;
    public Sprite xp2Decoy;
    public Sprite rubyFlag;
    public Sprite rubyDecoy;
 
    void Awake()
    {

        //Sprite objSprite = gameObject.GetComponent<Image>().sprite;
        int selectedskin = GameObject.Find("Data Manager").GetComponent<DataManager>().GiveSelectedSkin();
        if(isFlag)
        {
            switch (selectedskin){
                case 0:
                gameObject.GetComponent<Image>().sprite = marineFlag;
                break;

                case 1:
                gameObject.GetComponent<Image>().sprite = xp2Flag;
                gameObject.GetComponent<RectTransform>().localScale *= 1.4f;
                break;

                case 2:
                gameObject.GetComponent<Image>().sprite = goldFlag;
                gameObject.GetComponent<RectTransform>().localScale *= 1.4f;
                break;

                case 3:
                gameObject.GetComponent<Image>().sprite = rubyFlag;
                gameObject.GetComponent<RectTransform>().localScale *= 1.4f;
                break;
            }
        }
        else
        {
            switch (selectedskin){
                case 0:
                gameObject.GetComponent<Image>().sprite = marineDecoy;
                break;

                case 1:
                gameObject.GetComponent<Image>().sprite = xp2Decoy;
                gameObject.GetComponent<RectTransform>().localScale *= 1.4f;
                break;

                case 2:
                gameObject.GetComponent<Image>().sprite = goldDecoy;
                gameObject.GetComponent<RectTransform>().localScale *= 1.4f;
                break;

                case 3:
                gameObject.GetComponent<Image>().sprite = rubyDecoy;
                gameObject.GetComponent<RectTransform>().localScale *= 1.4f;
                break;
            }
        }
    }

}
