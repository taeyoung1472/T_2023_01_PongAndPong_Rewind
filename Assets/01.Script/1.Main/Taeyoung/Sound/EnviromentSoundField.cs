using UnityEngine;

public class EnviromentSoundField : MonoBehaviour
{
    [SerializeField] private Vector3 fieldSize;
    [SerializeField, Range(0.0f, 1.0f)] private float sizeFactor;
    [SerializeField] private EnviromentSound[] audioSourceArr;

    private Transform cameraTrans;

    public void Start()
    {
        cameraTrans = Camera.main.transform;
    }

    public void Update()
    {
        float dist = Mathf.Abs(transform.position.x - cameraTrans.position.x) * 2;

        float targetVolume = 0.0f;
        if (dist < fieldSize.x)
            targetVolume = 1.0f;
        else if (dist < fieldSize.x + fieldSize.x * sizeFactor)
            targetVolume = 1 - ((dist - fieldSize.x) / (fieldSize.x * sizeFactor));
        else
            targetVolume = 0.0f;

        foreach (var source in audioSourceArr)
        {
            source.SetVolume(targetVolume);
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, fieldSize);
        Gizmos.color = Color.grey;
        Vector3 outterSize = fieldSize * sizeFactor;
        Gizmos.DrawWireCube(transform.position, fieldSize + new Vector3(outterSize.x, 0, 0));
    }
#endif
}
