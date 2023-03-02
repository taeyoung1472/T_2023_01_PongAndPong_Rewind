using UnityEngine;

public class PointInTime {
	//위치 및 회전 저장
	public Vector3 position;
	public Quaternion rotation;

	//생성자
	public PointInTime(Vector3 _position, Quaternion _rotation){
		position = _position;
		rotation = _rotation;
	}
}
