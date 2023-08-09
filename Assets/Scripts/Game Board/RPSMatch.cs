using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RPSMatch : MonoBehaviour
{
public Sprite MatchSprite;
public Sprite RegularSprite;
public RPSObjects Rock;
public RPSObjects Paper;
public RPSObjects Scissors;
public GameObject State;
public Tweens tweens;

    public void MakeButtonsInteractable(bool state)
    {

        Rock.gameObject.GetComponent<Button>().interactable = state;
        Paper.gameObject.GetComponent<Button>().interactable = state;
        Scissors.gameObject.GetComponent<Button>().interactable = state;
        if(!state)
        {
            Rock.gameObject.GetComponent<Image>().color = Color.grey;
            Paper.gameObject.GetComponent<Image>().color = Color.grey;
            Scissors.gameObject.GetComponent<Image>().color = Color.grey;
        } else
        {
            Rock.gameObject.GetComponent<Image>().color = Color.white;
            Paper.gameObject.GetComponent<Image>().color = Color.white;
            Scissors.gameObject.GetComponent<Image>().color = Color.white;
        }
}

    public void RegularRPS()
    {
        State.GetComponent<Image>().sprite = RegularSprite;
    }


    public void Match()
    {
        State.GetComponent<Image>().sprite = MatchSprite;
        
        tweens.ScaleShake(State);
    }

    public void Appear()
    {
        Rock.Center();
        Paper.Center();
        Scissors.Center();
    }

}
