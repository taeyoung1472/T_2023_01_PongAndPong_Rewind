using UnityEngine;

/// <summary>
/// 키 누름으로 시간을 되감는 방법의 예
/// </summary>
public class RewindByKeyPress : MonoBehaviour
{
    bool isRewinding = false;
    [SerializeField] float rewindIntensity = 0.02f;          //되감기 속도를 변경하는 변수
    [SerializeField] RewindTestManager rewindManager;
    [SerializeField] AudioSource rewindSound;
    float rewindValue = 0;

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Y))                     //원하는 키의 키코드로 변경하삼
        {
            rewindValue += rewindIntensity;                 //버튼을 누른 채 점점 더 과거로 시간을 되돌림

            if (!isRewinding)
            {
                rewindManager.StartRewindTimeBySeconds(rewindValue);
                rewindSound.Play();
            }
            else
            {
                if(rewindManager.HowManySecondsAvailableForRewind>rewindValue)      //범위를 벗어난 값을 가져오지 않도록 안전 확인
                    rewindManager.SetTimeSecondsInRewind(rewindValue);
            }
            isRewinding = true;
        }
        else
        {
            if(isRewinding)
            {
                rewindManager.StopRewindTimeBySeconds();
                rewindSound.Stop();
                rewindValue = 0;
                isRewinding = false;
            }
        }
    }
}
