using TMPro;
using UnityEngine;

public class EndManager : MonoSingleTon<EndManager>
{
    [SerializeField] private GameObject endPanel;

    public void End()
    {
        endPanel.SetActive(true);
    }
}
