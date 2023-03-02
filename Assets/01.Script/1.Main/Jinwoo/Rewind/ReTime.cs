using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReTime : MonoBehaviour {
	//되감기 활성화 또는 비활성화 플래그 지정
	[HideInInspector]
	public bool isRewinding = false;

	//이전 위치 및 회전에 액세스하는 성능 향상을 위해 연결 목록 데이터 구조를 사용
	private LinkedList<PointInTime> PointsInTime;

	//되감기를 트리거하는 키
	[Tooltip("되감기를 실행할 키의 문자 또는 이름을 입력")]
	public string KeyTrigger;

	[HideInInspector]
	public bool UseInputTrigger;
	private bool hasAnimator = false;
	private bool hasRb = false;
	public Animator animator;

	[HideInInspector]
	public float RewindSeconds = 5;

	[HideInInspector]
	public float RewindSpeed = 1;
	private bool isFeeding = true;
	private ParticleSystem Particles;

	[HideInInspector]
	public bool PauseEnd;

	private void Start()
	{
		Init();
	}
	// 초기화에 사용
	public void Init()
    {
		PointsInTime = new LinkedList<PointInTime>();

		//파티클 시스템이 포함된 경우 구성 요소를 캐시하고 추가.
		if (GetComponent<ParticleSystem>())
			Particles = GetComponent<ParticleSystem>();

		//키보드 입력을 사용하려는 경우 키 결과를 캐시하고 소문자로 변환하여 오류를 방지합니다.
		if (UseInputTrigger)
			KeyTrigger = KeyTrigger.ToLower(); //  ToLower가 소문자로 바꾸는 거임

		//애니메이터가 있는 경우 캐시
		if (GetComponent<Animator>())
		{
			hasAnimator = true;
			animator = GetComponent<Animator>();
		}

		//rigidbody가 있는지 없는지
		if (GetComponent<Rigidbody>())
			hasRb = true;

		//모든 하위 항목에 시간 되감기 스크립트 추가 - Bubbling
		foreach (Transform child in transform)
		{
			child.gameObject.AddComponent<ReTime>();
			child.GetComponent<ReTime>().UseInputTrigger = UseInputTrigger;
			child.GetComponent<ReTime>().KeyTrigger = KeyTrigger;
			child.GetComponent<ReTime>().RewindSeconds = RewindSeconds;
			child.GetComponent<ReTime>().RewindSpeed = RewindSpeed;
			child.GetComponent<ReTime>().PauseEnd = PauseEnd;
		}
	}
	private void Update() 
	{
		//특정 입력이 트리거되면 되감기 시작 그렇지 않으면 중지
		if (UseInputTrigger) 
			if (Input.GetKey (KeyTrigger))
				StartRewind();
			else
				StopRewind();
	}

	private void FixedUpdate()
	{
		ChangeTimeScale (RewindSpeed);
		//true이면 되감기를 실행하고, 그렇지 않으면 이벤트를 기록
		if (isRewinding) {
			Rewind();
		}else{
			Time.timeScale = 1f;
			if(isFeeding)
				Record();
		}
	}

	//되감기 메소드
	private void Rewind()
	{
		if (PointsInTime.Count > 0 ) {
			PointInTime PointInTime = PointsInTime.First.Value;
			transform.position = PointInTime.position;
			transform.rotation = PointInTime.rotation;
			PointsInTime.RemoveFirst();
		} else {
			if(PauseEnd)
				Time.timeScale = 0;
			else
				StopRewind();
		}
	}

	//생성자를 사용하여 새 데이터 추가
	private void Record()
	{
		if(PointsInTime.Count > Mathf.Round(RewindSeconds / Time.fixedDeltaTime)){
			PointsInTime.RemoveLast();
		}
		PointsInTime.AddFirst (new PointInTime (transform.position, transform.rotation));
		if (Particles)
		if (Particles.isPaused) {
			Particles.Play();
		}
	}

	private void StartRewind()
	{
		isRewinding = true;
		if(hasAnimator)
			animator.enabled = false;

		if (hasRb)
			GetComponent<Rigidbody>().isKinematic = true;
	}

	private void StopRewind()
	{
		Time.timeScale = 1;
		isRewinding = false;
		if(hasAnimator)
			animator.enabled = true;

		if (hasRb)
			GetComponent<Rigidbody>().isKinematic = false;
	}
		
	private void ChangeTimeScale(float speed)
	{
		Time.timeScale = speed;
		//타임 스케일에 따라 픽스드델타타임도 모시깽 해줘야 됨. ...아마두?
		if (speed > 1)
			Time.fixedDeltaTime = 0.02f / speed;
		else
			Time.fixedDeltaTime = speed * 0.02f;
	}

	//되감기를 활성화하는 노출된 메서드
	public void StartTimeRewind()
	{
		isRewinding = true;

		if(hasAnimator)
			animator.enabled = false;

		if (hasRb)
			GetComponent<Rigidbody>().isKinematic = true;
		
		if(transform.childCount > 0){
			foreach (Transform child in transform)
				child.GetComponent<ReTime>().StartRewind ();
		}
	}

	//되감기를 비활성화하는 노출된 메서드
	public void StopTimeRewind(){
		isRewinding = false;
		Time.timeScale = 1;
		if(hasAnimator)
			animator.enabled = true;

		if (hasRb)
			GetComponent<Rigidbody>().isKinematic = false;

		if(transform.childCount > 0){
			foreach (Transform child in transform) {
				child.GetComponent<ReTime>().StopTimeRewind ();
			}
		}
	}

	//상위 개체의 체크포인트 끝
	public void StopFeeding(){
		isFeeding = false;

		if(transform.childCount > 0){
			foreach (Transform child in transform) {
				child.GetComponent<ReTime>().StopFeeding ();
			}
		}
	}

	//상위 개체에 대한 체크 포인트 시작
	public void StartFeeding(){
		isFeeding = true;

		if(transform.childCount > 0){
			foreach (Transform child in transform) {
				child.GetComponent<ReTime>().StartFeeding ();
			}
		}
	}

	//ReTime 컴포넌트를 추가할 때 파티클 시스템인 모든 개체에 파티클 되감기 스크립트도 추가
	private void Reset(){
		if(GetComponent<ParticleSystem>())
			gameObject.AddComponent<ReTimeParticles>();

		// 파티클인 모든 자식에 파티클 되감기 스크립트를 추가.- Bubbling
		foreach (Transform child in transform) {
			if(child.GetComponent<ParticleSystem>())
				child.gameObject.AddComponent<ReTimeParticles>();
		}
	}
}
