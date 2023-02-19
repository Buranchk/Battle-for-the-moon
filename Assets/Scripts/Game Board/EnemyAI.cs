using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject highlight;
    public Sprite closed;
    public Sprite rockOpen;
    public Sprite paperOpen;
    public Sprite scissorsOpen;

    //permanent statement
    public bool playerUnit = false;

    //condition
    public bool isOpen = false;
    public string type = "Nothing";
    public bool isOverTheUnit = false;
    public bool movedOn;

    public void Init()
    {
        highlight.SetActive(false);
    }

    void OnMouseOver()
    {
        if(GetGameBoard().windowRPS.activeInHierarchy == false)
        {
            isOverTheUnit = true;
            if (Input.GetMouseButtonDown(0))
            {
                if (movedOn && GetGameBoard().gameStage == 4)
                    GetGameBoard().AttackEnemy(gameObject);
            }
        }
    }

    public void ChangeType(string newType)
    {
        type = newType;
        if(isOpen)
        {
            switch (newType)
            {

                case "rock":
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = rockOpen;
                    highlight.GetComponent<SpriteRenderer>().sprite = rockOpen;
                    break;

                case "paper":
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = paperOpen;
                    highlight.GetComponent<SpriteRenderer>().sprite = paperOpen;
                    break;

                case "scissors":
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = scissorsOpen;
                    highlight.GetComponent<SpriteRenderer>().sprite = scissorsOpen;
                    break;
            }
        }else if (!isOpen)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = closed;
            highlight.GetComponent<SpriteRenderer>().sprite = closed;
        }
        
    }

    //Unit highlight exit
    GameBoard GetGameBoard()
    {
        return GameObject.Find("Game Board").GetComponent<GameBoard>();
    }

    public void setAlpha(float opacity)
    {
        Color colour;
        colour = GetComponent<SpriteRenderer>().color;
        colour.a = opacity;
        GetComponent<SpriteRenderer>().color = colour;
    }

    public void SpawnAnimation()
    {
        setAlpha(0);
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        LeanTween.scale(gameObject, new Vector3(0.85f, 0.85f), 0.7f).setEaseOutElastic();

        //gameObject.LeanAlpha(1, 0.75f).setEaseOutBack();
        //gameObject.LeanScale(new Vector3(1f, 1f), 0.5f).setEaseInCubic();
        //gameObject.LeanScale(new Vector3(0.85f, 0.85f), 0.5f).setEaseOutBack();
    }

}
