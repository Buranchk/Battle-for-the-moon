using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweens : MonoBehaviour
{

    GameObject RPSfigure;
    Vector3 positionPlace;
    GameObject RPSstate;
    public GameObject explosionPrefab;
    

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

    public void AppearScale(GameObject item)
    {
        LeanTween.scale(item, new Vector2(1.1f, 1.1f), 0.25f).setEaseOutCirc().setLoopPingPong(1);
    }

    public void ScaleDownDisappear(GameObject item)
    {
        LeanTween.scale(item, new Vector2(0f, 0f), 0.1f).setEaseInCirc().setOnComplete(() =>
        {
            item.SetActive(false);
        });
    }

    public void AbortGame(GameObject item)
    {
        item.SetActive(true);
        LeanTween.scale(item, new Vector2(1.3f, 1.3f), 0.5f).setEaseOutCirc();
        LeanTween.scale(item, new Vector2(0.1f, 0.1f), 0.3f).setEaseInCirc().setDelay(1f);
    }

    public void UnitsMeet(GameObject unit1, GameObject unit2, bool win)
    {
        Vector2 initialPos1 = unit1.transform.position;
        Vector2 initialPos2 = unit2.transform.position;

        Vector2 initScale1 = unit1.transform.localScale;
        Vector2 initScale2 = unit2.transform.localScale;

        Vector2 centre = new Vector2 ((unit1.transform.position.x + unit2.transform.position.x)/2, (unit1.transform.position.y + unit2.transform.position.y)/2);
        
        LeanTween.scale(unit1, (initScale1 * 1.5f), 0.1f).setEaseOutQuad().setLoopPingPong(1);
        
        CreateAndShakeExplosion(centre);

        LeanTween.move(unit1, centre, 0.1f).setEaseInCirc().setLoopPingPong(1);
        if(!win)
        {
            Vector2 supCent = (centre + initialPos2)/2;
            LeanTween.move(unit2, supCent, 0.1f).setLoopPingPong(1);
        }
        else if(win)
        {
            if((centre - initialPos2).y == 0)
            {
                LeanTween.move(unit2, new Vector2(initialPos2.x, initialPos2.y + 0.3f), 0.1f);
                LeanTween.move(unit2, new Vector2(initialPos2.x, initialPos2.y - 0.3f), 0.1f).setDelay(0.1f);
                LeanTween.move(unit2, new Vector2(initialPos2.x, initialPos2.y), 0.1f).setDelay(0.2f).setOnComplete(() =>
                {
                    unit2.transform.position = initialPos2;
                }); 
            }
            else if(((centre - initialPos2).x == 0))
            {
                LeanTween.move(unit2, new Vector2(initialPos2.x + 0.3f, initialPos2.y), 0.1f);
                LeanTween.move(unit2, new Vector2(initialPos2.x - 0.3f, initialPos2.y), 0.1f).setDelay(0.1f);
                LeanTween.move(unit2, new Vector2(initialPos2.x, initialPos2.y), 0.1f).setDelay(0.2f).setOnComplete(() =>
                {
                    unit2.transform.position = initialPos2;
                });
            } 
        }


        
    }
    public void CreateAndShakeExplosion(Vector2 center)
    {
        // Instantiate the prefab
        GameObject explosionInstance = Instantiate(explosionPrefab, center, Quaternion.identity);
        explosionInstance.transform.localScale = Vector3.one * 0.3f;

        LeanTween.scale(explosionInstance, new Vector2(1f,1f), 0.2f).setEaseInElastic().setLoopPingPong(1);
        // Shake the object
        LeanTween.move(explosionInstance, explosionInstance.transform.position + new Vector3(0.1f, 0, 0), 0.2f)
            .setEaseShake()
            .setOnComplete(() =>
            {
                // Destroy the object after shaking
                Destroy(explosionInstance);
            });
    }

}

