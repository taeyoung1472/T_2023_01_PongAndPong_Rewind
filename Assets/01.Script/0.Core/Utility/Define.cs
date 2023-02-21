using Cinemachine;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Define
{
    private static Player _player = null;
    public static Player player { get { return SearchByClass<Player>(ref _player); } }
    private static Transform playerTrm;
    private static Camera mainCam;
    public static Transform PlayerTrm { get { return SearchByName<Transform>(ref playerTrm, "Player"); } }
    public static Camera MainCam { get { return SearchByClass<Camera>(ref mainCam); } }

    private static T SearchByName<T>(ref T t, string s) where T : Object
    {
        if (t == null)
        {
            t = Utility.SearchByName<T>(s);
        }

        return t;
    }
    private static T SearchByClass<T>(ref T t) where T : Object
    {
        if (t == null)
        {
            t = Utility.SearchByClass<T>();
        }

        return t;
    }

    public static Vector3 GetVecDir(VectorDir dir)
    {
        return dir switch
        {
            VectorDir.Up => Vector3.up,
            VectorDir.Down => Vector3.down,
            VectorDir.Right => Vector3.right,
            VectorDir.Left => Vector3.left,
            VectorDir.Foward => Vector3.forward,
            VectorDir.Back => Vector3.back,
            _ => Vector3.zero,
        };
    }

    private static Camera _cam = null;
    public static Camera Cam
    {
        get
        {
            if (_cam == null)
                _cam = Camera.main;
            return _cam;
        }
    }

    private static CinemachineVirtualCamera _vCamOne = null;
    private static CinemachineVirtualCamera _vCamTwo = null;
    private static CinemachineVirtualCamera _cartCam = null;

    public static CinemachineVirtualCamera VCamOne
    {
        get
        {
            if (_vCamOne == null)
                _vCamOne = GameObject.FindObjectOfType<CameraManager>().transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            return _vCamOne;
        }
    }
    public static CinemachineVirtualCamera VCamTwo
    {
        get
        {
            if (_vCamTwo == null)
                _vCamTwo = GameObject.FindObjectOfType<CameraManager>().transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
            return _vCamTwo;
        }
    }
    public static CinemachineVirtualCamera CartCam
    {
        get
        {
            if (_cartCam == null)
                _cartCam = GameObject.FindObjectOfType<CameraManager>().transform.GetChild(2).GetComponent<CinemachineVirtualCamera>();
            return _cartCam;
        }
    }
}
#region Vector
public enum VectorDir
{
    Up,
    Down,
    Right,
    Left,
    Foward,
    Back,
}
#endregion