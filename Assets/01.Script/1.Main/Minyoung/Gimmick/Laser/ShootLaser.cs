using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{
    public Material mat;
    public LaserBeam beam;

    private LaserCurveInteract laserCurveInteract;
    private void Awake()
    {
        laserCurveInteract = FindObjectOfType<LaserCurveInteract>();
    }
    private void Start()
    {
       // SetLine();
    }
    void Update()
    {
       //SetLine();
    }

    public void SetLine()
    {
        Destroy(GameObject.Find("Laser Beam"));
        GameObject obj = Instantiate(beam.gameObject);
        obj.transform.position = Vector3.zero;
        Debug.Log(obj.transform.position);
        obj.name = "Laser Beam";


        //Destroy(GameObject.Find("Laser Beam"));
        // beam = new LaserBeam(transform.position, transform.right, mat);
    }
}
