using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInteract : Interact
{
    [SerializeField]
    private Transform _endPointTrm = null;

    protected override void ChildInteractEnd()
    {
    }

    protected override void ChildInteractStart()
    {
        _player.ForceStop();
        _player.transform.position = _endPointTrm.position;
        InteractEnd(true);
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_endPointTrm != null)
        {
            Gizmos.DrawSphere(_endPointTrm.position, 0.5f);
        }
    }
#endif
}
