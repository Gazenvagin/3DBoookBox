using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadPagesBook3 : MonoBehaviour
{
    public List<String> PagesBook3 = new List<String>();
    void Awake()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);

        BoxSaveData boxSaveData = JsonUtility.FromJson<BoxSaveData>(jsonString);

        for (int i = 0; i < boxSaveData.PathPagesBook3.Length; i++)
        {
            PagesBook3.Add(Application.streamingAssetsPath + boxSaveData.PathPagesBook3[i]); 
            //Debug.Log(boxSaveData.PathPagesBook3[i]);
        }
    }
}
