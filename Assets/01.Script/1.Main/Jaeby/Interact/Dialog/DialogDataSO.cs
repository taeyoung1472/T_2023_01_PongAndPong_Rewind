using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Dialog/Data")]
public class DialogDataSO : ScriptableObject
{
    public DialogDataSO nextData;
    public List<string> texts;
    public float nextCharDelay = 0.05f;
}
