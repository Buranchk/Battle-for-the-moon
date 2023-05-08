using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animation : MonoBehaviour
{
    public GameResult Resuts;
    public Image lvlNumb;
    public DataManager Data;

    public Image XP_space;
    public Image XP_Fill;


    public Sprite n0;
    public Sprite n1;
    public Sprite n2;
    public Sprite n3;
    public Sprite n4;
    public Sprite n5;
    public Sprite n6;
    public Sprite n7;
    public Sprite n8;
    public Sprite n9;
    public Sprite n10;

    public void RefreshUI()
    {
        Resuts.LoadUI();
    }

    public void ChangeNumber()
    {
        Data = GameObject.Find("Data Manager").GetComponent<DataManager>();
        int lvl = Data.GetLvl();

        switch(lvl)
        {
            case 0:
            lvlNumb.sprite = n0;
            break;

            case 1:
            lvlNumb.sprite = n1;
            break;

            case 2:
            lvlNumb.sprite = n2;
            break;

            case 3:
            lvlNumb.sprite = n3;
            break;

            case 4:
            lvlNumb.sprite = n4;
            break;

            case 5:
            lvlNumb.sprite = n5;
            break;

            case 6:
            lvlNumb.sprite = n6;
            break;

            case 7:
            lvlNumb.sprite = n7;
            break;

            case 8:
            lvlNumb.sprite = n8;
            break;

            case 9:
            lvlNumb.sprite = n9;
            break;

            case 10:
            lvlNumb.sprite = n10;
            break;

        }

    }

    public void DarkenXP()
    {
        Darken(XP_space);
        Darken(XP_Fill);
    }

    private void Darken(Image imageToDarken)
    {
        LeanTween.color(imageToDarken.rectTransform, Color.gray, 0.5f);
    }

}
