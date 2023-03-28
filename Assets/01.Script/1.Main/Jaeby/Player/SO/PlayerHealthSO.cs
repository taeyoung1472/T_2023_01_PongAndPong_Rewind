using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Health")]
public class PlayerHealthSO : ScriptableObject
{
    public int maxHP = 10;
    public float godModeTime = 1f;
}
