using UnityEngine;


//이 스크립트는 사용자 지정 변수 역행과 함께 기본적으로 구현된 역행 솔루션(예: 추적 파티클, 오디오...)의 설정을 보여줌
public class TimerRewind : RewindAbstract
{
    CircularBuffer<float> trackedTime;     //데이터를 저장하려면 이 CircularBuffer 클래스를 사용해라.
    [SerializeField] TestTimer testTimer;
    
    [SerializeField] ParticlesSetting particleSettings;

    private void Start()
    {
        //순환 버퍼는 start 메서드에서 초기화해야 하며 필드 초기화를 사용할 수 없음
        trackedTime = new CircularBuffer<float>();  
        //사용자 지정 추적 스크립트에서 파티클을 추적하도록 선택할 때 먼저 시작 메서드에서 이런 파티클를 초기화해야 함
        InitializeParticles(particleSettings);      
    }


    //이 메소드에서 추적할 대상을 정의함.
    //일단 지금은 이미 구현된 오디오 추적, 파티클 추적 + 새로운 사용자 지정 타이머 추적을 추적하려고 함
    protected override void Track()
    {
        Debug.Log("기록 중");
        //TrackParticles();
        //TrackAudio();
        TrackTimer();
    }

    //이 메소드에서는 되감기 시간에 리와인드할 항목을 정의함.
    //일단 파티클, 오디오 및 사용자 정의 구현 타이머를 리와인드 하려고 함
    protected override void Rewind(float seconds)
    {
        Debug.Log("되감기 중");
        //RestoreParticles(seconds);
        //RestoreAudio(seconds);
        RestoreTimer(seconds);
    }


    // 커스텀 변수 추적의 예
    public void TrackTimer()
    {
        trackedTime.WriteLastValue(testTimer.CurrentTimer);
    }


    //커스텀 변수 되둘리기의 예
    public void RestoreTimer(float seconds)
    {
        float rewindValue= trackedTime.ReadFromBuffer(seconds);
        //testTimer.CurrentTimer = rewindValue;
        //testTimer.SetText(rewindValue);
    }

    protected override void InitOnPlay()
    {
        //testTimer.timerDefault = RewindTestManager.Instance.howManySecondsToTrack;
        //testTimer.CurrentTimer = testTimer.timerDefault;
    }

    protected override void InitOnRewind()
    {

    }
}
