using UnityEngine;

public abstract class RecordObject : MonoBehaviour    
{
    protected bool isRewinding { get { return RewindManager.Instance.IsRewinding; } }
    protected bool isEnd { get { return RewindManager.Instance.IsEnd; } }
    protected float recordingPercent { get { return RewindManager.Instance.CurRecordingPercent; } }

    public abstract void InitOnRewind();
    public abstract void Register();
    public abstract void DeRegister();
    public abstract void Recorde(int index);
    public abstract void ApplyData(int index);
}