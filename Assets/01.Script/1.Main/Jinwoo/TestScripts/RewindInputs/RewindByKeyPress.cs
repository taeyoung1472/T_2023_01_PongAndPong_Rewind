using UnityEngine;

/// <summary>
/// Ű �������� �ð��� �ǰ��� ����� ��
/// </summary>
public class RewindByKeyPress : MonoBehaviour
{
    bool isRewinding = false;
    [SerializeField] float rewindIntensity = 0.01f;          //�ǰ��� �ӵ��� �����ϴ� ����
    //[SerializeField] RewindTestManager rewindManager;
    [SerializeField] AudioSource rewindSound;
    float rewindValue = 0;


    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Y))                     //���ϴ� Ű�� Ű�ڵ�� �����ϻ�
        {
            rewindValue += rewindIntensity;                 //��ư�� ���� ä ���� �� ���ŷ� �ð��� �ǵ���

            if (!isRewinding)
            {
                RewindTestManager.Instance.StartRewindTimeBySeconds(rewindValue);
                rewindSound.Play();
            }
            else
            {
                if(RewindTestManager.Instance.HowManySecondsAvailableForRewind>rewindValue)      //������ ��� ���� �������� �ʵ��� ���� Ȯ��
                    RewindTestManager.Instance.SetTimeSecondsInRewind(rewindValue);
            }
            isRewinding = true;
        }
        else
        {
            if(isRewinding)
            {
                RewindTestManager.Instance.StopRewindTimeBySeconds();
                rewindSound.Stop();
                rewindValue = 0;
                isRewinding = false;
            }
        }
    }
}
