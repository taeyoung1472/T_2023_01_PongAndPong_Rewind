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