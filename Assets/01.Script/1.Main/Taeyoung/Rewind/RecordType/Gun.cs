using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePos;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioRecord.Instance.PlayAudio(shootClip);
            Instantiate(bullet, firePos.position, firePos.rotation);
        }
    }
}
