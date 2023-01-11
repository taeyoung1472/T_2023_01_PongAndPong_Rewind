using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : TransformRecord
{
    public override void OnUpdate()
    {
        transform.Rotate(Vector3.forward * 100 * Time.deltaTime);
    }
}
