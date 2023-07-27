using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionShowcase : MonoBehaviour
{
    public Sprite rock;
    public Sprite paper;
    public Sprite scissors;

    public GameObject myPick;
    public GameObject enemyPick;

    public bool winState;

    //Settings
    public float targetRotationZ = 40f;
    public float animationDuration = 0.3f;

    public Vector2 initialEnemyPosition;
    public Vector2 initialMyPosition;


    public void SetRPSFightAnimation(string enemyStuff, string myStuff, bool win, Vector3 pos)
    {
        gameObject.transform.position = pos;

        enemyPick.GetComponent<RectTransform>().localPosition = new Vector3(300f, 0f, 0f);
        myPick.GetComponent<RectTransform>().localPosition = new Vector3(-300f, 0f, 0f);

        winState = win;
        gameObject.SetActive(true);

        initialMyPosition = myPick.transform.position;
        initialEnemyPosition = enemyPick.transform.position;

        ChangeSprite(myPick, myStuff);
        ChangeSprite(enemyPick, enemyStuff);

        StartCoroutine(RPSAnimation());

    }

    public void SetRPSTieAnimation(string element, Vector3 pos)
    {
        gameObject.transform.position = pos;
        
        enemyPick.GetComponent<RectTransform>().localPosition = new Vector3(300f, 0f, 0f);
        myPick.GetComponent<RectTransform>().localPosition = new Vector3(-300f, 0f, 0f);

        gameObject.SetActive(true);
        
        winState = true;
        ChangeSprite(myPick, element);
        ChangeSprite(enemyPick, element);

        StartCoroutine(RPSAnimation());        
    }

    public IEnumerator RPSAnimation()
    {
        RollInside(true);
        RollInside(false);
        yield return new WaitForSeconds(animationDuration);
        RollOut(true);
        RollOut(false);
        yield return new WaitForSeconds(animationDuration* 2);
        gameObject.SetActive(false);
    }



    public void RollInside(bool me)
    {
        GameObject pick;
        int mod;
        if(me)
        {
            
            pick = myPick;
            mod = -1;
        }
        else
        {
            pick = enemyPick;
            mod = 1;
        }
        pick.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        // Animate the GameObject to the target alpha, position and rotation
        LeanTween.alpha(pick.GetComponent<Image>().rectTransform, 1f, animationDuration * 0.75f)
            .setEase(LeanTweenType.easeInQuad);

        LeanTween.move(pick.GetComponent<RectTransform>(), new Vector3(75f, 0f, 0f) * mod, animationDuration * 0.75f)
            .setEase(LeanTweenType.easeInQuad);
        
        LeanTween.rotateZ(pick, targetRotationZ * mod, animationDuration * 0.75f)
            .setEase(LeanTweenType.easeInQuad);


    }

    public void RollOut(bool me)
    {
        GameObject pick;
        //int mod;
        if(me)
        {
            
            pick = myPick;
        //    mod = -1;
        }
        else
        {
            pick = enemyPick;
        //    mod = 1;
        }
        pick.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        // Animation is finished, start the second animation to move back and fade out
        if(me)
            LeanTween.move(pick.GetComponent<RectTransform>(), new Vector3(-130f, 60f, 0f), animationDuration * 1.25f)
            .setEase(LeanTweenType.easeOutQuad);
        else
            LeanTween.move(pick.GetComponent<RectTransform>(), new Vector3(130f, 60f, 0f), animationDuration * 1.25f)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.rotateZ(pick, 0, animationDuration * 1.25f)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.alpha(pick.GetComponent<Image>().rectTransform, 0f, animationDuration * 1.25f)
            .setEase(LeanTweenType.easeOutQuad);

            


    }

    public void ChangeSprite(GameObject receiver, string exporter)
    {  
        receiver.GetComponent<Image>().SetNativeSize();


        switch(exporter)
        {
            case "rock":
            receiver.GetComponent<Image>().sprite = rock;
            break;

            case "paper":
            receiver.GetComponent<Image>().sprite = paper;
            break;

            case "scissors":
            receiver.GetComponent<Image>().sprite = scissors;
            break;
        }

    }

}
