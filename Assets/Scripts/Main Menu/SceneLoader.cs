using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        print(GameObject.Find("Data Manager").GetComponent<DataManager>().GetPower());
        if(GameObject.Find("Data Manager").GetComponent<DataManager>().GetPower() > 33){
            GameObject.Find("Data Manager").GetComponent<DataManager>().TakePower();
            SceneManager.LoadScene("Game Board");
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
