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
}