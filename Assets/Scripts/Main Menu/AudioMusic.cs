using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMusic : MonoBehaviour
{
    public AudioSource playerAudio;
    private DataManager DataMan;
    public float soundVolume=0.5f;


    //Game Music
    public AudioClip mainMenuMusic;
    public AudioClip ambientMusic;
    public AudioClip gameMusic;



    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();  
        playerAudio.clip = ambientMusic;
        playerAudio.Play();
        playerAudio.mute = !DataMan.GetSound();
        playerAudio.volume=0.35f;
    }

  // Realization for AudioManager to initilize it when changig game scenes, and prevents it from duplication
    public static AudioMusic Instance;

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

    //Controls for mute button, depends on state saved in DataManager
    public void Mutebutton()
    {
        if(playerAudio.mute==false)
            playerAudio.mute=true;
        else
            playerAudio.mute=false;

    }
}