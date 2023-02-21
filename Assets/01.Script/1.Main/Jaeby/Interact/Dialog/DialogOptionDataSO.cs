using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "SO/DialogOption/Data")]
public class DialogOptionDataSO : ScriptableObject
{
    public Sprite icon = null;
    public string explainText = null;
    public DialogDataSO _nextDialogData = null;
    public bool actionExit = true;
    public List<DialogOptionDataSO> dialogOptions = new List<DialogOptionDataSO>();
    public string eventKey = "";
}
