using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TransformRecord
{
    public override void OnUpdate()
    {
        Vector2 input;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        transform.Translate(10 * input.x * Time.deltaTime * Vector3.right);
    }
}
