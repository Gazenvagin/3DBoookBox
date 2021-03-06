using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadPagesBook5 : MonoBehaviour
{
    public List<String> PagesBook5 = new List<String>();
    void Awake()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);

        BoxSaveData boxSaveData = JsonUtility.FromJson<BoxSaveData>(jsonString);

        for (int i = 0; i < boxSaveData.PathPagesBook5.Length; i++)
        {
            PagesBook5.Add(Application.streamingAssetsPath + boxSaveData.PathPagesBook5[i]); 
            //Debug.Log(boxSaveData.PathPagesBook5[i]);
        }
    }
}
