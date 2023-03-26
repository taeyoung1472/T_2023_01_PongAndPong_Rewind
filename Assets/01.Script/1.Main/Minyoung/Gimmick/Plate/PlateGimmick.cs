using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateGimmick : MonoBehaviour
{
    [SerializeField] private int cnt = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            cnt--;
            print(cnt);
            if (cnt <= 0)
            {
                print("사라짐");
                Destroy(gameObject);
            }
            //나중에 알아서 메쉬공부
        }
    }
}
