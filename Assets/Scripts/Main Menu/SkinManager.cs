using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : DataManager
{
    private int skinAmount = 3;
    private List<bool> skins;
    public int currentSkin = 0;
    private int leftSkin = 4;
    private int rightSkin = 2;

    public GameObject skin1;
    public GameObject skin2;
    public GameObject skin3;
    public GameObject skin4;

    //set open and closed skins
    //rotation of the group
    //last selected skin
    /*
    ::Skin manager::
    preload skin selection depending on a last selected skin
    -massive that can roll !@
    -always find a pos3
    -manage pos3 locked skins depending on the massive of bools

    ::Skin platform::
    change width and height depending of the png size!
    change scale!
    make movement!
    */


    public void RightSwipe()
    {
        //pos 1 , 2 , 3 , 4
        currentSkin = GetPrev(currentSkin);
        leftSkin = GetPrev(leftSkin);
        rightSkin = GetPrev(rightSkin);

        // if(skins[currentSkin])
        // {
        //     save.selectedSkin = currentSkin;
        //     //Json UPD
        // }
        //GameObject.Find("BackSkin").GetComponent<Skin>();
        
        skin1.GetComponent<Skin>().RightSwipe();
        skin1.GetComponent<Skin>().queue = GetNext(skin1.GetComponent<Skin>().queue);
        
        skin2.GetComponent<Skin>().RightSwipe();
        skin2.GetComponent<Skin>().queue = GetNext(skin2.GetComponent<Skin>().queue);

        skin3.GetComponent<Skin>().RightSwipe();
        skin3.GetComponent<Skin>().queue = GetNext(skin3.GetComponent<Skin>().queue);

        skin4.GetComponent<Skin>().RightSwipe();
        skin4.GetComponent<Skin>().queue = GetNext(skin4.GetComponent<Skin>().queue);
        SaveSkinChange();
    }

    public void LeftSwipe()
    {
        currentSkin = GetNext(currentSkin);
        leftSkin = GetNext(leftSkin);
        rightSkin = GetNext(rightSkin);

        skin1.GetComponent<Skin>().LeftSwipe();
        skin1.GetComponent<Skin>().queue = GetPrev(skin1.GetComponent<Skin>().queue);
        
        skin2.GetComponent<Skin>().LeftSwipe();
        skin2.GetComponent<Skin>().queue = GetPrev(skin2.GetComponent<Skin>().queue);

        skin3.GetComponent<Skin>().LeftSwipe();
        skin3.GetComponent<Skin>().queue = GetPrev(skin3.GetComponent<Skin>().queue);

        skin4.GetComponent<Skin>().LeftSwipe();
        skin4.GetComponent<Skin>().queue = GetPrev(skin4.GetComponent<Skin>().queue);
        SaveSkinChange();
    }

    private int GetNext(int num)
    {
        if(num == skinAmount)
            num = 0;
        else if(num < skinAmount)
            num++;
        return num;
    }

    private int GetPrev(int num)
    {
        if(num == 0)
            num = skinAmount;
        else if(num > 0)
            num--;
        return num;
    }


    private void Start()
    {
        //skinAmount = save.skins.Count;
        //skins = save.skins;
        //LoadSkin();
    }

    private void LoadSkin()
    {
        int smth = save.selectedSkin;
        currentSkin = smth;
        leftSkin = GetPrev(currentSkin);
        rightSkin = GetNext(currentSkin);
    }

    private void SaveSkinChange()
    {
        Save sv = GetSave();
        sv.selectedSkin = currentSkin;
        SetSave(sv);
    }
}
