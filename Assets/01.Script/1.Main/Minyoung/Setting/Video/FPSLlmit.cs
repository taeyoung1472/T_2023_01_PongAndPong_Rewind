using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPSLlmit : MonoBehaviour
{
    public Dropdown fpsDroptown;

    [SerializeField] private List<int> fpsList = new List<int>();

    public Button preBtn;
    public Button nextBtn;

    public int index = 0;

    public TextMeshProUGUI fpsText;
    public void Awake()
    {
        InitUI();
    }
    public void Start()
    {
        preBtn.onClick.AddListener(() =>
        {
            if (index != 0)
            {
                index--;
                fpsText.text = fpsList[index].ToString();
            }
        });
        nextBtn.onClick.AddListener(() =>
        {
            if (index == fpsList.Count - 1)
            {
                index = fpsList.Count -1;
            }
            else
            {
                index++;
            }

            //index = index % fpsList.Count;
            fpsText.text = fpsList[index].ToString();
        });
    }
    private void InitUI()
    {
        //fpsDroptown.options.Clear();

        //for (int i = 0; i < fpsList.Count; i++)
        //{
        //    Dropdown.OptionData option = new Dropdown.OptionData();

        //    option.text = fpsList[i].ToString();
        //    fpsDroptown.options.Add(option);
        //}
        //fpsDroptown.RefreshShownValue();

    }

    public void ApplyFPS()
    {
        // Application.targetFrameRate = fpsDroptown.value;
        Application.targetFrameRate = int.Parse(fpsText.text);
        Debug.Log(Application.targetFrameRate);
    }

}
