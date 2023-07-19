using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FingerScript : MonoBehaviour
{

    private LTDescr scaleTween;
    private LTDescr alphaTween;
    public float animationDuration = 1.8f;


    public bool playingAnim = false;

    public void StartPointing(List<GameObject> units)
    {
        gameObject.SetActive(true);
        playingAnim = true;
        var unitList = new List<GameObject>(units);
        if(scaleTween == null)
            unitList.Reverse();
        StartCoroutine(PointToUnits(unitList));
    }

       IEnumerator PointToUnits(List<GameObject> units)
    {
        units.Reverse();
        foreach (var unit in units)
        {
            if(!playingAnim)
            {
                yield break;
            }

            // Set the position of the gameObject's transform to the current unit
            transform.position = unit.transform.position;

            // Create LeanTween animations for scaling and opacity
            scaleTween = LeanTween.scale(gameObject, Vector3.one * 1.08f, animationDuration * 0.2f).setLoopPingPong();
            Image img = gameObject.GetComponent<Image>();
            alphaTween = LeanTween.value(gameObject, a => {
                    Color c = img.color;
                    c.a = a;
                    img.color = c;
                }, img.color.a, 1f, animationDuration)
            .setLoopPingPong(1).setEaseInOutCubic();

            // Wait for the animations to finish before moving on to the next unit
            yield return new WaitForSeconds(animationDuration * 2);

        }
    }

    public void StopPointing()
    {
        // Finish the scale and opacity animations
        LeanTween.cancel(scaleTween.id);
        LeanTween.cancel(alphaTween.id);

        // Reset the scale and opacity to the end values
        gameObject.transform.localScale = Vector3.one;

        // Change the alpha to be transparent.
        Image image = gameObject.GetComponent<Image>();
            Color color = image.color;
            color.a = 0f;  
            image.color = color;


        playingAnim = false;
        gameObject.SetActive(false);

    }
    
}
