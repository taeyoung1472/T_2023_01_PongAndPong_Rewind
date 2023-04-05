using UnityEditor;

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
