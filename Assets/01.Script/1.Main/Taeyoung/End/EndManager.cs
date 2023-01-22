using TMPro;
using UnityEngine;

public class EndManager : MonoSingleTon<EndManager>
{
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject clearButton;
    [SerializeField] private GameObject restartButton;

    private bool isClear;
    public bool IsClear { get { return isClear; } set { isClear = value; } }

    public void ActivePanel()
    {
        resultText.text = isClear ? "스테이지 클리어" : "스테이지 실패";

        clearButton.SetActive(isClear);
        restartButton.SetActive(!isClear);

        endPanel.SetActive(true);
    }
}
