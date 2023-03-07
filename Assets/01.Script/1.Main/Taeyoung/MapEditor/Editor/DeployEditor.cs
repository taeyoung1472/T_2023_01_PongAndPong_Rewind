using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DeployEditor : EditorWindow
{
    [MenuItem("Custom/MapEditor")]
    public static void ShowWindow()
    {
        GetWindow<DeployEditor>();
    }

    void OnGUI()
    {

    }
}
