using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    Vector3 pos, dir;

    GameObject laserObj;

    public LineRenderer laser;
    List<Vector3> laserIndices = new List<Vector3>();

    public ShootLaser Shoot;
    //public LaserBeam(Vector3 pos, Vector3 dir, Material mat)
    //{
    //    this.laser = new LineRenderer();
    //    this.laserObj = new GameObject();
    //    this.laserObj.name = "Laser Beam";
    //    this.pos = pos;
    //    this.dir = dir;

    //    this.laser = this.laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
    //    this.laser.startWidth = 0.1f;
    //    this.laser.endWidth = 0.1f;
    //    this.laser.material = mat;
    //    this.laser.startColor = Color.green;
    //    this.laser.endColor = Color.green;

    //    CastRay(pos, dir, laser);
    //}
    private void Awake()
    {
        Shoot = FindObjectOfType<ShootLaser>();
        transform.position = Shoot.transform.position;
    }
    private void Start  ()
    {
        CastRay(Shoot.transform.position, transform.right, laser);
        //Debug.Log(laser.gameObject.name);
    }
    void CastRay(Vector3 pos, Vector3 dir, LineRenderer laser)
    {
        laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30, 1 << LayerMask.NameToLayer("LaserCurveGimmick")))
        {
            Debug.Log("¹Ì·¯");
            CheckHit(hit, dir, laser);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(30));

            UpdateLaser();
        }
    }

    void UpdateLaser()
    {
        int cnt = 0;
        laser.positionCount = laserIndices.Count;

        foreach (Vector3 idx in laserIndices)
        {
            laser.SetPosition(cnt, idx);
            cnt++;
        }
    }

    void CheckHit(RaycastHit hitInfo, Vector3 direction, LineRenderer laser)
    {
        if (hitInfo.collider.CompareTag("ReflectGimmick"))
        {
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);

            CastRay(pos, dir, laser);
        }
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLaser();
        }
    }
}
