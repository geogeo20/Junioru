using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public static class LocalDataSaver
{
    /// <summary>
    /// Generic method to read data from the device
    /// </summary>
    /// <typeparam name="T">Data to read</typeparam>
    /// <param name="path">Path from where to read</param>
    /// <returns></returns>
    public static T ReadGameData<T>(string path) where T : class
    {
        string filePath = Path.Combine(Application.persistentDataPath, path);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            try
            {
                T data = JsonConvert.DeserializeObject<T>(dataAsJson);
                Debug.Log("[Local Data Saver] " + typeof(T).FullName + " read from the device");
                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log("[Local Data Saver] " + typeof(T).FullName + " could not read from the device");
                return null;
            }
        }
        else
        {
            Debug.Log("[Local Data Saver] No data found at the given path");
            return null;
        }
    }

    /// <summary>
    /// Generic method to save data on the device
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="data"></param>
    /// <param name="path">Path where to save the data</param>
    public static void SaveGameData<T>(T data, string path) where T : class
    {
        string filePath = Path.Combine(Application.persistentDataPath, path);

        string dataAsJson = JsonConvert.SerializeObject(data);

        try
        {
            File.WriteAllText(filePath, dataAsJson);
            Debug.Log("[Local Data Saver] " + typeof(T).FullName + " saved on the device");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("[Local Data Saver] Failed to save " + typeof(T).FullName + " on the device");
        }
    }

    public static void DeleteGameData(string path)
    {
        string filePath = Path.Combine(Application.persistentDataPath, path);

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log("[Local Data Saver] " + path + " deleted from the the device");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log("[Local Data Saver] Could not delete " + path + " from the the device");
            }
        }
    }
}