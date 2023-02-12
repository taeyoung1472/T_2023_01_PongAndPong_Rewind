using System.Collections.Generic;
using UnityEngine;

public abstract class RecordObject : MonoBehaviour
{
    protected float RecordingPercent { get { return RewindManager.Instance.CurRecordingPercent; } }

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
    [SerializeField] private bool isRewind = false;
    public bool IsRewind { get { return isRewind; } }

    // �ʱ�ȭ
    public virtual void Init() { }

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

    protected void GenerateList<T>(ref List<T> list, T initValue)
    {
        int totalCount = RewindManager.Instance.TotalRecordCount;

        list = new(totalCount);
        list.AddRange(new T[totalCount]);
        list[0] = initValue;
    }
}