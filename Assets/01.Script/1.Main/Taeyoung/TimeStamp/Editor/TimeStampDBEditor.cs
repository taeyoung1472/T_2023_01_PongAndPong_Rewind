using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimeStampDataBase))]
public class TimeStampDBEditor : Editor
{
    TimeStampDataBase myClass;

    private void OnEnable()
    {
        myClass = (TimeStampDataBase)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Rename File"))
        {
            RenameFiles();
        }
        DrawDefaultInspector();
    }

    private void RenameFiles()
    {
        string oldPath;
        string newPath;

        for (int i = 0; i < myClass.stampDataArr.Length; i++)
        {
            oldPath = AssetDatabase.GetAssetPath(myClass.stampDataArr[i].sprite);
            newPath = "Assets/02.Resource/Sprite/TimeStampIcon" + $"/{(int)myClass.stampDataArr[i].type}_{myClass.stampDataArr[i].type}.png";

            if (oldPath == newPath)
                continue;

            if (File.Exists(newPath))
            {
                string tempPath = "Assets/02.Resource/Sprite/TimeStampIcon" + $"/OLD_{(int)myClass.stampDataArr[i].type}_{myClass.stampDataArr[i].type}_Hash:{Random.Range(0, 99999)}.wav";
                Debug.Log(tempPath + " , " + newPath);
                AssetDatabase.MoveAsset(newPath, tempPath);
                AssetDatabase.Refresh();
            }

            AssetDatabase.MoveAsset(oldPath, newPath);
            AssetDatabase.Refresh();
        }
    }

}
