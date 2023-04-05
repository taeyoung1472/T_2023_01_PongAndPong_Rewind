using UnityEngine;

[CreateAssetMenu(menuName = "SO/NPC/Data")]
public class NPCData : ScriptableObject
{
    public NPCType npcType = NPCType.None;
    public IconType iconType = IconType.None;
    public string npcName = "";
}
