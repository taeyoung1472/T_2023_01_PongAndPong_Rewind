using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Memo")]
public class PrepareMemoData : ScriptableObject
{
    public Sprite icon;
    public MemoType memoType;
}
public enum MemoType
{
    Icon,
    Memo,
}