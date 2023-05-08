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
        save.gold = 0;
        save.ruby = 0;
        save.energy = 100;
        save.xp = 0;
        save.lvl = 0;
        save.skins = new List<bool>(){true, false, false, false};
        save.selectedSkin = 0;
        SaveChanges();
    }


    //Getters//
    public int GetPower()
    {
        LoadSave();
        return save.energy;
    }

    public string GetName()
    {
        LoadSave();
        return save.name;
    }

    public int GetGold()
    {
        LoadSave();
        return save.gold;
    }

    public int GetRuby()
    {
        LoadSave();
        return save.ruby;
    }

    public int GetLvl()
    {
        LoadSave();
        return save.lvl;
    }

    public int GetXP()
    {
        LoadSave();
        return save.xp;
    }

    public int GetSelectedSkin()
    {
        LoadSave();
        return save.selectedSkin;
    }

    public List<bool> GetSkins()
    {
        LoadSave();
        return save.skins;
    }

    public Skin GetSkinSelection()
    {
        LoadSave();
        return save.skinSeletion;
    }
    
    
    //Setters//

    public void TakePower()
    {
        LoadSave();
        save.energy = save.energy - 1;
        SaveChanges();
    }

    public void GiveGold(int amount)
    {
        LoadSave();
        save.gold += amount;
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
        save.ruby += amount;
        SaveChanges();
    }

    public void SetSelectedSkin(int num, Skin skin)
    {
        LoadSave();
        save.skinSeletion = skin;
        save.selectedSkin = num;
        SaveChanges();
    }

    public void TakeResources(string currency, int amount)
    {
        LoadSave();
        
        switch (currency)
        {
            case "gold":
            save.gold = save.gold - amount;
            break;

            case "ruby":
            save.ruby = save.ruby - amount;
            break;

            case "lvl":
            break;

        }
        SaveChanges();
    }

    public void NextLvl()
    {
        int[] lvls = new int[11]{0, 5, 10, 15, 25, 35, 50, 70, 90, 120, 150};
        save.xp = lvls[save.lvl + 1];
        save.lvl = save.lvl + 1;
        SaveChanges();
    }

    public void FillEnergy()
    {
        save.energy = 100;
        SaveChanges();
    }

    //Functions

    public void EndGame(bool status, int selectedSkin)
    {
        LoadSave();
        
        if(status)
        {
            print("u won");
            save.gold = save.gold + 20;
            save.ruby += 2;
            save.xp = save.xp + 5;
                

            switch (selectedSkin){
                case 1:
                save.xp = save.xp + 5;
                break;

                case 2:
                save.gold = save.gold + 20;
                break;

                case 3:
                save.ruby += 2;
                break;
            }
        }
        else if(!status)
        {
            print("u lost");
            save.gold = save.gold + 5;
            save.xp += 2;
            switch (selectedSkin){
                case 1:
                save.xp = save.xp + 2;
                break;

                case 2:
                save.gold = save.gold + 5;
                break;

                case 3:
                save.ruby += 1;
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

    public bool CheckResources(string currency, int amount)
    {
        LoadSave();
        
        switch (currency)
        {
            case "gold":
            if(save.gold >= amount)
                return true;
            else
                return false;

            case "ruby":
            if(save.ruby >= amount)
                return true;
            else
                return false;

            case "lvl":
            if(save.lvl >= amount)
                return true;
            else
                return false;

            case "energy":
            if(save.energy >= amount)
                return true;
            else
                return false;


        }
        return false;
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
    public int gold;
    public int ruby;
    public int energy;
    public int lvl;
    public int xp;

    //Skin data
    public Skin skinSeletion;
    public List<bool> skins;
    public int selectedSkin;
}