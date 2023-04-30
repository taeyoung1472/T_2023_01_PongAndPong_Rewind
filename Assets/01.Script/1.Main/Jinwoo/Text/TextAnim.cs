using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TextAnim : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    [SerializeField] private TextAnimDataSO textData;

    private int idx = 0;

    private bool isAnim = true;

    void Start()
    {
        //EndCheck();
    }
    public void SetText(TextMeshProUGUI text)
    {
        _textMeshPro = text;
    }
    public void SetTextSpeed(float speed)
    {
        textData.timeBtwnChars = speed;
    }
    public void EndCheck()
    {
        if (idx <= textData.stringArray.Length - 1)
        {
            _textMeshPro.text = textData.stringArray[idx];
            idx += 1;
            isAnim = true;
            StartCoroutine(TextVisible());
        }
    }
    public void StopAnim()
    {
        StopAllCoroutines();
        isAnim = false;
        _textMeshPro.SetText("");
        _textMeshPro.ClearMesh();
    }
    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (isAnim)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                
                //Invoke("EndCheck", textData.timeBtwnWords);
                break;
            }

            counter += 1;
            yield return new WaitForSeconds(textData.timeBtwnChars);


        }
    }
}