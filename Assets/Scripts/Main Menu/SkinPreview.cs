using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkinPreview : MonoBehaviour
{
    public Sprite spaceMarine;
    public Sprite XP;
    public Sprite gold;
    public Sprite ruby;

    public void Appear (int typeSkin)
    {
        switch (typeSkin)
       { 
        case 0:
        gameObject.GetComponent<Image>().sprite = spaceMarine;

        break;
        
        case 1:
        gameObject.GetComponent<Image>().sprite = XP;
        
        break;
        
        case 2:
        gameObject.GetComponent<Image>().sprite = gold;
        
        break;
        
        case 3:
        gameObject.GetComponent<Image>().sprite = ruby;

        break;
       }

        LeanTween.color(gameObject, new Vector4(1f, 1f, 1f, 1f), 0.3f);
        LeanTween.scale(gameObject, new Vector3(1.3f, 1.3f, 1f), 0.3f);
    }
    public void DisAppear()
    {
        //gameObject.GetComponent<Image>().sprite;
        LeanTween.color(gameObject, new Vector4(0f, 0f, 0f, 0f), 0.01f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

    }

}
