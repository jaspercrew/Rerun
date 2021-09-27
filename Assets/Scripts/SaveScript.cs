using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class SaveScript {
    
    
    
    public static List<List<int>> tempSave = new List<List<int>>();

    public static List<List<int>> saveProgress = new List<List<int>>();
             
    //it's static so we can call it from anywhere
    public static void Save() {
        SaveScript.saveProgress = tempSave;
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create (Application.persistentDataPath + "/saveFile.gd"); //you can call it anything you want
        bf.Serialize(file, SaveScript.saveProgress);
        file.Close();
    }   
     
    public static void Load() {
        if(File.Exists(Application.persistentDataPath + "/saveFile.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveFile.gd", FileMode.Open);
            SaveScript.saveProgress = (List<List<int>>)bf.Deserialize(file);
            file.Close();
        }
    }
}