using UnityEngine;

[CreateAssetMenu(menuName = "Data/Object")]
public class EditorObjectData : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;
    public ObjectType objectType;
}
public enum ObjectType
{
    Icon,
    Memo,
}