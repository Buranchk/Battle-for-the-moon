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

    public void SetUpPrice(int type)
    {
        switch(type)
        {
            case 1:
            Description.GetComponent<TMPro.TextMeshProUGUI>().text = "XP is doubled";
            gameObject.GetComponent<Image>().sprite = gold;
            Price.GetComponent<TMPro.TextMeshProUGUI>().text = "550";
            break;

            case 2:
            Description.GetComponent<TMPro.TextMeshProUGUI>().text = "Gold is doubled";
            gameObject.GetComponent<Image>().sprite = XP;
            Price.GetComponent<TMPro.TextMeshProUGUI>().text = "5";
            break;

            case 3:
            Description.GetComponent<TMPro.TextMeshProUGUI>().text = "Rubins are doubled";
            gameObject.GetComponent<Image>().sprite = ruby;
            Price.GetComponent<TMPro.TextMeshProUGUI>().text = "150";
            break;
        }

    }

}
