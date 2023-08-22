using System;
using UnityEngine;

public abstract class ControlAbleObjcet : MonoBehaviour
{
    protected ControlType curControlType = ControlType.None;
    public abstract void Control(ControlType controlType, bool isLever, Player player, DirectionType dirType);
    public abstract void ResetObject();

    public virtual void SetColor() { }

    public bool isLocked =false;
    [HideInInspector] public Color controlColor;
 
}
public enum ControlType
{
    Control,
    None,
    ReberseControl,
}
