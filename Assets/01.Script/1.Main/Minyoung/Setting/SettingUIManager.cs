using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour
{
    [SerializeField] private GameObject controlScroll;
    [SerializeField] private GameObject settingScroll;

    public bool isControll;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isControll)
            {
                controlScroll.SetActive(false);
            isControll = false;
            }
            else
            {
                settingScroll.SetActive(false);
            }
        }
    }
    public void OnSettingScrol()
    {
        settingScroll.SetActive(true);

    }
    public void OnControlScroll()
    {
        controlScroll.SetActive(true);
        isControll = true;
    }
}
