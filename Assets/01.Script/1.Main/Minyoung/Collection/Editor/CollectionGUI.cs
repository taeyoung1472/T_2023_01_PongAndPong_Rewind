using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollectionManager)), CanEditMultipleObjects]
public class CollectionGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("InitCollectionJson"))
        {
            //CollectionManager.Instance.InitSaveJSON();
        }
    }

  
}
