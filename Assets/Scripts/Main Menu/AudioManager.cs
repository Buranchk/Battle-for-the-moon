using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource playerAudio;
    private DataManager DataMan;
    public float soundVolume=1.0f;
    

    //Game Music
    public AudioClip mainMenuMusic;
    public AudioClip ambientMusic;
    public AudioClip gameMusic;

    //Sounds for click on buttons UI with a short sharp response
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

    //BattleScene SFX
    public AudioClip flagDecoyAppereance;
    public AudioClip shuffleUnit;
    public AudioClip enemySpawn;
    public AudioClip unitFight;
    public AudioClip unitDeath;
    public AudioClip unitMatch;



    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();  
//        playerAudio.clip = ambientMusic;
        playerAudio.Play();
        playerAudio.mute = !DataMan.GetSound();
        //playerAudio.volume=0.01f;
    }

  // Realization for AudioManager to initilize it when changig game scenes, and prevents it from duplication
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

    //Functions for changing audiotrack and playing music depending on scene
    // public void MainMenuMusic()
    // {
    //     if(playerAudio.clip!=mainMenuMusic)
    //     { 
    //         playerAudio.clip = mainMenuMusic;
    //         playerAudio.Play();
    //     }
    // }
    // public void GameMusic()
    // {
    //     playerAudio.clip = gameMusic;
    //     playerAudio.Play();
    // }

    // public void AmbientMusic()
    // {
    //     playerAudio.clip = ambientMusic;
    //     playerAudio.Play();
    // }

    //Controls for mute button, depends on state saved in DataManager
    public void Mutebutton()
    {
        if(playerAudio.mute==false)
            playerAudio.mute=true;
        else
            playerAudio.mute=false;

    }

    //Functions for SFX 
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
        playerAudio.PlayOneShot(cashRegisterRing, 1.5f*soundVolume);
    }   

     public void XPSoundFX()
    {
        playerAudio.PlayOneShot(expFillSFX, soundVolume);
    }  

     public void LVLupSFX()
    {
        playerAudio.PlayOneShot(lvlUpSFX, soundVolume);
    }  

     public void SelectionSoundFX()
    {
        playerAudio.PlayOneShot(selectionSFX, 1.2f*soundVolume);
    }  

     public void AirWhistleSoundFX()
    {
        playerAudio.PlayOneShot(airWhistlSFX, 0.7f*soundVolume);
    }  

     public void MoneyDingSoundFX()
    {
        playerAudio.PlayOneShot(moneyDingSFX, soundVolume);
    }  

    //Functions for Battle Scene SFX
     public void FlagDecoyAppereance()
    {
        playerAudio.PlayOneShot(flagDecoyAppereance, soundVolume);
    }   

     public void ShuffleUnit()
    {
        playerAudio.PlayOneShot(shuffleUnit , soundVolume);
    }  

     public void EnemySpawn()
    {
        playerAudio.PlayOneShot(enemySpawn, soundVolume);
    }  

     public void UnitFight()
    {
        playerAudio.PlayOneShot(unitFight, soundVolume);
    }  

     public void UnitDeath()
    {
        playerAudio.PlayOneShot(unitDeath, soundVolume);
    }  

     public void UnitMatch()
    {
        playerAudio.PlayOneShot(unitMatch, soundVolume);
    }
}
