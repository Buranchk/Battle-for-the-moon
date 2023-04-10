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
