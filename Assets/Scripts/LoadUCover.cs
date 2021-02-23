using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadUCover : MonoBehaviour
{
    public List<String> CoverUBooks = new List<String>();
    void Awake()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);

        BoxSaveData boxSaveData = JsonUtility.FromJson<BoxSaveData>(jsonString);

        for (int i = 0; i < boxSaveData.PathUCoverBook.Length; i++)
        {
            CoverUBooks.Add(Application.streamingAssetsPath + boxSaveData.PathUCoverBook[i]); 
            //Debug.Log(boxSaveData.PathUCoverBook[i]);
        }
    }
}
