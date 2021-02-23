using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadPagesBook0 : MonoBehaviour
{
    public List<String> PagesBook0 = new List<String>();
    void Awake()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);

        BoxSaveData boxSaveData = JsonUtility.FromJson<BoxSaveData>(jsonString);

        for (int i = 0; i < boxSaveData.PathPagesBook0.Length; i++)
        {
            PagesBook0.Add(Application.streamingAssetsPath + boxSaveData.PathPagesBook0[i]); 
            //Debug.Log(boxSaveData.PathPagesBook0[i]);
        }
    }
}
