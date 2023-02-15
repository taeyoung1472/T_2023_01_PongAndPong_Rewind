using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/DesignData")]
public class DesignDataSO : ScriptableObject
{
    [Header("������")]
    public Sprite _nomalIcon = null;
    public Sprite _traderIcon = null;
    public Sprite _specialIcon = null;
    [Header("����")]
    public Color _nomalColor = Color.white;
    public Color _traderColor = Color.white;
    public Color _specialColor = Color.white;
}
