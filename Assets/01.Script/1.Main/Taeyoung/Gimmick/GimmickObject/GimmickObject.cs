using UnityEngine;

public abstract class GimmickObject : MonoBehaviour
{
    public abstract void Init();
    public virtual void InitOnRewind() { }
    public virtual void InitOnPlay() { }

    public virtual void Awake()
    {
        if (RewindManager.Instance)
        {
            RewindManager.Instance.InitRewind += InitOnRewind;
            RewindManager.Instance.InitPlay += InitOnPlay;
        }
    }
}
