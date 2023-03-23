using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        transform.position = target.position;
    }
}
