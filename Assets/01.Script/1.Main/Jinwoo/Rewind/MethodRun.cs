using UnityEngine;

public class MethodRun : MonoBehaviour {
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			GetComponent<ReTime> ().StartTimeRewind ();
		}

		if(Input.GetKeyDown(KeyCode.Return)){
			GetComponent<ReTime> ().StopTimeRewind ();
		}
	}
}
