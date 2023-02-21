using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBackgroundMover : MonoBehaviour
{
    [SerializeField] private Transform background_1;
    [SerializeField] private Transform background_2;

    [SerializeField] private float speed;
    [SerializeField] private float limitPos;
    [SerializeField] private float size;

    void Update()
    {
        background_1.Translate(Vector3.up * speed * Time.deltaTime);
        background_2.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if(background_1.transform.position.y > limitPos)
        {
            background_1.transform.position = background_2.transform.position + new Vector3(0, -size, 0);
        }
        if (background_2.transform.position.y > limitPos)
        {
            background_2.transform.position = background_1.transform.position + new Vector3(0, -size, 0);
        }
    }
}
