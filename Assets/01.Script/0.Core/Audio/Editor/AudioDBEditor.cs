using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioDataBase))]
public class AudioDBEditor : Editor
{
    AudioDataBase myClass;

    private void OnEnable()
    {
        myClass = (AudioDataBase)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Rename File"))
        {
            RenameFiles();
        }
        DrawDefaultInspector();
    }

    private void RenameFiles()
    {
        string oldPath;
        string newPath;

        for (int i = 0; i < myClass.soundDataArr.Length; i++)
        {
            if (myClass.soundDataArr[i].clips.Length > 1)
            {
                for (int j = 0; j < myClass.soundDataArr[i].clips.Length; j++)
                {
                    oldPath = AssetDatabase.GetAssetPath(myClass.soundDataArr[i].clips[j]);
                    newPath = "Assets/02.Resource/Audio/Game" + $"/{(int)myClass.soundDataArr[i].type}_{j}_{myClass.soundDataArr[i].type}";
                    AssetDatabase.MoveAsset(oldPath, newPath);
                    AssetDatabase.Refresh();
                }
                continue;
            }
            else
            {
                oldPath = AssetDatabase.GetAssetPath(myClass.soundDataArr[i].clips[0]);
                newPath = "Assets/02.Resource/Audio/Game" + $"/{(int)myClass.soundDataArr[i].type}_{myClass.soundDataArr[i].type}";
                AssetDatabase.MoveAsset(oldPath, newPath);
                AssetDatabase.Refresh();
                continue;
            }
        }
    }
}
