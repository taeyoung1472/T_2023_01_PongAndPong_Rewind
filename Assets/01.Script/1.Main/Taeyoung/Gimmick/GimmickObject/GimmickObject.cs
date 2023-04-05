using UnityEngine;

public abstract class GimmickObject : MonoBehaviour
{
    public abstract void Init();
    public virtual void InitOnRewind() { }
    public virtual void InitOnPlay() { }
}
