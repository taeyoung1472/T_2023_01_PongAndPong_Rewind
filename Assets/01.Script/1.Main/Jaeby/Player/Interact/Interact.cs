using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interact : MonoBehaviour
{
    protected bool _interactable = true;
    public bool Interactable { get => _interactable; set => _interactable = value; }
    public abstract void InteractStart(Player player);
    public abstract void InteractEnd(Player player);
}
