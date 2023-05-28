using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnShow : MonoBehaviour
{
    public GameBoard GB;


    void Update()
    {
        if(GB.turn)
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        else
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
