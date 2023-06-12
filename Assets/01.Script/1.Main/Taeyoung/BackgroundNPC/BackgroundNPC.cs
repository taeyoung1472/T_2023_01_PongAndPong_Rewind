using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNPC : MonoBehaviour
{
    public AnimateType animateType;
    private Animator npcAnim;

    public void Awake()
    {
        npcAnim = GetComponent<Animator>();
        npcAnim.SetTrigger(animateType.ToString());
        npcAnim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
    }

    public enum AnimateType
    {
        Sit,
        Phone,
        Talk,
        Idle,
        Work
    }
}