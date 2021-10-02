using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveScript {
    
    private static bool[][] _saveProgress = new bool[Constants.NumWorlds][];
    private static float[] _settings = new float[2];
    private static BinaryFormatter _bf = new BinaryFormatter();

    private static readonly string SaveLocation = Application.persistentDataPath + "/save.rr";
    private static readonly string SettingsLocation = Application.persistentDataPath + "/settings.rr";

    // this isn't a constructor; it's a static constructor, which basically means
    // it gets called automatically at the beginning, kinda like Start() i think
    static SaveScript()
    {
        for (int i = 0; i < _saveProgress.Length; i++)
        {
            _saveProgress[i] = new bool[Constants.LevelsPerWorld];
        }

        Debug.Log("loading save and settings");
        Load();
    }

    public static void Save() {
        // TODO: compare file sizes when saving via serialization vs. plain text
        
        FileStream file = File.Create(SaveLocation); 
        _bf.Serialize(file, _saveProgress);
        file.Close();
        Debug.Log("saved level completion data to " + SaveLocation);
        
        file = File.Create(SettingsLocation); 
        _bf.Serialize(file, _settings);
        file.Close();
        Debug.Log("saved settings data to " + SettingsLocation);
    }

    public static void Load()
    {
        if (File.Exists(SaveLocation))
        {
            FileStream file = File.Open(SaveLocation, FileMode.Open);
            _saveProgress = (bool[][]) _bf.Deserialize(file);
            file.Close();
        }
        
        if (File.Exists(SettingsLocation))
        {
            FileStream file = File.Open(SettingsLocation, FileMode.Open);
            _settings = (float[]) _bf.Deserialize(file);
            file.Close();
        }
    }

    public static void UpdateLevel(int world, int level, bool complete = true)
    {
        _saveProgress[world][level] = complete;
    }

    public static bool IsLevelComplete(int world, int level)
    {
        return _saveProgress[world][level];
    }

    public static void UpdateSettings(float sound, float music)
    {
        _settings[0] = sound;
        _settings[1] = music;
    }

    public static float GetSound() => _settings[0];
    
    public static float GetMusic() => _settings[1];
}