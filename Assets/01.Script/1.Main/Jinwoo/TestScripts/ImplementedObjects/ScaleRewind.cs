using UnityEngine;
using UnityEngine.UI;


//이 스크립트는 간단한 커스텀 변수 추적 설정을 보여줌
public class ScaleRewind : RewindAbstract           
{
    [SerializeField] Slider scaleSlider;

    CircularBuffer<Vector3> trackedObjectScales;       //데이터를 저장하려면 이 CircularBuffer 클래스를 사용해라

    private void Start()
    {
        //순환 버퍼는 start 메서드에서 초기화해야 하며 필드 초기화를 사용할 수 없음
        trackedObjectScales = new CircularBuffer<Vector3>();        
    }

    //이 메소드에서 추적할 대상을 정의.
    //일단 사용자 지정 추가 변수 스케일 추적만 트래킹하려고 합니다.
    protected override void Track()
    {
        TrackObjectScale();      
    }


    //이 함수에서는 되감기 시간에 되돌릴 항목을 정의
    //여기선 일단 개체 크기를 되돌릴려고 함
    protected override void Rewind(float seconds)
    {
        RestoreObjectScale(seconds);
    }


    // 커스텀 변수 추적의 예
    public void TrackObjectScale()
    {
        trackedObjectScales.WriteLastValue(transform.localScale);
    }


    // 커스텀 변수 되둘리기의 예
    public void RestoreObjectScale(float seconds)
    {
        transform.localScale = trackedObjectScales.ReadFromBuffer(seconds);

        //그 동안 객체 스케일과 일치하도록 슬라이더 값을 추가로 되돌릴 수도 있음
        scaleSlider.value = transform.localScale.x;
    }

    //데모 개체 스케일 조정이 선택된 경우에만 하는 것이 좋을 듯
    //그렇지 않으면 슬라이더 값을 추적하고 되돌리는 것이 더 나을 것 같음
    //그러면 그에 따라 개체 배율도 업데이트 됨
}
