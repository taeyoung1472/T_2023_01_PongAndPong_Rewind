using UnityEngine;

public class FocusObject : MonoBehaviour
{
    private void OnEnable()
    {
        if (CamManager.Instance == null)
            return;

        CamManager.Instance.AddTargetGroup(transform);
    }

    private void OnDisable()
    {
        if (CamManager.Instance == null)
            return;

        CamManager.Instance.RemoveTargetGroup(transform);
    }
}
