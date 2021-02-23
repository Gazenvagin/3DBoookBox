using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadCover : MonoBehaviour
{
    public List<String> CoverBooks = new List<String>();
    void Awake()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);

        BoxSaveData boxSaveData = JsonUtility.FromJson<BoxSaveData>(jsonString);

        for (int i = 0; i < boxSaveData.PathCoverBook.Length; i++)
        {
            CoverBooks.Add(Application.streamingAssetsPath + boxSaveData.PathCoverBook[i]); 
        }
    }
}
