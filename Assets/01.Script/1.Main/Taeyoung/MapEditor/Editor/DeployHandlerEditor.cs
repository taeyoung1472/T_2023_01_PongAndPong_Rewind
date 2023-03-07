using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DeployHandler))]
public class DeployHandlerEditor : Editor
{
    DeployHandler myScript;

    private void OnEnable()
    {
        myScript = target as DeployHandler;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }
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
