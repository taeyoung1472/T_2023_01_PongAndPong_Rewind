using UnityEngine;
using UnityEngine.UI;


//�� ��ũ��Ʈ�� ������ Ŀ���� ���� ���� ������ ������
public class ScaleRewind : RewindAbstract           
{
    [SerializeField] Slider scaleSlider;

    CircularBuffer<Vector3> trackedObjectScales;       //�����͸� �����Ϸ��� �� CircularBuffer Ŭ������ ����ض�

    private void Start()
    {
        //��ȯ ���۴� start �޼��忡�� �ʱ�ȭ�ؾ� �ϸ� �ʵ� �ʱ�ȭ�� ����� �� ����
        trackedObjectScales = new CircularBuffer<Vector3>();        
    }

    //�� �޼ҵ忡�� ������ ����� ����.
    //�ϴ� ����� ���� �߰� ���� ������ ������ Ʈ��ŷ�Ϸ��� �մϴ�.
    protected override void Track()
    {
        TrackObjectScale();      
    }


    //�� �Լ������� �ǰ��� �ð��� �ǵ��� �׸��� ����
    //���⼱ �ϴ� ��ü ũ�⸦ �ǵ������� ��
    protected override void Rewind(float seconds)
    {
        RestoreObjectScale(seconds);
    }


    // Ŀ���� ���� ������ ��
    public void TrackObjectScale()
    {
        trackedObjectScales.WriteLastValue(transform.localScale);
    }


    // Ŀ���� ���� �ǵѸ����� ��
    public void RestoreObjectScale(float seconds)
    {
        transform.localScale = trackedObjectScales.ReadFromBuffer(seconds);

        //�� ���� ��ü �����ϰ� ��ġ�ϵ��� �����̴� ���� �߰��� �ǵ��� ���� ����
        scaleSlider.value = transform.localScale.x;
    }

    //���� ��ü ������ ������ ���õ� ��쿡�� �ϴ� ���� ���� ��
    //�׷��� ������ �����̴� ���� �����ϰ� �ǵ����� ���� �� ���� �� ����
    //�׷��� �׿� ���� ��ü ������ ������Ʈ ��
}
