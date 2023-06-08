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
    public GameObject outliner;
    public GameObject shopCanvas;
    public SkinManager skinManager;
    public int typeSkin;

    private void Start()
    {
        if(available)
            gameObject.GetComponent<SpriteRenderer>().sprite = unlocked;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = locked;
    }

    [Obsolete]
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!shopCanvas.active){
                if (queue == 0)
                {
                    if (available)
                    {
                        Select();
                        AudioManager.Instance.SelectionSoundFX();
                    }
                    else if (!available)
                    {
                        GameObject.Find("Menu Manager").GetComponent<MenuManager>().ShowCaseSkin(typeSkin);
                        print("BuyOption");
                    }
                }
                else if (queue == 1)
                {
                    skinManager.LeftSwipe();
                }
                else if (queue == 3)
                {
                    skinManager.RightSwipe();
                }
            }
        }
    }

    public void SwitchLock()
    {   
        Sprite objectSpriteRenderer = gameObject.GetComponent<SpriteRenderer>().sprite;
        if(objectSpriteRenderer.name == unlocked.name)
        {
            objectSpriteRenderer = locked;
        }
        else if (objectSpriteRenderer.name == locked.name)
        {
            objectSpriteRenderer = unlocked;
        }
    }

    public void Select()
    {
        skinManager.SetCurrentSkin(gameObject.GetComponent<Skin>());
        print("new skin is selected");
        outliner.SetActive(true);
    }

    public void Unselect()
    {
        outliner.SetActive(false);
        //setactive backguy? (false)
    }

    public void RightSwipe()
    {
        switch(queue)
        {
        
        case 0:
        //mid to RIGHT
        SkinMove(3.362528f, -0.4885723f, 1.0f, 1.4f);
        // gameObject.transform.position = new Vector3(3.362527f, -0.4885723f, 1);
        // gameObject.transform.localScale = new Vector3(0.8621865f, 0.8621865f, 0.8621865f);
        break;
        
        case 1:
        //right to BACK
        SkinMove(0.0f, -0.4885723f, 3.0f, 1f);
        // gameObject.transform.position = new Vector3(0.4885723f, -0.4885723f, 3);
        // gameObject.transform.localScale = new Vector3(0.574791f, 0.574791f, 0.574791f);
        break;
        
        case 2:
        //back to LEFT
        SkinMove(-3.362528f, -0.4885723f, 1.0f, 1.4f);
        // gameObject.transform.position = new Vector3(-2.385383f, -0.4885723f, 1);
        // gameObject.transform.localScale = new Vector3(0.8621865f, 0.8621865f, 0.8621865f);
        break;

        case 3:
        //left to MID
        SkinMove(0.0f, -0.4885723f, 0.0f, 1.8f);
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
        SkinMove(-3.362528f, -0.4885723f, 1f, 1.4f);
        // gameObject.transform.position = new Vector3(-2.385383f, -0.4885723f, 1);
        // gameObject.transform.localScale = new Vector3(0.8621865f, 0.8621865f, 0.8621865f);
        break;
        
        case 1:
        //right to mid
        SkinMove(0.0f, -0.4885723f, 0f, 1.8f);
        // gameObject.transform.position = new Vector3(0.4885723f, -0.4885723f, 0);
        // gameObject.transform.localScale = new Vector3(1.149582f, 1.149582f, 1.149582f);
        break;
        
        case 2:
        //back to right
        SkinMove(3.362528f, -0.4885723f, 1f, 1.4f);
        // gameObject.transform.position = new Vector3(3.362527f, -0.4885723f, 1);
        // gameObject.transform.localScale = new Vector3(0.8621865f, 0.8621865f, 0.8621865f);
        break;

        case 3:
        //left to back
        SkinMove(0.0f, -0.4885723f, 3f, 1f);
        // gameObject.transform.position = new Vector3(0.4885723f, -0.4885723f, 3);
        // gameObject.transform.localScale = new Vector3(0.574791f, 0.574791f, 0.574791f);
        break;
        }
    }

    public void SkinMove(float xPos, float yPos, float zPos, float allScale)
    {
        GameObject MySelf = gameObject;
        LeanTween.move(MySelf, new Vector3(xPos, yPos, zPos), 0.4f).setEaseInOutQuint();
        LeanTween.scale(MySelf, new Vector2(allScale, allScale), 0.4f).setEaseInOutQuint();
        AudioManager.Instance.AirWhistleSoundFX();
    }

}
