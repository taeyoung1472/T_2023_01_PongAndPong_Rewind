using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class TextAnimCutScene : MonoBehaviour
{
    public CinemachineVirtualCamera npcCam;
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    [SerializeField] private CutSceneTextAnimDataSO textData;
    public CutSceneTextAnimDataSO TextData
    { get => textData; set => textData = value; }

    private int idx = 0;

    public bool isAnim = false;

    public bool isComplete = false;
    public bool isSkip = false;

    void Awake()
    {
        //EndCheck();
        isComplete = false;
        isAnim = false;
        isSkip = false;
    }
    private void OnEnable()
    {
        isAnim = false;
        isSkip = false;
    }
    public void SetText(TextMeshProUGUI text)
    {
        _textMeshPro = text;
    }
    public void SetTextSpeed(float speed)
    {
        textData.timeBtwnChars = speed;
    }
    public void ClearText()
    {
        _textMeshPro.SetText("");
    }
    public void EndCheck(int num)
    {
        if (idx <= textData.cutSceneTexts.Length - 1)
        {
            _textMeshPro.text = textData.cutSceneTexts[idx].text;
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
            if (!isSkip)
            {
                AudioManager.PlayAudioRandPitch(SoundType.OnNPCSpeak);
                yield return new WaitForSeconds(textData.timeBtwnChars);
            }


        }
        isSkip = false;
        isAnim = false;
        isComplete = true;
    }

    int num = 1;
    public void CutSceneTextAnim()
    {
        StopAnim();
        EndCheck(num++);
    }
    
}