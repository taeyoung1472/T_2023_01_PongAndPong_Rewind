using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 슬라이더 입력으로 시간을 되감는 방법의 예
/// </summary>
public class RewindBySlider : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    //원인 TimeManager 시간 단계가 Time.FixedDeltaTime으로 설정되고 슬라이더의 애니메이터도 동일한 시간 단계로 설정

    [SerializeField] Slider slider;
    [SerializeField] RewindManager rewindManager;
    [SerializeField] AudioSource rewindSound;
    Animator sliderAnimator;
    private int howManyFingersTouching = 0;


    void Start()
    {
        sliderAnimator = slider.GetComponent<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        howManyFingersTouching++;

        if (howManyFingersTouching == 1)
            OnSliderDown();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        howManyFingersTouching--;

        if (howManyFingersTouching == 0)
            OnSliderUp();
    }
    public void OnSliderUp()
    {
        if (slider.interactable)
        {
            rewindManager.StopRewindTimeBySeconds();                    //되감기가 완료되면 올바르게 중지해라
            RestoreSliderAnimation();
            rewindSound.Stop();
        }
    }
    public void OnSliderDown()
    {
        if (slider.interactable)
        {
            rewindManager.StartRewindTimeBySeconds(-slider.value);       //되감기 미리보기를 시작 (슬라이더는 음수 값이므로 빼기 기호로 전달)                                               
            SliderAnimationPause();
            rewindSound.Play();
        }
    }
    public void OnSliderUpdate(float value)
    {
        rewindManager.SetTimeSecondsInRewind(-value);                    //슬라이더 값이 변경되면 되감기 미리보기 상태를 변경함(슬라이더에 음수 값이 있으므로 빼기 기호로 전달됨).

    }
    public void SliderAnimationPause()                                  //되감기 슬라이더 애니메이터가 일시 중지된 경우
    {
        sliderAnimator.SetFloat("TimeRewindSpeed", 0);
    }
    public void RestoreSliderAnimation()                                //슬라이더 복원으로 사용 후 해제하면 올바른 값으로 되돌아감
    {
        float animationTimeStartFrom = (slider.value - slider.minValue) / RewindManager.Instance.howManySecondsToTrack;
        sliderAnimator.Play("AutoResizeAnim", 0, animationTimeStartFrom);
        sliderAnimator.SetFloat("TimeRewindSpeed", 1);
        StartCoroutine(ResetSliderValue());
    }
    //원인 슬라이더 애니메이터가 고정 업데이트 중.
    IEnumerator ResetSliderValue()
    {
        yield return new WaitForFixedUpdate();
        slider.value = 0;
    }
}
