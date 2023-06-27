using UnityEngine;

public class FocusObject : MonoBehaviour
{
    [SerializeField] private StageArea myArea;

    private void Awake()
    {
        if (myArea)
        {
            myArea.OnEntryArea += () =>
            {
                if (CamManager.Instance == null)
                    return;

                CamManager.Instance.AddTargetGroup(transform);
            };

            myArea.OnExitArea += () =>
            {
                if (CamManager.Instance == null)
                    return;

                CamManager.Instance.RemoveTargetGroup(transform);
            };
        }
    }

    private void OnEnable()
    {
        if(myArea == null)
        {
            if (CamManager.Instance == null)
                return;

            CamManager.Instance.AddTargetGroup(transform);
        }
    }

    private void OnDisable()
    {
        if (myArea == null)
        {
            if (CamManager.Instance == null)
                return;

            CamManager.Instance.RemoveTargetGroup(transform);
        }
    }
}
