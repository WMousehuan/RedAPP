/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using dotmob;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class LocalDataManager
{


    public static readonly string FILE_NAME_TABLE_SPEC_COIN = "SpecCoin.txt";

    public static readonly string FILE_NAME_TABLE_SPEC_ITEM = "SpecItem.txt";

    public static readonly string FILE_NAME_TABLE_OPTION = "Option.txt";

    public static readonly string FILE_NAME_TABLE_LEVEL_DATA = "LevelData.txt";

    public static readonly string FILE_NAME_TABLE_MAP_DATA = "MapData.txt";

    private const string C_FOLDER_DATA_DECRYPT = "dotmob_data_decrypt";
    

    public static string GetFileContents(string fileName, bool onlyPersistentPath = false)
    {
        string text = string.Empty;
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            using (new MemoryStream())
            {
                try
            {
                text = DataEncryption.DecryptString(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                text = null;
                File.Delete(path);
                Debug.Log("Jewel Hunter LocalDataManager.GetFileContents MemoryStream ex: " + ex.Message);
            }
            }
        }
        if (string.IsNullOrEmpty(text))
        {
           // Debug.Log("Jewel Hunter LocalDataManager.GetFileContents start fileName:" + fileName);
            if (onlyPersistentPath)
            {
                text = string.Empty;
            }
            try
            {
                text = Utils.GetResourcesStringFile(RemoveFileNameExtension(C_FOLDER_DATA_DECRYPT + "/" + fileName));


            }
            catch (Exception ex)
            {
                Debug.Log("Jewel Hunter LocalDataManager.GetFileContents ex: " + ex.Message);
            }
        }
       // Debug.Log("Jewel Hunter LocalDataManager.GetFileContents end fileName:" + fileName);
        return text;
    }

    public static void SaveFile(bool isResourcesFolder, string fileName, string jsonData)
    {
        try
        {
            if (!string.IsNullOrEmpty(jsonData) && !string.IsNullOrEmpty(fileName))
            {
                if (!Application.isEditor)
                {
                    isResourcesFolder = false;
                }

                string path = (!isResourcesFolder) ? Path.Combine(Application.persistentDataPath, fileName)
                    : (Application.dataPath + "/Resources/" + C_FOLDER_DATA_DECRYPT + "/network/" + fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.WriteAllText(path, DataEncryption.EncryptString(jsonData));
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Jewel Hunter LocalDataManager.SaveFile ex: " + ex.Message);
        }
    }

    public static bool ExistLocalTableFile(string tableName)
    {
        string empty = string.Empty;
        switch (tableName)
        {
            case "coin":
                empty = FILE_NAME_TABLE_SPEC_COIN;
                break;
            case "item":
                empty = FILE_NAME_TABLE_SPEC_ITEM;
                break;
            case "game":
                empty = FILE_NAME_TABLE_LEVEL_DATA;
                break;
            case "map":
                empty = FILE_NAME_TABLE_MAP_DATA;
                break;
            case "option":
                empty = FILE_NAME_TABLE_OPTION;
                break;
            default:
                return false;
        }
        string path = Path.Combine(Application.persistentDataPath, empty);
        if (File.Exists(path))
        {
            return true;
        }
        return true;
    }

    private static string RemoveFileNameExtension(string fileName)
    {
        if (fileName.Length > 4)
        {
            return fileName.Substring(0, fileName.Length - 4);
        }
        return string.Empty;
    }
}