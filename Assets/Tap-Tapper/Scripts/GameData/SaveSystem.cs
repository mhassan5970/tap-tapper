using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public static class SaveSystem
{
    //public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static void Init(PlayerData playerData)
    {
        string data = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("playerData", data);
        //if (!Directory.Exists(SAVE_FOLDER))
        //{
        //    Directory.CreateDirectory(SAVE_FOLDER);
        //    string saveString = JsonUtility.ToJson(playerData);
        //    File.WriteAllText(SAVE_FOLDER + "save.json", saveString);

        //    Debug.Log("Save FileCreated");
        //}
        //else
        //{
        //    if (!File.Exists(SAVE_FOLDER + "/save.json"))
        //    {
        //        string saveString = JsonUtility.ToJson(playerData);
        //        File.WriteAllText(SAVE_FOLDER + "save.json", saveString);

        //        Debug.Log("Save FileCreated");
        //    }
        //}
    }

    public static void Save(PlayerData playerData)
    {
        //string saveString = JsonUtility.ToJson(playerData);
        //File.WriteAllText(SAVE_FOLDER + "save.json", saveString);

        playerData.saveVersion += 0.1f; //save version of data

        string data = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("playerData", data);

        Debug.Log("Data Saved");
    }

    public static string Load()
    {
        string data = PlayerPrefs.GetString("playerData");

        return data;
        //if (File.Exists(SAVE_FOLDER + "/save.json"))
        //{
        //    string playerData = File.ReadAllText(SAVE_FOLDER + "save.json");
        //    return playerData;
        //}
        //else
        //{
        //    return null;
        //}
    }

}