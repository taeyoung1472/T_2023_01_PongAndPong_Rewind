using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipManager : MonoSingleTon<SkipManager>
{
    [SerializeField] private Image skipImg;
    public bool isSkip = false;

    private void Start()
    {
        isSkip = false;
        skipImg.fillAmount = 0;
    }
    private void Update()
    {
        if(skipImg.fillAmount >= 1f)
        {
            isSkip = true;
            StartCoroutine(SkipCutScene());
            return;   
        }

        if (Input.GetKey(KeyCode.Return) )
        {
            skipImg.fillAmount += 0.5f * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            skipImg.fillAmount = 0f;
        }
    }
    IEnumerator SkipCutScene()
    {
        FadeInOutManager.Instance.FadeIn(2f);
        yield return new WaitForSeconds(3.2f);
        SceneManager.LoadScene("Lab");
    }
}
