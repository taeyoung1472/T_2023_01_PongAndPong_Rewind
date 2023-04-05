using UnityEngine;

public class LaserGimmickObjcet : GimmickObject
{
    [SerializeField] protected bool isLaserReflect;
    public bool IsLaserReflect { get { return isLaserReflect; } }

    public override void Init()
    {

    }

    public virtual void LaserExcute() { }
}
