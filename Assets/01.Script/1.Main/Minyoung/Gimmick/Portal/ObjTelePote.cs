using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTelePote : MonoBehaviour
{
    private Collider _col;
       
    public bool isOverlapping = false;

    public Transform reciever;

    public Transform objTrm;

    public List<Transform> telObjList;
    private void Awake()
    {
        _col = GetComponent<Collider>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            objTrm.position += new Vector3(-1, 0, 0);
            foreach (Transform trm in telObjList)
            {
                trm.position += new Vector3(-1, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            objTrm.position += new Vector3(1, 0, 0);

            foreach (Transform trm in telObjList)
            {
                trm.position += new Vector3(1, 0, 0);
            }
        }


        if (isOverlapping)
        {
            Vector3 centerPos = _col.bounds.center;

            foreach (Transform trm in telObjList)
            {
                Vector3 diffVec = centerPos - trm.position;

                //trm.position = reciever.position;
                trm.position = reciever.position - diffVec;
            }
           

            //objTrm.position = reciever.position;
            isOverlapping = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TelObj"))
        {
            telObjList.Add(other.gameObject.transform);
            isOverlapping = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TelObj"))
        {
            telObjList.Remove(other.gameObject.transform);
            isOverlapping = false;
        }
    }

}
