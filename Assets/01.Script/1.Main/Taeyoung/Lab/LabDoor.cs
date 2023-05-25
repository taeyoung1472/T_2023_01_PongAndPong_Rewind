using DG.Tweening;
using UnityEngine;

public class LabDoor : MonoBehaviour
{
    private Transform playerTrans;

    [SerializeField] private float openDist;
    [SerializeField] private float closeDist;
    private bool isOpen;

    [SerializeField] private Transform[] pivots;

    private void Awake()
    {
        playerTrans = FindObjectOfType<Player>().transform;
    }

    void FixedUpdate()
    {
        float dist = Mathf.Abs(transform.position.x - playerTrans.position.x);

        if (isOpen)
        {
            if (dist > closeDist)
            {
                Close();
            }
        }
        else
        {
            if (dist < openDist)
            {
                Open();
            }
        }
    }

    void Open()
    {
        AudioManager.PlayAudio(SoundType.OnLabDoorOpen);
        isOpen = true;

        foreach (var pivot in pivots)
        {
            pivot.DOScale(new Vector3(1, 1, 0), 0.4f);
        }
    }

    void Close()
    {
        AudioManager.PlayAudio(SoundType.OnLabDoorClose, 1.75f);
        isOpen = false;

        foreach (var pivot in pivots)
        {
            pivot.DOScale(new Vector3(1, 1, 1), 0.3f);
        }
    }
}
