using System;
using UnityEngine;
using UnityEngine.Events;

public class RewindTestManager : MonoSingleTon<RewindTestManager>
{
    public Action<float> RewindTimeCall { get; set; }
    public Action<bool> TrackingStateCall { get; set; }
    public Action<float> RestoreBuffers { get; set; }

    /// <summary>
    /// �����ε� �ɶ� �� �� ȣ��
    /// </summary>
    public Action InitRewind { get; set; }
    /// <summary>
    /// ���� �ɶ� �� �� ȣ��
    /// </summary>
    public Action InitPlay { get; set; }

    /// <summary>
    /// ���ű��� �󸶳� �����ؾ� �ϴ����� �����ϴ� ����, ������ �Ѱ迡 ������ �� ��ȯ ���ۿ��� ���� ���� ���
    /// </summary>
    public float howManySecondsToTrack = 10;

    /// <summary>
    /// �� ������ �ǰ��⿡ ����� �� �ִ� �ð�(��)�� ��ȯ�� (�״ϱ� �ð��� �������� Ŀ���� ����)
    /// </summary>
    public float HowManySecondsAvailableForRewind { get; private set; }


    /// <summary>
    /// ���� ���� �ǰ��� ������ �˷���
    /// </summary>
    public bool IsBeingRewinded { get; private set; } = false;


    private float rewindSeconds = 0;

    //private void Start()
    //{
    //    //HowManySecondsAvailableForRewind = 0;
        
    //    //Debug.Log("InitPlay");
    //}


    /// <summary>
    /// �� �޼��带 ȣ���Ͽ� ������ �̸����� ���� ��� ������ �ʸ�ŭ �ð��� �ǰ���
    /// </summary>
    /// <param name="seconds">��ü�� ���ݺ��� �ǰ��ƾ� �ϴ� �ð�(��)�� �����ϴ� �Ű�����(�Ű������� 0���� ũ�ų� ���ƾ� ��).</param>
    public void RewindTimeBySeconds(float seconds)
    {
        if(seconds>HowManySecondsAvailableForRewind)
        {
            Debug.LogError("����� ���� ���� ������� ����!!! �߸��� ���ο� ����. ȣ��� �ǰ���� HowManySecondsAvailableForRewind �Ӽ����� �۾ƾ� ��.");
            return;
        }
        if(seconds<0)
        {
            Debug.LogError("RewindTimeBySeconds()�� �Ű������� ��� ���̾�� ��!!!");
            return;
        }
        InitRewind?.Invoke();
        TrackingStateCall?.Invoke(false);

        RewindTimeCall?.Invoke(seconds);
        RestoreBuffers?.Invoke(seconds);

        TrackingStateCall?.Invoke(true);
        IsBeingRewinded = true;
    }
    /// <summary>
    /// �������� �̸� �� �� �ִ� ������� �ð� �ǰ��⸦ �����Ϸ��� �� �޼��带 ȣ���ض�. 
    /// �ǰ��Ⱑ ������ StopRewindTimeBySeconds()�� ȣ���ؾ� ��. 
    /// ���̿� ������ �̸����⸦ ������Ʈ�Ϸ��� SetTimeSecondsInRewind() �޼��带 ȣ��
    /// </summary>
    /// <param name="seconds">�ǰ��� �̸����⸦ �ǰ��� �� �� ���� �����ϴ� �Ű�����(�Ű������� >=0 �̾�� ��)</param>
    /// <returns></returns>
    public void StartRewindTimeBySeconds(float seconds)
    {
        if (seconds > HowManySecondsAvailableForRewind)
        {
            Debug.LogError("����� ���� ���� ������� ����!!! �߸��� ���ο� ����. ȣ��� �ǰ���� HowManySecondsAvailableForRewind �Ӽ����� �۾ƾ� ��");
            return;
        }
        if (seconds < 0)
        {
            Debug.LogError("StartRewindTimeBySeconds()�� �Ű������� ��� ���̾�� ��!!!");
            return;
        }

        InitRewind?.Invoke();
        //ReTimeStart?.Invoke();
        rewindSeconds = seconds;
        TrackingStateCall?.Invoke(false);
        IsBeingRewinded = true;
    }
    private void FixedUpdate()
    {
        //Debug.Log(Time.time);
        if (IsBeingRewinded)
        {
            RewindTimeCall?.Invoke(rewindSeconds);
            //Debug.Log(rewindSeconds);
        }
        else if (HowManySecondsAvailableForRewind != howManySecondsToTrack)
        {
            HowManySecondsAvailableForRewind+=Time.fixedDeltaTime;
            
            if (HowManySecondsAvailableForRewind > howManySecondsToTrack)
                HowManySecondsAvailableForRewind = howManySecondsToTrack;
        }
    }

    /// <summary>
    /// �ǰ��Ⱑ Ȱ��ȭ�� ���� �ǰ��� �̸����⸦ ������Ʈ�Ϸ��� �� �޼��带 ȣ���ϻ�(StartRewindTimeBySeconds() �޼���� ������ ȣ���).
    /// </summary>
    /// <param name="seconds">�ǰ��� �̸����⸦ �� �ʷ� �̵��ؾ� �ϴ��� �����ϴ� �Ű�����(�Ű������� >=0�̾�� ��)</param>
    public void SetTimeSecondsInRewind(float seconds)
    {
        if (seconds > HowManySecondsAvailableForRewind)
        {
            Debug.LogError("����� ���� ���� ������� ����!!! �߸��� ���ο� ������. ȣ��� �ǰ���� HowManySecondsAvailableForRewind ���� �۾ƾ� ��.");
            return;
        }

        if (seconds < 0)
        {
            Debug.LogError("SetTimeSecondsInRewind()�� �Ű������� ��� ���̾�� ��!!!");
            return;
        }
        rewindSeconds = seconds;
    }
    /// <summary>
    /// �ǰ��� ���� �̸����⸦ �����ϰ� ���� ���� �ǰ��� ���·� ȿ�������� �����Ϸ��� �� �޼��带 ȣ���ϻ�
    /// </summary>
    public void StopRewindTimeBySeconds()
    {
        HowManySecondsAvailableForRewind -= rewindSeconds;
        IsBeingRewinded = false;

        RestoreBuffers?.Invoke(rewindSeconds);
        TrackingStateCall?.Invoke(true);

        //InitPlay?.Invoke();
        //ReTimeStop?.Invoke();

        Debug.Log("�����ε� ����");
    }

    public void StartAreaPlay()
    {
        HowManySecondsAvailableForRewind = 0;
        InitPlay?.Invoke();

    }
}
