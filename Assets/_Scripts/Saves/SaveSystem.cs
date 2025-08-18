using Newtonsoft.Json;
using System;
using UnityEngine;

public class SaveSystem
{
    public static PlayerData PlayerData { get; private set; }

    public static void Initialize()
    {
        LoadAll();
    }

    private static void LoadAll()
    {
        PlayerData = Load<PlayerData>(typeof(PlayerData).ToString()) ?? new PlayerData();
    }

    private static T Load<T>(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            var des = JsonConvert.DeserializeObject<T>(json);
            return des;
        }
        return default(T);
    }

    public static void Save<T>(T value) where T : class
    {
        var key = typeof(T).ToString();
        string json = JsonConvert.SerializeObject(value);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }
}

[Serializable]
public class PlayerData
{
    public int money;
    public int waveIndex;

    public float damageModifier;
    public float healthModifier;
    public float attackSpeedModifier;

    [JsonIgnore]
    public Action OnDataChanged;

    public void Save()
    {
        SaveSystem.Save(this);
        OnDataChanged?.Invoke();
    }
}