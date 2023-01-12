using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
   public Save save = new Save();
   private string path;

    void Awake()
    {
//OS path selection
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save.json");
#else
        path = Path.Combine(Application.dataPath, "Save.json");
#endif
    DontDestroyOnLoad(this.gameObject);
    }

    public bool LoadSave()
    {   
        if (File.Exists(path))
        {
            save = JsonUtility.FromJson<Save>(File.ReadAllText(path));
            return true;
        }
        else
            return false;
    }

    public void SaveChanges()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        File.WriteAllText(path, JsonUtility.ToJson(save));
#endif
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }

    public void InitializeAccount(string naming)
    {
        print("New Account");
        save.name = naming;
        save.coins = 0;
        save.emeralds = 0;
        save.power = 100;
        save.lvl = 0.0f;
        save.skins = new List<bool>(){true, false, true, false};
        save.selectedSkin = 0;
        SaveChanges();
    }

    public Save GetSave()
    {
        LoadSave();
        return save;
    }

    public void SetSave(Save sv)
    {
        save = sv;
        SaveChanges();
    }

    public int GivePower()
    {
        LoadSave();
        return save.power;
    }

    public int GiveSelectedSkin()
    {
        LoadSave();
        return save.selectedSkin;
    }

    public void TakePower()
    {
        save = GetSave();
        save.power = save.power - 1;
        SetSave(save);
    }

}

[Serializable]
public class Save
{
    //Stat data
    public string name;
    public int coins;
    public int emeralds;
    public int power;
    public float lvl;

    //Skin data
    public List<bool> skins;
    public int selectedSkin;
}