using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweens : MonoBehaviour
{
    GameObject RPSfigure;
    Vector3 positionPlace;
    GameObject RPSstate;

    public void MoveShake(GameObject item, Vector3 place)
    {
        RPSfigure = item;
        positionPlace = place;

        float inX = place.x;
        float inY = place.y;

        float ranX = Random.Range(-0.8f, 0.8f);
        float ranY = Random.Range(-0.8f, 0.8f);

        LeanTween.move(item, new Vector2(inX + ranX, inY + ranY), 0.1f).setEaseInCubic().setDelay(0.1f);
        LeanTween.move(item, new Vector2(inX + ranX, inY + ranY), 0.1f).setEaseOutQuad().setLoopPingPong(1).setOnComplete(PlaceBack);

    }

    private void PlaceBack()
    {
        RPSfigure.transform.position = positionPlace;
    }

    private void SetBack()
    {
        RPSstate.transform.localScale = Vector3.one;
    }

    public void ScaleShake(GameObject item)
    {
        RPSstate = item;
        LeanTween.scale(item, new Vector2(1.2f, 1.2f), 0.2f).setEaseOutBounce().setLoopPingPong(1).setOnComplete(SetBack);
    }

    public void MoveTo(GameObject item, bool me)
    {
        RectTransform rectItemTransform = item.GetComponent<RectTransform>();
        if (me)
        {
            rectItemTransform.transform.position = new Vector3(2700f, -250f, 0f);
            LeanTween.move(rectItemTransform, new Vector2(1500f, -500f), 0.8f).setEaseOutExpo();
            LeanTween.rotate(item, new Vector3(0f, 0f, -8f), 0.35f).setEaseOutExpo();
            LeanTween.rotate(item, new Vector3(0f, 0f, 3f), 0.35f).setEaseInQuad().setDelay(0.35f);
            LeanTween.rotate(item, new Vector3(0f, 0f, -1f), 0.35f).setEaseInQuad().setDelay(0.70f);

            LeanTween.scale(item, new Vector3(1f, 1f, 1f), 1.5f).setEaseInOutQuad().setLoopPingPong(1);

            LeanTween.rotate(item, new Vector3(0f, 0f, 0f), 0.50f).setEaseInQuad().setDelay(3.45f);
            LeanTween.move(rectItemTransform, new Vector2(-400f, -250f), 1f).setEaseInOutSine().setDelay(4f);
        } else if (!me)
        {
            rectItemTransform.transform.position = new Vector3(-2700f, -250f, 0f);
            LeanTween.move(rectItemTransform, new Vector2(-1500f, -750f), 0.8f).setEaseOutExpo();
            LeanTween.rotate(item, new Vector3(0f, 0f, -8f), 0.35f).setEaseOutExpo();
            LeanTween.rotate(item, new Vector3(0f, 0f, 3f), 0.35f).setEaseInQuad().setDelay(0.35f);
            LeanTween.rotate(item, new Vector3(0f, 0f, -1f), 0.35f).setEaseInQuad().setDelay(0.70f);
            
            LeanTween.scale(item, new Vector3(1f, 1f, 1f), 1.5f).setEaseInOutQuad().setLoopPingPong(1);

            LeanTween.rotate(item, new Vector3(0f, 0f, 0f), 0.50f).setEaseInQuad().setDelay(3.45f);
            LeanTween.move(rectItemTransform, new Vector2(-400f, -250f), 1f).setEaseInOutSine().setDelay(4f);
        }
    }

}
