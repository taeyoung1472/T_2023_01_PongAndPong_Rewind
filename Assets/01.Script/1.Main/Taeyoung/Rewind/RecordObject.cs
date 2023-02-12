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

    // 초기화
    public virtual void Init() { }

    // 본인 흐름의 실행되는 업데이트
    public virtual void OnUpdate() { }

    // 본인 역 흐름때 실행되는 업데이트
    public virtual void OnRewindUpdate() { }

    // 본인 흐름으로 바뀔때 실행
    public abstract void InitOnPlay();

    // 본인 흐름 역으로 바뀔때 실행
    public abstract void InitOnRewind();

    // 등록
    public abstract void Register();

    // 등록 해지
    public abstract void DeRegister();

    // 정보 저장
    public abstract void Recorde(int index);

    // 정보 불러오기
    public abstract void ApplyData(int index, int nextIndexDiff);

    protected void GenerateList<T>(ref List<T> list, T initValue)
    {
        int totalCount = RewindManager.Instance.TotalRecordCount;

        list = new(totalCount);
        list.AddRange(new T[totalCount]);
        list[0] = initValue;
    }
}