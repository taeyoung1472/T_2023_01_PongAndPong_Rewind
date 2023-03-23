using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// �����̴� �Է����� �ð��� �ǰ��� ����� ��
/// </summary>
public class RewindBySlider : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    //���� TimeManager �ð� �ܰ谡 Time.FixedDeltaTime���� �����ǰ� �����̴��� �ִϸ����͵� ������ �ð� �ܰ�� ����

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
            rewindManager.StopRewindTimeBySeconds();                    //�ǰ��Ⱑ �Ϸ�Ǹ� �ùٸ��� �����ض�
            RestoreSliderAnimation();
            rewindSound.Stop();
        }
    }
    public void OnSliderDown()
    {
        if (slider.interactable)
        {
            rewindManager.StartRewindTimeBySeconds(-slider.value);       //�ǰ��� �̸����⸦ ���� (�����̴��� ���� ���̹Ƿ� ���� ��ȣ�� ����)                                               
            SliderAnimationPause();
            rewindSound.Play();
        }
    }
    public void OnSliderUpdate(float value)
    {
        rewindManager.SetTimeSecondsInRewind(-value);                    //�����̴� ���� ����Ǹ� �ǰ��� �̸����� ���¸� ������(�����̴��� ���� ���� �����Ƿ� ���� ��ȣ�� ���޵�).

    }
    public void SliderAnimationPause()                                  //�ǰ��� �����̴� �ִϸ����Ͱ� �Ͻ� ������ ���
    {
        sliderAnimator.SetFloat("TimeRewindSpeed", 0);
    }
    public void RestoreSliderAnimation()                                //�����̴� �������� ��� �� �����ϸ� �ùٸ� ������ �ǵ��ư�
    {
        float animationTimeStartFrom = (slider.value - slider.minValue) / RewindManager.Instance.howManySecondsToTrack;
        sliderAnimator.Play("AutoResizeAnim", 0, animationTimeStartFrom);
        sliderAnimator.SetFloat("TimeRewindSpeed", 1);
        StartCoroutine(ResetSliderValue());
    }
    //���� �����̴� �ִϸ����Ͱ� ���� ������Ʈ ��.
    IEnumerator ResetSliderValue()
    {
        yield return new WaitForFixedUpdate();
        slider.value = 0;
    }
}
