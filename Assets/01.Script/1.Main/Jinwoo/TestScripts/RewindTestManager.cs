using System;
using UnityEngine;

public class RewindTestManager : MonoBehaviour
{
    /// <summary>
    /// �� �׼��� ����ڰ� ����ϱ� ���� ���� �ƴ� 
    /// Ŭ���� ���� �����͸� ������
    /// RewindTimeBySeconds(), StartRewindTimeBySeconds(),
    /// SetTimeSecondsInRewind(), StopRewindTimeBySeconds()�� ���� �غ�� �޼��带 ����ϰ� ���� �� ���Ƽ� ����
    /// </summary>
    public static Action<float> RewindTimeCall { get; set; }
    /// <summary>
    /// �� �׼��� ����ڰ� ����ϱ� ���� ���� �ƴ� 
    /// Ŭ���� ���� �����͸� ������
    /// RewindTimeBySeconds(), StartRewindTimeBySeconds(),
    /// SetTimeSecondsInRewind(), StopRewindTimeBySeconds()�� ���� �غ�� �޼��带 ����ϰ� ���� �� ���Ƽ� ����
    /// </summary>
    public static Action<bool> TrackingStateCall { get; set; }
    /// <summary>
    /// �� �׼��� ����ڰ� ����ϱ� ���� ���� �ƴ� 
    /// Ŭ���� ���� �����͸� ������
    /// RewindTimeBySeconds(), StartRewindTimeBySeconds(),
    /// SetTimeSecondsInRewind(), StopRewindTimeBySeconds()�� ���� �غ�� �޼��带 ����ϰ� ���� �� ���Ƽ� ����
    /// </summary>
    public static Action<float> RestoreBuffers { get; set; }


    /// <summary>
    /// �� ������ �ǰ��⿡ ����� �� �ִ� �ð�(��)�� ��ȯ�� (�״ϱ� �ð��� �������� Ŀ���� ����)
    /// </summary>
    public float HowManySecondsAvailableForRewind { get; private set; }


    /// <summary>
    /// ���� ���� �ǰ��� ������ �˷���
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

        //RewindManager�� ����Ͽ� �� ���� �ϳ��� ��ũ��Ʈ�� ���ԵǾ� �ִ��� Ȯ��
        if (managers.Length>1)                                               
        {
            Debug.LogError("RewindManager�� �� ������ �� �� �̻� ����� �� �����ϴ�. �ٸ� RewindManager ���� �ٶ�!!!");
        }
    }


    /// <summary>
    /// ���ű��� �󸶳� �����ؾ� �ϴ����� �����ϴ� ����, ������ �Ѱ迡 ������ �� ��ȯ ���ۿ��� ���� ���� ���
    /// </summary>
    public static float howManySecondsToTrack = 10;


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
        TrackingStateCall?.Invoke(false);
        RewindTimeCall?.Invoke(seconds);
        RestoreBuffers?.Invoke(seconds);
        TrackingStateCall?.Invoke(true);
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
    }
}
