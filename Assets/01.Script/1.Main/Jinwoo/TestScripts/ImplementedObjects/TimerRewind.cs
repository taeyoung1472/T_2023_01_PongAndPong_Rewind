using UnityEngine;


//�� ��ũ��Ʈ�� ����� ���� ���� ����� �Բ� �⺻������ ������ ���� �ַ��(��: ���� ��ƼŬ, �����...)�� ������ ������
public class TimerRewind : RewindAbstract
{
    CircularBuffer<float> trackedTime;     //�����͸� �����Ϸ��� �� CircularBuffer Ŭ������ ����ض�.
    [SerializeField] ParticleTimer particleTimer;
    
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
        TrackParticles();
        TrackAudio();
        TrackTimer();
    }

    //�� �޼ҵ忡���� �ǰ��� �ð��� �����ε��� �׸��� ������.
    //�ϴ� ��ƼŬ, ����� �� ����� ���� ���� Ÿ�̸Ӹ� �����ε� �Ϸ��� ��
    protected override void Rewind(float seconds)
    {
        RestoreParticles(seconds);
        RestoreAudio(seconds);
        RestoreTimer(seconds);
    }


    // Ŀ���� ���� ������ ��
    public void TrackTimer()
    {
        trackedTime.WriteLastValue(particleTimer.CurrentTimer);
    }


    //Ŀ���� ���� �ǵѸ����� ��
    public void RestoreTimer(float seconds)
    {
        float rewindValue= trackedTime.ReadFromBuffer(seconds);
        particleTimer.CurrentTimer = rewindValue;
        particleTimer.SetText(rewindValue);
    }

    protected override void InitOnPlay()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitOnRewind()
    {
        throw new System.NotImplementedException();
    }
}
