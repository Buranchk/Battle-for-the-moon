using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneLoader : MonoBehaviour
{
    public GameObject AdProposition;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        AudioMusic.Instance.MainMenuMusic();
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }


    public void SendNoTutorial()
    {
        PlayerPrefs.SetInt("TutorialStates", 0);
    }


    public void LoadGame()
    {
        DataManager dataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        if(dataMan.GetPremiumStatus())
        {
            SceneManager.LoadScene("Game Board");
        }
        else if(dataMan.GetPower() >= 20)
        {
            dataMan.TakePower();
            SceneManager.LoadScene("Game Board");
            AudioMusic.Instance.GameMusic();
        }
        else if (dataMan.GetPower() < 20)
        {
            GameObject.Find("PlayOption").SetActive(false);
            GameObject battery = GameObject.Find("Battery");
            battery.GetComponent<Image>().color = new Color(1f,0f,0f,1f);
            battery.transform.localScale = battery.transform.localScale * 1.1f;

            LeanTween.scale(battery, new Vector3(1f,1f,1f), 0.1f);
            LeanTween.color(battery.GetComponent<Image>().rectTransform, new Color(1f,1f,1f,1f), 0.1f);

            AdProposition.SetActive(true);
        }
    }

    public void LoadConnectRoom()
    {
        DataManager dataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        if(dataMan.GetPower() >= 20)
        {
            SceneManager.LoadScene("RoomConnection");
        }
        else 
        {
            GameObject.Find("PlayOption").SetActive(false);
            GameObject battery = GameObject.Find("Battery");
            battery.GetComponent<Image>().color = new Color(1f,0f,0f,1f);
            battery.transform.localScale = battery.transform.localScale * 1.1f;

            LeanTween.scale(battery, new Vector3(1f,1f,1f), 0.1f);
            LeanTween.color(battery.GetComponent<Image>().rectTransform, new Color(1f,1f,1f,1f), 0.1f);

            AdProposition.SetActive(true);
        }
    }

    private void ShowAd()
    {
        
    }

    public void LoadProfile()
    {
        SceneManager.LoadScene("Profile");
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
