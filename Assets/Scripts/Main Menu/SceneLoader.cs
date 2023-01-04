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
        print(GameObject.Find("Data Manager").GetComponent<DataManager>().GivePower());
        if(GameObject.Find("Data Manager").GetComponent<DataManager>().GivePower() > 33){
            GameObject.Find("Menu Manager").GetComponent<MenuManager>().TakePower();
            SceneManager.LoadScene("Game Board");
        }
    }

    public void LoadProfile()
    {
        SceneManager.LoadScene("Profile");
    }
}
