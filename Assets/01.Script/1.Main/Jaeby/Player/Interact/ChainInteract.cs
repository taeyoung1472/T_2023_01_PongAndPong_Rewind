using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainInteract : MonoBehaviour
{
    protected Player _player = null;

    public void Init(Player player)
    {
        _player = player;
    }

    public abstract void InteractStart();

    public abstract void InteractEnd();
}
