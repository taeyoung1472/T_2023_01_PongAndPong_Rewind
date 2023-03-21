using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StageCorePos : MonoBehaviour
{
    [SerializeField] private CoreType playerType;

    private void OnDrawGizmos()
    {
        GUIStyle labelStyle = new();
        string labelString = "";

        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 24;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;

        switch (playerType)
        {
            case CoreType.Default:
                Gizmos.color = Color.blue;
                labelString = "순행자 시작";
                break;
            case CoreType.Rewind:
                Gizmos.color = Color.red;
                labelString = "역행자 시작";
                break;
            case CoreType.EndPoint:
                Gizmos.color = Color.magenta;
                labelString = "목표점";
                break;
        }
        Gizmos.DrawWireCube(transform.position + Vector3.up, new Vector3(1, 2, 1));
        Handles.Label(transform.position + Vector3.up * 2.5f, labelString, labelStyle);
    }

    enum CoreType
    {
        Default,
        Rewind,
        EndPoint,
    }
}
