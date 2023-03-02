using UnityEngine;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(ReTime))]
public class TimeRewindCustomGUI : Editor {
	public override void OnInspectorGUI(){
		GUILayout.Box (Resources.Load("이이잉") as Texture, GUILayout.Width(375), GUILayout.Height(200));
		ReTime script = (ReTime)target;
		GUILayout.Label ("코드에서 [ StartTimeRewind() ] 및 [ StopTimeRewind() ]를 수동으로 수행할 수 있슴"); 

		//vars
		script.RewindSeconds = 
			EditorGUILayout.FloatField (
				new GUIContent ("Rewind Seconds",
				"되감기 시간(초)"), script.RewindSeconds);
		
		script.RewindSpeed = 
			EditorGUILayout.Slider(
				new GUIContent(
					"Rewind Speed",
					"되감기가 재생되는 속도 0.1 - 5.0"), script.RewindSpeed, 0.1f, 5f);
		
		script.UseInputTrigger = 
			EditorGUILayout.Toggle (
				new GUIContent(
					"Use Input Trigger",
					"이를 활성화하면 키 트리거를 사용할 수 있음. 비활성화된 경우 StartRewind() 및 StopRewind() 메서드를 사용할 수 있음"), script.UseInputTrigger);

		EditorGUI.BeginDisabledGroup (script.UseInputTrigger == false);
		script.KeyTrigger = EditorGUILayout.TextField (new GUIContent("Key Trigger", "트리거의 키 이름을 입력합니다."), script.KeyTrigger);
		EditorGUI.EndDisabledGroup ();

		script.PauseEnd = EditorGUILayout.Toggle (new GUIContent ("Pause on End", "이 기능을 활성화하면 되감기가 끝날 때 모든 것이 일시 중지됨"), script.PauseEnd);
	}
}
