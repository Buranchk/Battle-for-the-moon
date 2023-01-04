using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour 
{
    //settings
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer tileRenderer;
    public GameObject highlight;
    private GameBoard Board;


    //condition
    public GameObject unitSuggestMove;
    public GameObject unitLinked;

    public void Init(bool isOffset) 
    {
        tileRenderer.color = isOffset ? offsetColor : baseColor;
        highlight.SetActive(false);
    }
    
    void OnMouseOver()
    {
        if (unitSuggestMove != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //TileMoveSMTH
                UnitMove();
            }
        }
    }


    private void UnitMove()
    {
        int x = ((int)gameObject.transform.position.x);
        int y = ((int)gameObject.transform.position.y);
        int xe = ((int)unitSuggestMove.transform.position.x);
        int ye = ((int)unitSuggestMove.transform.position.y);
        GetGameBoard().TileStep(x, y, xe, ye);
    }

    public void TileHighlight(GameObject unit, bool isActiveSelection)
    {
        highlight.SetActive(isActiveSelection);
        if (isActiveSelection)
            unitSuggestMove = unit;
        else if (!isActiveSelection)
            unitSuggestMove = null;
    }

    void OnMouseEnter() 
    {
        if(GetGameBoard().windowRPS.activeInHierarchy == false && GetGameBoard().gameStage >= 2)
            highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        if(unitSuggestMove == null)
            highlight.SetActive(false);
    }

    GameBoard GetGameBoard()
    {
        if (Board == null)
            Board = GameObject.Find("Game Board").GetComponent<GameBoard>();
        return Board;
    }
}