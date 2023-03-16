using UnityEngine;


//�� ��ũ��Ʈ�� ����� ���� ���� ����� �Բ� �⺻������ ������ ���� �ַ��(��: ���� ��ƼŬ, �����...)�� ������ ������
public class TimerRewind : RewindAbstract
{
    CircularBuffer<float> trackedTime;     //�����͸� �����Ϸ��� �� CircularBuffer Ŭ������ ����ض�.
    [SerializeField] TestTimer testTimer;
    
    [SerializeField] ParticlesSetting particleSettings;

    private void Start()
    {
        //��ȯ ���۴� start �޼��忡�� �ʱ�ȭ�ؾ� �ϸ� �ʵ� �ʱ�ȭ�� ����� �� ����
        trackedTime = new CircularBuffer<float>();  
        //����� ���� ���� ��ũ��Ʈ���� ��ƼŬ�� �����ϵ��� ������ �� ���� ���� �޼��忡�� �̷� ��ƼŬ�� �ʱ�ȭ�ؾ� ��
        InitializeParticles(particleSettings);      
    }


    //�� �޼ҵ忡�� ������ ����� ������.
    //�ϴ� ������ �̹� ������ ����� ����, ��ƼŬ ���� + ���ο� ����� ���� Ÿ�̸� ������ �����Ϸ��� ��
    protected override void Track()
    {
        Debug.Log("��� ��");
        //TrackParticles();
        //TrackAudio();
        TrackTimer();
    }

    //�� �޼ҵ忡���� �ǰ��� �ð��� �����ε��� �׸��� ������.
    //�ϴ� ��ƼŬ, ����� �� ����� ���� ���� Ÿ�̸Ӹ� �����ε� �Ϸ��� ��
    protected override void Rewind(float seconds)
    {
        Debug.Log("�ǰ��� ��");
        //RestoreParticles(seconds);
        //RestoreAudio(seconds);
        RestoreTimer(seconds);
    }


    // Ŀ���� ���� ������ ��
    public void TrackTimer()
    {
        trackedTime.WriteLastValue(testTimer.CurrentTimer);
    }


    //Ŀ���� ���� �ǵѸ����� ��
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
