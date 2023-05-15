using UnityEngine;

public class TutorialChoiceBtn : MonoBehaviour
{
    [SerializeField] private bool isChoice;

    public void OnMouseDown()
    {
        TutorialManager.Instance.Choice(isChoice);
    }
}
