using UnityEngine;

/// <summary>
/// Ű �������� �ð��� �ǰ��� ����� ��
/// </summary>
public class RewindByKeyPress : MonoBehaviour
{
    bool isRewinding = false;
    [SerializeField] float rewindIntensity = 0.01f;          //�ǰ��� �ӵ��� �����ϴ� ����
    //[SerializeField] RewindTestManager rewindManager;
    float rewindValue = 0;

    private void Start()
    {
        //RewindManager.Instance.StartAreaPlay();
    }
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Y))                     //���ϴ� Ű�� Ű�ڵ�� �����ϻ�
        {
            rewindValue += rewindIntensity;                 //��ư�� ���� ä ���� �� ���ŷ� �ð��� �ǵ���

            if (!isRewinding)
            {
                RewindManager.Instance.StartRewindTimeBySeconds(rewindValue);
            }
            else
            {
                if(RewindManager.Instance.HowManySecondsAvailableForRewind>rewindValue)      //������ ��� ���� �������� �ʵ��� ���� Ȯ��
                    RewindManager.Instance.SetTimeSecondsInRewind(rewindValue);
            }
            isRewinding = true;
        }
        else
        {
            if(isRewinding)
            {
                RewindManager.Instance.StopRewindTimeBySeconds();
                rewindValue = 0;
                isRewinding = false;
            }
        }
    }
}
