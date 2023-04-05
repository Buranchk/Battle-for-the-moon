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
        save.xp = 0;
        save.lvl = 0;
        save.skins = new List<bool>(){true, false, false, false};
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
        LoadSave();
        save.power = save.power - 1;
        SaveChanges();
    }

    public List<bool> GetSkins()
    {
        LoadSave();
        return save.skins;
    }
    
    public void SetSelectedSkin(int num, Skin skin)
    {
        LoadSave();
        save.skinSeletion = skin;
        save.selectedSkin = num;
        SaveChanges();
    }

    public void GiveGold(int amount)
    {
        LoadSave();
        save.coins += amount;
        SaveChanges();
    }

    public void GiveXP(int amount)
    {
        LoadSave();
        save.xp += amount;
        SaveChanges();
    }

    public void GiveRuby(int amount)
    {
        LoadSave();
        save.emeralds += amount;
        SaveChanges();
    }

    public void EndGame(bool status, int selectedSkin)
    {
        LoadSave();
        
        if(status)
        {
            print("u won");
            save.coins = save.coins + 20;
            save.emeralds += 2;
            save.xp = save.xp + 5;
                

            switch (selectedSkin){
                case 1:
                save.xp = save.xp + 5;
                break;

                case 2:
                save.coins = save.coins + 20;
                break;

                case 3:
                save.emeralds += 2;
                break;
            }
        }
        else if(!status)
        {
            print("u lost");
            save.coins = save.coins + 5;
            save.xp += 2;
            switch (selectedSkin){
                case 1:
                save.xp = save.xp + 2;
                break;

                case 2:
                save.coins = save.coins + 5;
                break;

                case 3:
                save.emeralds += 1;
                break;
            }  
        }

        save.lvl = LevelUpdate();
        SaveChanges();
    }

    public int LevelUpdate()
    {
        int[] lvls = new int[11]{0, 5, 10, 15, 25, 35, 50, 70, 90, 120, 150};
        for(int i = 0; i < 10; i ++)
        {
            //print("in loop of i - " + i);
            //print("there is xp " + save.xp + " and lvl selection is " + lvls[i]);
            if(save.xp < lvls[i])
            {
                //print("worked on - " + i);
                return i-1;
            }
        }
        return 10;
    }

    public Skin GiveSkinSelection()
    {
        LoadSave();
        return save.skinSeletion;
    }

    public bool CheckResources(string currency, int amount)
    {
        LoadSave();
        
        switch (currency)
        {
            case "gold":
            if(save.coins > amount)
                return true;
            else
                return false;

            case "ruby":
            if(save.emeralds > amount)
                return true;
            else
                return false;

            case "lvl":
            if(save.lvl > amount)
                return true;
            else
                return false;

        }
        return false;
    }

    public void TakeResources(string currency, int amount)
    {
        LoadSave();
        
        switch (currency)
        {
            case "gold":
            save.coins = save.coins - amount;
            break;

            case "ruby":
            save.emeralds = save.emeralds - amount;
            break;

            case "lvl":
            break;

        }
        SaveChanges();
    }

    public void OpenSkin(int skinType)
    {
        save.skins[skinType] = true;
        SaveChanges();
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
    public int lvl;
    public int xp;

    //Skin data
    public Skin skinSeletion;
    public List<bool> skins;
    public int selectedSkin;
}