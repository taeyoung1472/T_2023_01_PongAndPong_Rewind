using UnityEngine;

public class ReTimeParticles : MonoBehaviour {
	private ParticleSystem Particles;
	[HideInInspector]
	public float PassedTime;
	private ReTime CoreReTime;
	private float PlaybackTime;
	private float LoopingTime;
	private int PSLoops;
	float t = 0f;
	float MaxTime = 0f;
	bool stop = false;
	bool init = true;
	[Tooltip("오직 루핑 파티클 시스템에만 해당됩니다." +
		" 각 루프 후 재설정 지점으로 사용될 부동 소수점 수를 입력하삼. " +
		"자연스러운 시각적 결과를 얻으려면 시행 착오가 필요하며 완벽한 소수점에 도달할 때까지 실험해야됨")]
	public float ResettingPoint;

	private void Start() 
	{
		Particles = GetComponent<ParticleSystem> ();
		PassedTime = 0.0f;
		LoopingTime = 0.0f;
		PSLoops = 0;
	}
		
	
	private void Update() 
	{
		if (!CoreReTime)
			CoreReTime = GetComponent<ReTime> ();

		//PS가 반복되면 LOOP TRACKER를 트리거합니다
		if (Particles.main.loop)
			ParticleSystemLoopTracker ();

		//방출을 중지하고 되감지 않으면 델타 시간을 계산
		if (!CoreReTime.isRewinding) {
			init = true;
			//파티클 시스템이 죽은 경우 계산 시작
			if (!Particles.IsAlive ())
				PassedTime += Time.deltaTime;

			//파티클 시스템이 반복되는 경우 측면을 따라 계산
			if (Particles.main.loop)
				LoopingTime += Time.deltaTime;
			
		} else {
			//되감는 경우 현재 델타 시간에서 경과된 델타 시간을 뺀 값
			PlaybackTime = Particles.time;
			PassedTime -= Time.deltaTime;
			Particles.Stop ();
			init = false;

			//PS가 루프에 있는지 여부
			if (Particles.main.loop) {
				if (PSLoops > 0) {
					if ( PlaybackTime <= ResettingPoint) {
						PlaybackTime = MaxTime;
						PSLoops--;
					}
				}
				Particles.Simulate (PlaybackTime - Time.deltaTime);
			} else {
				//경과 시간이 0일 때 시뮬레이션을 반대로 재생함
				if (PassedTime <= 0.0f)
					Particles.Simulate (PlaybackTime - Time.deltaTime);
			}
		}
	}

	private void ParticleSystemLoopTracker()
	{
		float time = Particles.time;

		if (init) {
			t = Particles.main.duration;

			//PS 시간 재설정으로 인해 t가 시간보다 크고 중지 플래그가 꺼지면 이것이 MAX TIME임
			if (t - time <= 0.1f) {
				if (!stop) {
					MaxTime = t;
					PSLoops++;
					stop = true;
				}
			} else {
				stop = false;
			}
		}
	}
}
