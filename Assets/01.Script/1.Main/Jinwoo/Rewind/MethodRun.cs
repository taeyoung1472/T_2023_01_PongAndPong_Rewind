using UnityEngine;

public class MethodRun : MonoBehaviour {
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.G)){
			GetComponent<ReTime> ().StartTimeRewind ();
		}

		if(Input.GetKeyDown(KeyCode.Return)){
			GetComponent<ReTime> ().StopTimeRewind ();
		}
	}
}
