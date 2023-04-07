using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSObjects : MonoBehaviour
{
    public Tweens Tween;
    public Vector3 place;

    void Awake()
    {
        place = gameObject.transform.position;
        Tween = GameObject.Find("Tweens").GetComponent<Tweens>();
    }
    public void Shake()
    {
        gameObject.transform.position = place;
        Tween.MoveShake(gameObject, place);
    }
}
