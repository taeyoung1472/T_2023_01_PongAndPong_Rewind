using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogOption : MonoBehaviour
{
    public Sprite icon = null;
    public string explainText = null;
    public DialogDataSO _nextDialogData = null;
    public UnityEvent ExcuteAction = null;
}
