using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Test : GameObjectRecord
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        transform.position += Vector3.right * Time.deltaTime * 10;
    }
}
