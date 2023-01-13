using UnityEngine;

public abstract class RecordObject : MonoBehaviour
{
    protected float RecordingPercent { get { return RewindManager.Instance.CurRecordingPercent; } }
    protected int InitIndex { get { if (RewindManager.Instance.IsRewinding) { return TotalRecordCount - 1; } else { return 0; } } }

    protected int TotalRecordCount 
    { 
        get 
        { 
            if(totalRecord == -1)
            {
                totalRecord = RewindManager.Instance.TotalRecordCount;
            }
            return totalRecord;
        } 
    }
    private int totalRecord = -1;

    // ���� �帧�� ����Ǵ� ������Ʈ
    public virtual void OnUpdate() { }

    // ���� �� �帧�� ����Ǵ� ������Ʈ
    public virtual void OnRewindUpdate() { }

    // ���� �帧���� �ٲ� ����
    public abstract void InitOnPlay();

    // ���� �帧 ������ �ٲ� ����
    public abstract void InitOnRewind();

    // ���
    public abstract void Register();

    // ��� ����
    public abstract void DeRegister();

    // ���� ����
    public abstract void Recorde(int index);

    // ���� �ҷ�����
    public abstract void ApplyData(int index, int nextIndexDiff);
}