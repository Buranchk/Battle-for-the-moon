using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource playerAudio;
    public float soundVolume=1.0f;
    

    //Game Music
    public AudioClip mainMenuMusic;
    public AudioClip ambientMusic;
    public AudioClip gameMusic;

    //Click on buttons UI with a short sharp response
    public AudioClip clicOnButtons1; //Click to start(Start screen); Shop Click(Main menu); Back(Space store); Back(Profile); Done(Geme board); Back (Game rresult)
    public AudioClip clicOnButtons2; //Profile Click(Main menu); Exit(Game board)
    public AudioClip playButton; //Start game sound, must represent energy waste

    //Action Sound FX
    public AudioClip cashRegisterRing;
    public AudioClip expFillSFX;
    public AudioClip lvlUpSFX;
    public AudioClip selectionSFX;
    public AudioClip airWhistlSFX;
    public AudioClip moneyDingSFX;
    public AudioClip winSFX;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAudio.clip = ambientMusic;
        playerAudio.Play();
    }
  
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void MainMenuMusic()
    {
        if(playerAudio.clip!=mainMenuMusic)
        { 
            playerAudio.clip = mainMenuMusic;
            playerAudio.Play();
        }
    }
    public void GameMusic()
    {
        playerAudio.clip = gameMusic;
        playerAudio.Play();
    }

    public void AmbientMusic()
    {
        playerAudio.clip = ambientMusic;
        playerAudio.Play();
    }
    public void Mutebutton()
    {
        if(playerAudio.mute==false)
            playerAudio.mute=true;
        else
            playerAudio.mute=false;

    }
    public void OnClicButton1()
    {
        playerAudio.PlayOneShot(clicOnButtons1, soundVolume);
    }

    public void OnClicButton2()
    {
        playerAudio.PlayOneShot(clicOnButtons2, soundVolume);
    }

    public void PlayButton()
    {
        playerAudio.PlayOneShot(playButton, soundVolume);
    }

     public void BuyButton()
    {
        playerAudio.PlayOneShot(cashRegisterRing, soundVolume);
    }   

     public void XPSoundFX()
    {
        playerAudio.PlayOneShot(cashRegisterRing, soundVolume);
    }  

     public void LVLupSFX()
    {
        playerAudio.PlayOneShot(cashRegisterRing, soundVolume);
    }  

     public void SelectionSoundFX()
    {
        playerAudio.PlayOneShot(cashRegisterRing, soundVolume);
    }  

     public void AirWhistleSoundFX()
    {
        playerAudio.PlayOneShot(cashRegisterRing, soundVolume);
    }  

     public void MoneyDingSoundFX()
    {
        playerAudio.PlayOneShot(cashRegisterRing, soundVolume);
    }  


}
