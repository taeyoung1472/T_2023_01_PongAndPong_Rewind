using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotVec;
    [SerializeField, Range(0.0f, 10.0f)] float moveYIntensity = 1;
    [SerializeField, Range(0.0f, 10.0f)] float moveYFrequency = 1;
    [SerializeField, Range(0.0f, 0.5f)] private float sizeIntensity = 0.2f;
    [SerializeField, Range(0.0f, 10.0f)] private float sizeFrequency = 1;

    private float originY;
    private float randSeed;
    private float time { get { return Time.time + randSeed; } }

    private void Awake()
    {
        randSeed = Random.Range(0.0f, Mathf.PI);
        originY = transform.position.y;
    }

    public void Update()
    {
        transform.Rotate(rotVec * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, originY + (moveYIntensity * Mathf.Sin(time * moveYIntensity)), transform.position.z);
        transform.localScale = Vector3.one - Vector3.one * (sizeIntensity * Mathf.Sin(time * sizeFrequency));
    }
}
