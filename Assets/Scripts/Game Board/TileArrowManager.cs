using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileArrowManager : MonoBehaviour
{
    public Sprite arrowUP;
    public Sprite arrowDown;
    public Sprite arrowLeft;
    public Sprite arrowRight;

    public void SetDirection(string direction)
    {
        transform.localScale = new Vector3(0.36f, 0.36f, 0.36f);
        switch(direction)
        {
            case "up":
                gameObject.GetComponent<SpriteRenderer>().sprite = arrowUP;
            break;
            case "down":
                gameObject.GetComponent<SpriteRenderer>().sprite = arrowDown;
            break;
            case "left":
                gameObject.GetComponent<SpriteRenderer>().sprite = arrowLeft;
            break;
            case "right":
                gameObject.GetComponent<SpriteRenderer>().sprite = arrowRight;
            break;
        }
        LeanTween.scale(gameObject, new Vector3(0.4f, 0.4f, 0.4f), 0.4f).setEaseInQuad();
        LeanTween.scale(gameObject, new Vector3(0.36f, 0.36f, 0.36f), 0.4f).setEaseOutQuad().setDelay(0.4f);
    }

}
