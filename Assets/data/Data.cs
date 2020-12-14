using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[Serializable]
public class Data
{
    private int money;

    [Serializable]
    public class Items
    {
        public List<int> ids;
    }

    public Items Loot;

    public void SaveLoot()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/data");
        FileStream file = File.Create(Application.persistentDataPath + "/data/" + "Loot.json");
        using (StreamWriter writter = new StreamWriter(file))
        {
            writter.Write(JsonUtility.ToJson(Loot));
        }
        file.Close();
    }
    public void AddItem(int id)
    {
        Loot.ids.Add(id);
        SaveLoot();
    }

    public void AddItems(int[] id)
    {
        Loot.ids.AddRange(id);
        SaveLoot();
    }
    public void AddMoney(int count)
    {
        money += count;
        SaveMoney();
    }

    public int GetMoney()
    {
        return money;
    }

    public bool RemoveMoney(int count)
    {
        if (money >= count)
        {
            money -= count;
            SaveMoney();
            return true;
        }
        else return false;
    }
    void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }

    public void Init()
    {
        money = PlayerPrefs.GetInt("Money");
        Loot.ids = new List<int>();
    }
}