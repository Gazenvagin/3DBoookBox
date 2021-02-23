using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadPagesBook1 : MonoBehaviour
{
    public List<String> PagesBook1 = new List<String>();
    void Awake()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);

        BoxSaveData boxSaveData = JsonUtility.FromJson<BoxSaveData>(jsonString);

        for (int i = 0; i < boxSaveData.PathPagesBook1.Length; i++)
        {
            PagesBook1.Add(Application.streamingAssetsPath + boxSaveData.PathPagesBook1[i]); 
            //Debug.Log(boxSaveData.PathPagesBook1[i]);
        }
    }
}
