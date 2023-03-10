using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DeployHandler)), CanEditMultipleObjects]
public class DeployHandlerEditor : Editor
{
    private DeployHandler myScript;
    //private GUIStyle boldLabelStyle = new();

    //SerializedProperty centerXProperty;
    //SerializedProperty centerYProperty;

    //SerializedProperty decalCenterXProperty;
    //SerializedProperty decalCenterYProperty;

    //SerializedProperty decalRotXProperty;
    //SerializedProperty decalRotYProperty;
    //SerializedProperty decalRotZProperty;
    
    private void OnEnable()
    {
        myScript = target as DeployHandler;

        //boldLabelStyle = new GUIStyle(EditorStyles.label);
        //boldLabelStyle.fontStyle = FontStyle.Bold;

        //centerXProperty = serializedObject.FindProperty(nameof(myScript.centerX));
        //centerYProperty = serializedObject.FindProperty(nameof(myScript.centerY));

        //decalCenterXProperty = serializedObject.FindProperty(nameof(myScript.centerX_decal));
        //decalCenterYProperty = serializedObject.FindProperty(nameof(myScript.centerY_decal));

        //decalRotXProperty = serializedObject.FindProperty(nameof(myScript.rotX));
        //decalRotYProperty = serializedObject.FindProperty(nameof(myScript.rotY));
        //decalRotZProperty = serializedObject.FindProperty(nameof(myScript.rotZ));
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        #region CustomInspector
        //serializedObject.Update();

        //myScript.firstAxis = EditorGUILayout.Vector3Field("First Axis", myScript.firstAxis);
        //myScript.secondAxis = EditorGUILayout.Vector3Field("Second Axis", myScript.secondAxis);
        //GUILayout.Label("Prefab ????", boldLabelStyle);
        //myScript.prefab = (MapElement)EditorGUILayout.ObjectField("Prefab", myScript.prefab, typeof(MapElement), true);
        //myScript.prefabSizeFactor = EditorGUILayout.FloatField("Size Factor", myScript.prefabSizeFactor);
        //GUILayout.BeginHorizontal();
        //EditorGUILayout.PropertyField(centerXProperty);
        //EditorGUILayout.PropertyField(centerYProperty);
        //GUILayout.EndHorizontal();
        //GUILayout.Label("Decal ????", boldLabelStyle);
        //myScript.decalPrefab = (MapElement)EditorGUILayout.ObjectField("Decal", myScript.decalPrefab, typeof(MapElement), true);
        //myScript.decalChance = EditorGUILayout.FloatField("Chance", myScript.decalChance);
        //myScript.decalSizeFactor = EditorGUILayout.FloatField("Size Factor", myScript.decalSizeFactor);
        //GUILayout.BeginHorizontal();
        //EditorGUILayout.PropertyField(decalCenterXProperty);
        //EditorGUILayout.PropertyField(decalCenterYProperty);
        //GUILayout.EndHorizontal();
        //GUILayout.BeginHorizontal();
        //EditorGUILayout.PropertyField(decalRotXProperty);
        //EditorGUILayout.PropertyField(decalRotYProperty);
        //EditorGUILayout.PropertyField(decalRotZProperty);
        //GUILayout.EndHorizontal();
        #endregion

        if (GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }
        if (GUILayout.Button("Clear"))
        {
            myScript.Clear();
        }
        if (GUILayout.Button("ChangeDrawMode"))
        {
            DeployHandler.isDrawWire = !DeployHandler.isDrawWire;
        }

        //serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        Vector3 center = (myScript.firstAxis + myScript.secondAxis) / 2;

        switch (myScript.deployType)
        {
            case DeployType.XY:
                myScript.transform.position = new Vector3(center.x, center.y, myScript.transform.position.z);
                break;
            case DeployType.XZ:
                myScript.transform.position = new Vector3(center.x, myScript.transform.position.y, center.z);
                break;
            case DeployType.YZ:
                myScript.transform.position = new Vector3(myScript.transform.position.x, center.y, center.z);
                break;
        }

        switch (myScript.deployType)
        {
            case DeployType.XY:
                myScript.firstAxis.z = myScript.transform.position.z;
                myScript.secondAxis.z = myScript.transform.position.z;
                break;
            case DeployType.XZ:
                myScript.firstAxis.y = myScript.transform.position.y;
                myScript.secondAxis.y = myScript.transform.position.y;
                break;
            case DeployType.YZ:
                myScript.firstAxis.x = myScript.transform.position.x;
                myScript.secondAxis.x = myScript.transform.position.x;
                break;
        }

        myScript.firstAxis = Handles.PositionHandle(myScript.firstAxis, Quaternion.identity);
        myScript.secondAxis = Handles.PositionHandle(myScript.secondAxis, Quaternion.identity);
    }
}
