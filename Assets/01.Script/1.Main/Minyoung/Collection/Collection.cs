using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField] private GameObject eatParticle;
    [SerializeField] private int index;

    [SerializeField] private int zoneIndex;

    public Vector3 pos;
    private bool isEat;
    public bool IsEat => isEat;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            isEat = true;
            Instantiate(eatParticle, other.transform.position, Quaternion.identity);

            StageManager.Instance.CurStageDataSO.stageCollection[index].zone[zoneIndex] = IsEat;

            gameObject.SetActive(false);
            AudioManager.PlayAudioRandPitch(SoundType.OnCollect);
        }
    }
}
