using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GimmickVisualLink)), CanEditMultipleObjects]
public class GimmickVisualLinkHandler : Editor
{
    private GimmickVisualLink myScript;
    private GUIStyle labelStyle = new();

    private void OnEnable()
    {
        myScript = target as GimmickVisualLink;

        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 32;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set"))
        {
            foreach (var obj in Selection.gameObjects)
            {
                if (obj.TryGetComponent<GimmickVisualLink>(out GimmickVisualLink link))
                {
                    link.Reset();
                }
            }
        }
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < myScript.pivots.Count; i++)
        {
            Handles.Label(myScript.pivots[i], (i + 1).ToString(), labelStyle);
            myScript.pivots[i] = Handles.PositionHandle(myScript.pivots[i], Quaternion.identity);
        }
        myScript.Generate();
    }
}
