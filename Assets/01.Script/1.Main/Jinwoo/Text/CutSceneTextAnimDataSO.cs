using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/CutSceneTextAnimDataSO")]
public class CutSceneTextAnimDataSO : ScriptableObject
{

    public CutSceneText[] cutSceneTexts;

    public float timeBtwnChars;
    public float timeBtwnWords;
}
[Serializable]
public class CutSceneText
{
    [TextArea]
    public string text;
    public int textOrder;
}