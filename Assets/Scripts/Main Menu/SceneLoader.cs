using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void LoadGame()
    {
        DataManager dataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        if(dataMan.GetPower() >= 20)
        {
            dataMan.TakePower();
            SceneManager.LoadScene("Game Board");
        }
        else 
        {
            GameObject battery = GameObject.Find("Battery");
            battery.GetComponent<Image>().color = new Color(1f,0f,0f,1f);
            battery.transform.localScale = battery.transform.localScale * 1.1f;

            LeanTween.scale(battery, new Vector3(1f,1f,1f), 0.1f);
            LeanTween.color(battery.GetComponent<Image>().rectTransform, new Color(1f,1f,1f,1f), 0.1f);
        }
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
