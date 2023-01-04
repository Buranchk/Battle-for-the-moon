using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //settings
    [SerializeField] private GameObject tile;
    public GameObject highlight;
    private GameBoard Board;

    //sprites
    public Sprite rock;
    public Sprite paper;
    public Sprite scissors;
    public Sprite rockOpen;
    public Sprite paperOpen;
    public Sprite scissorsOpen;
    public Sprite flag;
    public Sprite decoy;
    public Sprite nothing;

    
    //permanent statement
    public bool playerUnit = true;

    //condition
    public bool isOpen = false;
    public string type = "Nothing";
    public bool isOverTheUnit = false;
    public bool movedOn;

    //Unit initiation
    public void Init()
    {
        movedOn = false;
        highlight.SetActive(false);
    }

    public void ChangeType(string newType)
    {
        type = newType;
        
        switch (newType)
        {
            case "nothing":
            this.gameObject.GetComponent<SpriteRenderer>().sprite = nothing;
            highlight.GetComponent<SpriteRenderer>().sprite = nothing;
            break;

            case "rock":
            if(isOpen)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = rockOpen;
                highlight.GetComponent<SpriteRenderer>().sprite = rockOpen;
            } else
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = rock;
                highlight.GetComponent<SpriteRenderer>().sprite = rock;
            }
            break;

            case "paper":
            if(isOpen)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = paperOpen;
                highlight.GetComponent<SpriteRenderer>().sprite = paperOpen;
            } else
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = paper;
                highlight.GetComponent<SpriteRenderer>().sprite = paper;
            }

            break;

            case "scissors":
            if(isOpen)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = scissorsOpen;
                highlight.GetComponent<SpriteRenderer>().sprite = scissorsOpen;
            } else
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = scissors;
                highlight.GetComponent<SpriteRenderer>().sprite = scissors;
            }
            break;

            case "flag":
            this.gameObject.GetComponent<SpriteRenderer>().sprite = flag;
            highlight.GetComponent<SpriteRenderer>().sprite = flag;
            break;

            case "decoy":
            this.gameObject.GetComponent<SpriteRenderer>().sprite = decoy;
            highlight.GetComponent<SpriteRenderer>().sprite = decoy;
            break;
        }
    }

    //Unit selection
    void OnMouseOver()
    {
        if(GetGameBoard().windowRPS.activeInHierarchy == false)
        {
            isOverTheUnit = true;
            if (Input.GetMouseButtonDown(0) && gameObject.name != "DecoyUnit" && gameObject.name != "FlagUnit")
            {
                if (GetGameBoard().gameStage == 4 && GetGameBoard().turn)
                    GetGameBoard().SelectUnit(gameObject);
                else if (GetGameBoard().gameStage <= 2)
                {
                    GetGameBoard().SetFlagDecoy(gameObject);
                }
            }
        }
    }

    //Unit highlight exit
    void OnMouseExit()
    {
        isOverTheUnit = false;
    }

    GameBoard GetGameBoard()
    {
        if (Board == null)
            Board = GameObject.Find("Game Board").GetComponent<GameBoard>();
        return Board;
    }

}
