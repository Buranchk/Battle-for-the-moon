using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerTile : MonoBehaviour
{
    //settings
    public Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer tileRenderer;
    public GameObject highlight;
    private MultiplayerGameBoard Board;

    public TileArrowManager arrowManager;


    //condition
    public GameObject unitSuggestMove;
    public GameObject unitLinked;

    //Data
    private Color basedColor = new Color (0.482f, 0.608f, 0.682f, 1f);
    private Color offsetsColor = new Color (0.557f, 0.714f, 0.769f, 1f);
    private bool isOffsetData;

    public void Init(bool isOffset) 
    {
        isOffsetData = isOffset;
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
        {
            unitSuggestMove = unit;
            arrowManager.gameObject.SetActive(true);
            ArrowDirection(unit.transform.position.x, unit.transform.position.y);

        }
        else if (!isActiveSelection)
        {
            unitSuggestMove = null;
            arrowManager.gameObject.SetActive(false);

        }
    }

    public void ArrowDirection(float x, float y)
    {
        if (gameObject.transform.position.x > x)
        {
            arrowManager.SetDirection("right");
        }
        else if (gameObject.transform.position.x < x)
        {
            arrowManager.SetDirection("left");
        }
        else if (gameObject.transform.position.y > y)
        {
            arrowManager.SetDirection("up");
        }
        else if (gameObject.transform.position.y < y)
        {
            arrowManager.SetDirection("down");
        }
    }

    void OnMouseEnter() 
    {
        //if(GetGameBoard().windowRPS.activeInHierarchy == false && GetGameBoard().gameStage >= 4)
        //    highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        if(unitSuggestMove == null)
            highlight.SetActive(false);
    }

    MultiplayerGameBoard GetGameBoard()
    {
        if (Board == null)
            Board = GameObject.Find("Multiplayer Game Board").GetComponent<MultiplayerGameBoard>();
        return Board;
    }

    public void setAlpha(float opacity)
    {
        Color colour;
        colour = GetComponent<SpriteRenderer>().color;
        colour.a = opacity;
        GetComponent<SpriteRenderer>().color = colour;
    }

    public void MakeTileVisible()
    {
        if(isOffsetData)
            LeanTween.color(gameObject, offsetsColor, 0.75f);
        else
            LeanTween.color(gameObject, basedColor, 0.75f);
    }

}
