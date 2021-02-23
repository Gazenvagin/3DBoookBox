using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadPagesBook2 : MonoBehaviour
{
    public List<String> PagesBook2 = new List<String>();
    void Awake()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);

        BoxSaveData boxSaveData = JsonUtility.FromJson<BoxSaveData>(jsonString);

        for (int i = 0; i < boxSaveData.PathPagesBook2.Length; i++)
        {
            PagesBook2.Add(Application.streamingAssetsPath + boxSaveData.PathPagesBook2[i]); 
            //Debug.Log(boxSaveData.PathPagesBook2[i]);
        }
    }
}
