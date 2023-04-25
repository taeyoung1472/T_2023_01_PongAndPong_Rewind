using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/TextAnimDataSO")]
public class TextAnimDataSO : ScriptableObject
{
    //public CutSceneData[] array;
    public string[] stringArray;

    public float timeBtwnChars;
    public float timeBtwnWords;
}
[Serializable]
public class CutSceneData
{
    public string stringArray;
    public GameObject img;
}