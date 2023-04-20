using UnityEngine;

public class MapElement : MonoBehaviour
{
    public DeployType deployType;
    public Vector3 size;

//#if UNITY_EDITOR
//    private void OnDrawGizmos()
//    {
//        switch (deployType)
//        {
//            case DeployType.XY:
//                Gizmos.DrawWireCube(new Vector3(size.x / 2, size.y / 2, 0), new Vector3(size.x, size.y, 0));
//                break;
//            case DeployType.XZ:
//                Gizmos.DrawWireCube(new Vector3(size.x / 2, 0, 0), new Vector3(size.x, 0, size.z));
//                break;
//            case DeployType.YZ:
//                Gizmos.DrawWireCube(new Vector3(0, size.y / 2, 0), new Vector3(0, size.y, size.z));
//                break;
//        }
//    }
//#endif
}
