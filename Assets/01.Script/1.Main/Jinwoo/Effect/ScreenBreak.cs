using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBreak : MonoBehaviour
{
    [SerializeField]private List<Vector3> childTrm = new List<Vector3>();
    private void Awake()
    {
        //Vector3 explosionPosition = new Vector3(45.833f, 0f, 0f);
        foreach (Transform child in transform)
        {
            childTrm.Add(child.transform.localPosition);
        }

    }
    
    public void BreakScreen(Transform trm)
    {
        AudioManager.PlayAudioRandPitch(SoundType.OnReplayBreak);
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.isKinematic = false;
                //AddExplosionForce(Æø¹ß·Â, Æø¹ßÀ§Ä¡, ¹Ý°æ, À§·Î ¼Ú±¸ÃÄ¿Ã¸®´Â Èû)
                childRigidbody.AddExplosionForce(120f, trm.position, 7f);
               
            }
        }
    }
    
    public void InitPos()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.isKinematic = true;
                child.localPosition = childTrm[i];
                child.rotation = Quaternion.identity;

                i++;
            }
        }
    }
}
