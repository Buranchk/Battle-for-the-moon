using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour
{
    public int queue;
    public bool available;
    public Sprite unlocked;
    public Sprite locked;
    private void Start()
    {
        if(available)
            gameObject.GetComponent<SpriteRenderer>().sprite = unlocked;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = locked;
    }

    public void RightSwipe()
    {
        switch(queue)
        {
        
        case 0:
        //mid to RIGHT
        SkinMove(3.362527f, -0.4885723f, 1.0f, 1.4f);
        // gameObject.transform.position = new Vector3(3.362527f, -0.4885723f, 1);
        // gameObject.transform.localScale = new Vector3(0.8621865f, 0.8621865f, 0.8621865f);
        break;
        
        case 1:
        //right to BACK
        SkinMove(0.4885723f, -0.4885723f, 3.0f, 1f);
        // gameObject.transform.position = new Vector3(0.4885723f, -0.4885723f, 3);
        // gameObject.transform.localScale = new Vector3(0.574791f, 0.574791f, 0.574791f);
        break;
        
        case 2:
        //back to LEFT
        SkinMove(-2.385383f, -0.4885723f, 1.0f, 1.4f);
        // gameObject.transform.position = new Vector3(-2.385383f, -0.4885723f, 1);
        // gameObject.transform.localScale = new Vector3(0.8621865f, 0.8621865f, 0.8621865f);
        break;

        case 3:
        //left to MID
        SkinMove(0.4885723f, -0.4885723f, 0.0f, 1.8f);
        // gameObject.transform.position = new Vector3(0.4885723f, -0.4885723f, 0);
        // gameObject.transform.localScale = new Vector3(1.149582f, 1.149582f, 1.149582f);
        break;
        }

    }

    public void LeftSwipe()
    {
        switch(queue)
        {
        
        case 0:
        //mid to left
        SkinMove(-2.385383f, -0.4885723f, 1.0f, 1.4f);
        // gameObject.transform.position = new Vector3(-2.385383f, -0.4885723f, 1);
        // gameObject.transform.localScale = new Vector3(0.8621865f, 0.8621865f, 0.8621865f);
        break;
        
        case 1:
        //right to mid
        SkinMove(0.4885723f, -0.4885723f, 0.0f, 1.8f);
        // gameObject.transform.position = new Vector3(0.4885723f, -0.4885723f, 0);
        // gameObject.transform.localScale = new Vector3(1.149582f, 1.149582f, 1.149582f);
        break;
        
        case 2:
        //back to right
        SkinMove(3.362527f, -0.4885723f, 1.0f, 1.4f);
        // gameObject.transform.position = new Vector3(3.362527f, -0.4885723f, 1);
        // gameObject.transform.localScale = new Vector3(0.8621865f, 0.8621865f, 0.8621865f);
        break;

        case 3:
        //left to back
        SkinMove(0.4885723f, -0.4885723f, 3.0f, 1f);
        // gameObject.transform.position = new Vector3(0.4885723f, -0.4885723f, 3);
        // gameObject.transform.localScale = new Vector3(0.574791f, 0.574791f, 0.574791f);
        break;
        }
    }

    public void SkinMove(float xPos, float yPos, float zPos, float allScale)
    {
        GameObject MySelf = gameObject;
        LeanTween.move(MySelf, new Vector3(xPos, yPos, zPos), 0.4f).setEaseInOutQuint();
        LeanTween.scale(MySelf, new Vector3(allScale, allScale, allScale), 0.4f).setEaseInOutQuint();
    }

}
