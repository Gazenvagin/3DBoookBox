using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadPagesBook4 : MonoBehaviour
{
    public List<String> PagesBook4 = new List<String>();
    void Awake()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);

        BoxSaveData boxSaveData = JsonUtility.FromJson<BoxSaveData>(jsonString);

        for (int i = 0; i < boxSaveData.PathPagesBook4.Length; i++)
        {
            PagesBook4.Add(Application.streamingAssetsPath + boxSaveData.PathPagesBook4[i]); 
            //Debug.Log(boxSaveData.PathPagesBook4[i]);
        }
    }
}
