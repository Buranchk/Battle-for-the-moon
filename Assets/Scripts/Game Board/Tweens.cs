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
}
