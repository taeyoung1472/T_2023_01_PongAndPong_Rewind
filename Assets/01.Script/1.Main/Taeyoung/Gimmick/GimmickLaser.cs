using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GimmickLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform laserStartTrans;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        RaycastHit hit = new();
        Vector3 laserPos = laserStartTrans.position;
        Vector3 laserDir = laserStartTrans.forward;
        List<Vector3> positionList = new();

        positionList.Add(laserPos);
        int i = 0;
        while (true)
        {
            i++;
            if (i >= 50)
            {
                break;
            }

            if (Physics.Raycast(laserPos, laserDir, out hit) && hit.transform.TryGetComponent<LaserGimmickObjcet>(out LaserGimmickObjcet obj))
            {
                obj.LaserExcute();
                if (obj.IsLaserReflect == false)
                {
                    laserPos = hit.point;
                    positionList.Add(laserPos);
                    break;
                }
                laserPos = hit.point;
                laserDir = hit.normal;
                positionList.Add(laserPos);
                continue;
            }
            else
            {
                laserPos = hit.point;
                positionList.Add(laserPos);
                break;
            }
        }

        lineRenderer.positionCount = positionList.Count;
        lineRenderer.SetPositions(positionList.ToArray());
    }
}
