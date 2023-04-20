using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCheckAble
{
    public Transform trm { get; set; }


    public Player PlayerCheck()
    {
        foreach (var col in Physics.OverlapBox(trm.position, Vector3.one * 2.5f))
        {
            if (col.gameObject.TryGetComponent<Player>(out Player player))
            {
                return player;
            }
        }
        return null;
    }
}
