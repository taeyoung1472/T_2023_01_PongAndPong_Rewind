using UnityEngine;

public abstract class ControlAbleObjcet : MonoBehaviour
{
    protected ControlType curControlType = ControlType.None;
    public abstract void Control(ControlType controlType, bool isLever, Player player);

    public bool isLocked =false;

}
public enum ControlType
{
    Control,
    None,
    ReberseControl,
}
