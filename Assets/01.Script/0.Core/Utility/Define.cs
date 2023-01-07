using UnityEngine;
using Object = UnityEngine.Object;

public static class Define
{
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