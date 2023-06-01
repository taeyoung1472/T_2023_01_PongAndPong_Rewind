using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class TextAnim_Tuto : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    StringBuilder sb = new();

    string curText;

    public bool isComplete = false;
    public bool isSkip = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isComplete)
            {
                Skip();
            }
        }
    }

    public void SetText(string text)
    {
        _textMeshPro.SetText("");

        isComplete = false;
        isSkip = false;
        sb.Clear();

        curText = text;
        StartCoroutine(TextVisible());
    }

    public void Skip()
    {
        isSkip = true;
    }

    public bool IsEnd()
    {
        return isComplete;
    }

    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int textLength = curText.Length;
        int counter = 0;

        while (counter < textLength)
        {
            sb.Append(curText[counter]);
            _textMeshPro.SetText(sb.ToString());

            counter++;
            if (!isSkip)
            {
                AudioManager.PlayAudioRandPitch(SoundType.OnNPCSpeak);
                yield return new WaitForSeconds(0.1f);
            }
        }

        isComplete = true;
    }
}