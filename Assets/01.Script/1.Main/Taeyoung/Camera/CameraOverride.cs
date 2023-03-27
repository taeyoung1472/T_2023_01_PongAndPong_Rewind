using UnityEngine;

public class CameraOverride : MonoBehaviour
{
    [SerializeField] private Camera parentCam;
    Camera myCam;

    private void Start()
    {
        myCam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        myCam.fieldOfView = parentCam.fieldOfView;
    }
}
