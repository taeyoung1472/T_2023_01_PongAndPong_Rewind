using System;
using UnityEngine;

public class RewindTestManager : MonoBehaviour
{
    /// <summary>
    /// 이 액션은 사용자가 사용하기 위한 것이 아님 
    /// 클래스 간에 데이터를 공유함
    /// RewindTimeBySeconds(), StartRewindTimeBySeconds(),
    /// SetTimeSecondsInRewind(), StopRewindTimeBySeconds()와 같은 준비된 메서드를 사용하고 싶을 것 같아서 만듦
    /// </summary>
    public static Action<float> RewindTimeCall { get; set; }
    /// <summary>
    /// 이 액션은 사용자가 사용하기 위한 것이 아님 
    /// 클래스 간에 데이터를 공유함
    /// RewindTimeBySeconds(), StartRewindTimeBySeconds(),
    /// SetTimeSecondsInRewind(), StopRewindTimeBySeconds()와 같은 준비된 메서드를 사용하고 싶을 것 같아서 만듦
    /// </summary>
    public static Action<bool> TrackingStateCall { get; set; }
    /// <summary>
    /// 이 액션은 사용자가 사용하기 위한 것이 아님 
    /// 클래스 간에 데이터를 공유함
    /// RewindTimeBySeconds(), StartRewindTimeBySeconds(),
    /// SetTimeSecondsInRewind(), StopRewindTimeBySeconds()와 같은 준비된 메서드를 사용하고 싶을 것 같아서 만듦
    /// </summary>
    public static Action<float> RestoreBuffers { get; set; }


    /// <summary>
    /// 이 변수는 되감기에 사용할 수 있는 시간(초)을 반환함 (그니깐 시간이 지날수록 커지는 거임)
    /// </summary>
    public float HowManySecondsAvailableForRewind { get; private set; }


    /// <summary>
    /// 씬이 현재 되감기 중인지 알려줌
    /// </summary>
    public bool IsBeingRewinded { get; private set; } = false;


    private float rewindSeconds = 0;

    private void OnEnable()
    {
        HowManySecondsAvailableForRewind = 0;
    }
    private void Awake()
    {
        RewindManager[] managers= FindObjectsOfType<RewindManager>();

        //RewindManager를 사용하여 각 씬에 하나의 스크립트만 포함되어 있는지 확인
        if (managers.Length>1)                                               
        {
            Debug.LogError("RewindManager는 각 씬에서 두 번 이상 사용할 수 없습니다. 다른 RewindManager 제거 바람!!!");
        }
    }


    /// <summary>
    /// 과거까지 얼마나 추적해야 하는지를 정의하는 변수, 설정된 한계에 도달한 후 순환 버퍼에서 이전 값을 덮어씀
    /// </summary>
    public static float howManySecondsToTrack = 10;


    /// <summary>
    /// 이 메서드를 호출하여 스냅샷 미리보기 없이 즉시 지정된 초만큼 시간을 되감음
    /// </summary>
    /// <param name="seconds">개체를 지금부터 되감아야 하는 시간(초)을 정의하는 매개변수(매개변수는 0보다 크거나 같아야 함).</param>
    public void RewindTimeBySeconds(float seconds)
    {
        if(seconds>HowManySecondsAvailableForRewind)
        {
            Debug.LogError("저장된 추적 값이 충분하지 않음!!! 잘못된 색인에 도달. 호출된 되감기는 HowManySecondsAvailableForRewind 속성보다 작아야 함.");
            return;
        }
        if(seconds<0)
        {
            Debug.LogError("RewindTimeBySeconds()의 매개변수는 양수 값이어야 함!!!");
            return;
        }
        TrackingStateCall?.Invoke(false);
        RewindTimeCall?.Invoke(seconds);
        RestoreBuffers?.Invoke(seconds);
        TrackingStateCall?.Invoke(true);
    }
    /// <summary>
    /// 스냅샷을 미리 볼 수 있는 기능으로 시간 되감기를 시작하려면 이 메서드를 호출해라. 
    /// 되감기가 끝나면 StopRewindTimeBySeconds()를 호출해야 함. 
    /// 사이에 스냅샷 미리보기를 업데이트하려면 SetTimeSecondsInRewind() 메서드를 호출
    /// </summary>
    /// <param name="seconds">되감기 미리보기를 되감기 몇 초 전에 정의하는 매개변수(매개변수는 >=0 이어야 함)</param>
    /// <returns></returns>
    public void StartRewindTimeBySeconds(float seconds)
    {
        if (seconds > HowManySecondsAvailableForRewind)
        {
            Debug.LogError("저장된 추적 값이 충분하지 않음!!! 잘못된 색인에 도달. 호출된 되감기는 HowManySecondsAvailableForRewind 속성보다 작아야 함");
            return;
        }
        if (seconds < 0)
        {
            Debug.LogError("StartRewindTimeBySeconds()의 매개변수는 양수 값이어야 함!!!");
            return;
        }

        rewindSeconds = seconds;
        TrackingStateCall?.Invoke(false);
        IsBeingRewinded = true;
    }
    private void FixedUpdate()
    {
        if (IsBeingRewinded)
        {
            RewindTimeCall?.Invoke(rewindSeconds);
        }
        else if (HowManySecondsAvailableForRewind != howManySecondsToTrack)
        {
            HowManySecondsAvailableForRewind+=Time.fixedDeltaTime;
            
            if (HowManySecondsAvailableForRewind > howManySecondsToTrack)
                HowManySecondsAvailableForRewind = howManySecondsToTrack;
        }
    }

    /// <summary>
    /// 되감기가 활성화된 동안 되감기 미리보기를 업데이트하려면 이 메서드를 호출하삼(StartRewindTimeBySeconds() 메서드는 이전에 호출됨).
    /// </summary>
    /// <param name="seconds">되감기 미리보기를 몇 초로 이동해야 하는지 정의하는 매개변수(매개변수는 >=0이어야 함)</param>
    public void SetTimeSecondsInRewind(float seconds)
    {
        if (seconds > HowManySecondsAvailableForRewind)
        {
            Debug.LogError("저장된 추적 값이 충분하지 않음!!! 잘못된 색인에 도달함. 호출된 되감기는 HowManySecondsAvailableForRewind 보다 작아야 함.");
            return;
        }

        if (seconds < 0)
        {
            Debug.LogError("SetTimeSecondsInRewind()의 매개변수는 양수 값이어야 함!!!");
            return;
        }
        rewindSeconds = seconds;
    }
    /// <summary>
    /// 되감기 상태 미리보기를 중지하고 현재 값을 되감기 상태로 효과적으로 설정하려면 이 메서드를 호출하삼
    /// </summary>
    public void StopRewindTimeBySeconds()
    {
        HowManySecondsAvailableForRewind -= rewindSeconds;
        IsBeingRewinded = false;
        RestoreBuffers?.Invoke(rewindSeconds);
        TrackingStateCall?.Invoke(true);
    }
}
