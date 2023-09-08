using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCutStageController : MonoSingleTon<StartCutStageController>
{
    public TextMeshProUGUI text;
    [SerializeField] private StartCutStage[] cutStages;
    [HideInInspector] public StartCutStage curPlayingStage;
    private int curStageIndex = 0;

    public void PlayStage()
    {
        curPlayingStage = cutStages[curStageIndex];
        curStageIndex += 1;
        curPlayingStage.Play();
    }
}
