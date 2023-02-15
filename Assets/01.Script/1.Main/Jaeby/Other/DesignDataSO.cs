using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/DesignData")]
public class DesignDataSO : ScriptableObject
{
    [Header("아이콘")]
    public Sprite _nomalIcon = null;
    public Sprite _traderIcon = null;
    public Sprite _specialIcon = null;
    [Header("색깔")]
    public Color _nomalColor = Color.white;
    public Color _traderColor = Color.white;
    public Color _specialColor = Color.white;
}
