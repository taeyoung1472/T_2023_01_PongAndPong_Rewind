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

        data.directTime = EditorGUILayout.FloatField("���� ���ӽð�", data.directTime);

        if (data.typeList.Contains(CameraDirectType.Shake))
        {
            GUILayout.Label("[Shake]", EditorStyles.boldLabel);
            data.shakeAmplitude = EditorGUILayout.FloatField("����", data.shakeAmplitude);
            data.shakeFrequency = EditorGUILayout.FloatField("��", data.shakeFrequency);
            data.shakeCurve = EditorGUILayout.CurveField("Ŀ��", data.shakeCurve);
        }

        if (data.typeList.Contains(CameraDirectType.Zoom))
        {
            GUILayout.Label("[Zoom]", EditorStyles.boldLabel);
            data.zoomValue = EditorGUILayout.FloatField("�� ����", data.zoomValue);
            data.zoomCurve = EditorGUILayout.CurveField("Ŀ��", data.zoomCurve);
        }

        if (data.typeList.Contains(CameraDirectType.Rotate))
        {
            GUILayout.Label("[Rotate]", EditorStyles.boldLabel);
            data.rotateValue = EditorGUILayout.Vector3Field("ȸ����", data.rotateValue);
            data.rotateCurve = EditorGUILayout.CurveField("Ŀ��", data.rotateCurve);
        }

        if (data.typeList.Contains(CameraDirectType.Position))
        {
            GUILayout.Label("[Position]", EditorStyles.boldLabel);
            data.positionValue = EditorGUILayout.Vector3Field("�̵���", data.positionValue);
            data.positionCurve = EditorGUILayout.CurveField("Ŀ��", data.positionCurve);
        }

        serObj.ApplyModifiedProperties();
    }
}
