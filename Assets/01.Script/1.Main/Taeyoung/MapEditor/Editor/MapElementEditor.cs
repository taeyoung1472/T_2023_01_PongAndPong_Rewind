using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapElement))]
public class MapElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapElement myScript = target as MapElement;

        myScript.deployType = (DeployType)EditorGUILayout.EnumPopup("DeployType", myScript.deployType);

        switch (myScript.deployType)
        {
            case DeployType.XY:
                myScript.size.x = EditorGUILayout.FloatField("Width", myScript.size.x);
                myScript.size.y = EditorGUILayout.FloatField("Height", myScript.size.y);
                break;
            case DeployType.XZ:
                myScript.size.x = EditorGUILayout.FloatField("Width", myScript.size.x);
                myScript.size.z = EditorGUILayout.FloatField("Height", myScript.size.z);
                break;
            case DeployType.YZ:
                myScript.size.y = EditorGUILayout.FloatField("Width", myScript.size.y);
                myScript.size.z = EditorGUILayout.FloatField("Height", myScript.size.z);
                break;
        }

        if (GUILayout.Button("Collider 기준으로"))
        {
            Collider col = myScript.GetComponent<Collider>();
            if (col == null)
            {
                Debug.LogWarning("콜라이더가 없는데?");
                return;
            }
            switch (myScript.deployType)
            {
                case DeployType.XY:
                    myScript.size.x = col.bounds.size.x;
                    myScript.size.y = col.bounds.size.y;
                    break;
                case DeployType.XZ:
                    myScript.size.x = col.bounds.size.x;
                    myScript.size.z = col.bounds.size.z;
                    break;
                case DeployType.YZ:
                    myScript.size.y = col.bounds.size.y;
                    myScript.size.z = col.bounds.size.z;
                    break;
            }
        }
    }
}
