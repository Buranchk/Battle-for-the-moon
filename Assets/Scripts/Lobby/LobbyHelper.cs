using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LobbyHelper : MonoBehaviour
{

    //Assets
    public Sprite Marine;
    public Sprite Gold;
    public Sprite XP;
    public Sprite Ruby;
    public Sprite EnemyDark;
    public Sprite EnemyBright;

    //Changebles
    public GameObject soundIcon;
    public GameObject noSoundIcon;
    public GameObject User;
    public GameObject Enemy;
    public TextMeshProUGUI UserName;
    public TextMeshProUGUI EnemyName;
    public TextMeshProUGUI RoomName;


    //NotAlways Active
    public GameObject LoadingHeadEnemy;
    public GameObject ReadyGo;
    public GameObject StatusUserCircle;
    public GameObject LoadingUser;
    public GameObject ReadyUser;
    public GameObject StatusEnemyCircle;
    public GameObject LoadingEnemy;
    public GameObject ReadyEnemy;

    //DataMan
    private DataManager DataMan;

    void Start()
    {
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        SoundSwitch();
        SoundSwitch();
    }

    public void SoundSwitch()
    {
        if(DataMan.GetSound())
        {
            noSoundIcon.transform.localScale = noSoundIcon.transform.localScale * 1.3f;
            DataMan.SetSound(false);

            soundIcon.SetActive(false);
            noSoundIcon.SetActive(true);

            LeanTween.scale(noSoundIcon,new Vector3(0.85f, 0.85f, 0.85f), 0.1f).setEaseOutCirc();
        } 
        else if(!(DataMan.GetSound()))
        {
            
            soundIcon.transform.localScale = noSoundIcon.transform.localScale * 1.3f;
            DataMan.SetSound(true);

            soundIcon.SetActive(true);
            noSoundIcon.SetActive(false);

            LeanTween.scale(soundIcon, new Vector3(0.85f, 0.85f, 0.85f), 0.1f).setEaseOutCirc();
        }
    }

    private void SelectUserSkin()
    {
        int selectedskin = GameObject.Find("Data Manager").GetComponent<DataManager>().GetSelectedSkin();
        switch (selectedskin){
            case 0:
            User.GetComponent<Image>().sprite = Marine;
            break;

            case 1:
            User.GetComponent<Image>().sprite = XP;
            break;

            case 2:
            User.GetComponent<Image>().sprite = Gold;
            break;

            case 3:
            User.GetComponent<Image>().sprite = Ruby;
            break;
        }

    }

    public void RoomInit()
    {
        CloseActives();
        //actives
        FastAppear(LoadingHeadEnemy);
        Spin(LoadingHeadEnemy);
        Enemy.GetComponent<Image>().sprite = EnemyDark;
        SelectUserSkin();
        UserName.text = DataMan.GetName();
        // EnemyName.text =
        // RoomName.text =
    }

    public void RoomConnect()
    {
        ReadyGo.SetActive(true);
        LoadingHeadEnemy.SetActive(false);
        Enemy.GetComponent<Image>().sprite = EnemyBright;
        StatusUserCircle.SetActive(true);
        StatusEnemyCircle.SetActive(true);
        LoadingUser.SetActive(true);
        LoadingEnemy.SetActive(true);
        Spin(LoadingUser);
        Spin(LoadingEnemy);
    }

    public void UserReady()
    {
        LoadingUser.SetActive(false);
        FastAppear(ReadyUser);
    }

    public void EnemyReady()
    {
        LoadingEnemy.SetActive(false);
        FastAppear(ReadyEnemy);
    }

    public void CloseActives()
    {
        LoadingHeadEnemy.SetActive(false);
        ReadyGo.SetActive(false);
        StatusUserCircle.SetActive(false);
        LoadingUser.SetActive(false);
        ReadyUser.SetActive(false);
        StatusEnemyCircle.SetActive(false);
        LoadingEnemy.SetActive(false);
        ReadyEnemy.SetActive(false);
    }

    public void Spin(GameObject Loading)
    {
        LeanTween.rotateAround(Loading, Vector3.back, 360f, 0.85f).setLoopClamp().setEase(LeanTweenType.linear);
    }

    public void FastAppear(GameObject ReadySign)
    {
        ReadySign.transform.localScale = ReadySign.transform.localScale * 1.3f;
        LeanTween.scale(ReadySign, new Vector3(1f, 1f, 1f), 0.2f).setEaseOutCirc();
        ReadySign.SetActive(true);
    }

}
