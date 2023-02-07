using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DirectEditorWindow : EditorWindow
{
    Vector2 scrollPos;
    DirectManager directManager;
    List<DirectUnit> units { get { return directManager.DirectList; } }

    [MenuItem("Window/Example Editor Window")]
    public static void ShowWindow()
    {
        GetWindow<DirectEditorWindow>();
    }

    void OnGUI()
    {
        directManager = EditorGUILayout.ObjectField("DirectManager", directManager, typeof(DirectManager), true) as DirectManager;

        if (directManager == null)
        {
            EditorGUILayout.LabelField("DirectManager 에 값을 할당해 주세요");
            return;
        }
        if(units.Count == 0)
        {
            if (GUILayout.Button("Add Item"))
            {
                GameObject obj = new GameObject();
                obj.name = $"Direct Unit";
                obj.AddComponent<DirectUnit>();
                obj.transform.SetParent(directManager.transform);
                directManager.DirectList.Add(obj.GetComponent<DirectUnit>());
            }
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
                    obj.transform.SetParent(directManager.transform);
                    directManager.DirectList.Insert(i + 1, obj.GetComponent<DirectUnit>());
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
