using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlateGimmick : GimmickObject
{
    [SerializeField] private float destroyTime = 0f;
    private float basicTime;
    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        basicTime = destroyTime;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            destroyTime -= Time.deltaTime;
            if (destroyTime <= 0)
            {
                Debug.Log("»èÁ¦");
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        destroyTime = basicTime;
    }
}
