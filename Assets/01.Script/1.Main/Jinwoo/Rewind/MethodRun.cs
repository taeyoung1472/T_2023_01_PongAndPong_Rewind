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

	public void RewindStart()
    {
		GetComponent<ReTime> ().StartTimeRewind ();

    }

	public void RewindStop()
	{

		GetComponent<ReTime>().StopTimeRewind();

	}
}
