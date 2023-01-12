using UnityEngine;

public abstract class RecordObject : MonoBehaviour
{
    protected float RecordingPercent { get { return RewindManager.Instance.CurRecordingPercent; } }
    protected int TotalRecordCount { get { return RewindManager.Instance.TotalRecordCount; } }

    // ���� �帧�� ����Ǵ� ������Ʈ
    public virtual void OnUpdate() { }

    // ���� �� ������ ����Ǵ� ������Ʈ
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