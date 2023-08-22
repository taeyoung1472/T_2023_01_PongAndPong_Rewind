using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoSingleTon<A>
{
    public GameObject gim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnGimmick()
    {
        gim.SetActive(true);
    }
}
