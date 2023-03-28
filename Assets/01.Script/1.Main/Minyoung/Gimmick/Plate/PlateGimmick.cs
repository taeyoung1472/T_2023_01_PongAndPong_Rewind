using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateGimmick : GimmickObject
{
    [SerializeField] private int cnt = 0;

    public override void Init()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            cnt--;
            if (cnt <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
 
}
