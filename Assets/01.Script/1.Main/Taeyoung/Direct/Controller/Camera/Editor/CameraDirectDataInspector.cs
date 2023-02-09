using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraDirectData))]
public class CameraDirectDataInspector : Editor
{
    SerializedObject serObj;
    CameraDirectData data;

    public void OnEnable()
    {
        serObj = new SerializedObject(target);
        data = (CameraDirectData)target;
    }

    public override void OnInspectorGUI()
    {
        serObj.Update();

        EditorGUILayout.PropertyField(serObj.FindProperty("typeList"), true);

        data.directTime = EditorGUILayout.FloatField("연출 지속시간", data.directTime);

        if (data.typeList.Contains(CameraDirectType.Shake))
        {
            GUILayout.Label("[Shake]", EditorStyles.boldLabel);
            data.shakeAmplitude = EditorGUILayout.FloatField("진폭", data.shakeAmplitude);
            data.shakeFrequency = EditorGUILayout.FloatField("빈도", data.shakeFrequency);
            data.shakeCurve = EditorGUILayout.CurveField("커브", data.shakeCurve);
        }

        if (data.typeList.Contains(CameraDirectType.Zoom))
        {
            GUILayout.Label("[Zoom]", EditorStyles.boldLabel);
            data.zoomValue = EditorGUILayout.FloatField("줌 배율", data.zoomValue);
            data.zoomCurve = EditorGUILayout.CurveField("커브", data.zoomCurve);
        }

        if (data.typeList.Contains(CameraDirectType.Rotate))
        {
            GUILayout.Label("[Rotate]", EditorStyles.boldLabel);
            data.rotateValue = EditorGUILayout.Vector3Field("회전값", data.rotateValue);
            data.rotateCurve = EditorGUILayout.CurveField("커브", data.rotateCurve);
        }

        if (data.typeList.Contains(CameraDirectType.Position))
        {
            GUILayout.Label("[Position]", EditorStyles.boldLabel);
            data.positionValue = EditorGUILayout.Vector3Field("이동값", data.positionValue);
            data.positionCurve = EditorGUILayout.CurveField("커브", data.positionCurve);
        }

        serObj.ApplyModifiedProperties();
    }
}
