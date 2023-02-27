using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DirectUnit : MonoBehaviour
{
    [HideInInspector] public SequenceType sequenceType;
    [HideInInspector] public UnityEvent unityEvent;
    [HideInInspector] public float time;
}
public enum SequenceType
{
    Append,
    Join,
    AppendInterval,
}