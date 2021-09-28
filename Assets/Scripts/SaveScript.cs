using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public static class SaveScript {
    
    // TODO: redo this

    private static readonly List<List<int>> TempSave = new List<List<int>>();

    private static List<List<int>> _saveProgress = new List<List<int>>();
             
    //it's static so we can call it from anywhere
    public static void Save() {
        _saveProgress = TempSave;
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into
        //debug.log if you want to know where save games are located
        FileStream file = File.Create (Application.persistentDataPath + "/saveFile.gd"); 
        bf.Serialize(file, _saveProgress);
        file.Close();
    }   
     
    public static void Load() {
        if(File.Exists(Application.persistentDataPath + "/saveFile.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveFile.gd", FileMode.Open);
            _saveProgress = (List<List<int>>)bf.Deserialize(file);
            file.Close();
        }
    }
}