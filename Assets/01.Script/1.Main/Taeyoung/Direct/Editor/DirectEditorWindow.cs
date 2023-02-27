using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DirectEditorWindow : EditorWindow
{
    Vector2 scrollPos;
    DirectController directController;
    List<DirectUnit> units { get { return directController.DirectList; } }

    [MenuItem("Custom/Direct")]
    public static void ShowWindow()
    {
        GetWindow<DirectEditorWindow>();
    }

    void OnGUI()
    {
        directController = EditorGUILayout.ObjectField("DirectController", directController, typeof(DirectController), true) as DirectController;

        if (directController == null)
        {
            EditorGUILayout.LabelField("DirectController 에 값을 할당해 주세요");
            return;
        }
        if(units.Count == 0)
        {
            if (GUILayout.Button("Add Item"))
            {
                GameObject obj = new GameObject();
                obj.name = $"Direct Unit";
                obj.AddComponent<DirectUnit>();
                obj.transform.SetParent(directController.transform);
                directController.DirectList.Add(obj.GetComponent<DirectUnit>());
            }
        }
        if (GUILayout.Button("Save"))
        {
            string scenePath = EditorSceneManager.GetActiveScene().path;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
        }

        bool isAppend = false;
        float appendTime = 0.0f;
        float time = 0;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        for (int i = 0; i < units.Count; i++)
        {
            SerializedObject serializedObject = new SerializedObject(units[i]);
            SerializedProperty unityEvent = serializedObject.FindProperty("unityEvent");
            serializedObject.Update();

            switch (units[i].sequenceType)
            {
                case SequenceType.Append:
                    GUI.backgroundColor = Color.red;
                    break;
                case SequenceType.Join:
                    GUI.backgroundColor = Color.yellow;
                    break;
                case SequenceType.AppendInterval:
                    GUI.backgroundColor = Color.blue;
                    break;
            }

            switch (units[i].sequenceType)
            {
                case SequenceType.Append:
                    if (isAppend)
                    {
                        time += appendTime;
                    }
                    isAppend = true;
                    appendTime = units[i].time;
                    break;
                case SequenceType.Join:
                    break;
                case SequenceType.AppendInterval:
                    if (isAppend)
                    {
                        time += appendTime;
                    }
                    time += units[i].time;
                    isAppend = false;
                    break;
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label($"연출 {i + 1}({time})", EditorStyles.boldLabel);
                if (GUILayout.Button("Add Item"))
                {
                    GameObject obj = new GameObject();
                    obj.name = $"Direct Unit";
                    obj.AddComponent<DirectUnit>();
                    obj.transform.SetParent(directController.transform);
                    directController.DirectList.Insert(i + 1, obj.GetComponent<DirectUnit>());
                }
                if (GUILayout.Button("Remove Item") && units.Count != 0)
                {
                    DestroyImmediate(units[i].gameObject);
                    units.RemoveAt(i);
                    GUILayout.EndHorizontal();
                    continue;
                }
            }
            GUILayout.EndHorizontal();

            units[i].sequenceType = (SequenceType)EditorGUILayout.EnumPopup("SequenceType", units[i].sequenceType);

            if (units[i].sequenceType != SequenceType.Join)
            {
                units[i].time = EditorGUILayout.FloatField("시간", units[i].time);
            }

            if (units[i].sequenceType != SequenceType.AppendInterval)
            {
                EditorGUILayout.PropertyField(unityEvent);
            }

            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(10);
        }
        EditorGUILayout.EndScrollView();
    }
}
