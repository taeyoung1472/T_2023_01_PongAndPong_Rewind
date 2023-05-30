using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleTon<UIManager>
{
    [Header("[Clock]")]
    [SerializeField] private Slider clockFill;
    [SerializeField] private TextMeshProUGUI clockTimeText;

    private float totalTIme { get { return RewindManager.Instance.howManySecondsToTrack; } }

    private bool isPause = false;

    public bool IsPause => isPause;
    [SerializeField] private FreeLookCamera freeLookCamera;

    [SerializeField] private GameObject pauseImg;
    [SerializeField] private GameObject collectionImg;    
    #region 기믹 도감
    [Header("[Gimmick]")]
    [SerializeField] private GimmickEncyclopediaSO gimmickEncyclopediaSO;
    [SerializeField] private GameObject gimmickInfoPrefab;
    [SerializeField] private GameObject gimmickEncyclopediaImage;
    [SerializeField] private Transform gimmickInfoParentTrm;
    #endregion

    public void OnPlayTimeChange(float time)
    {
        clockFill.value = time / totalTIme;
        clockTimeText.SetText($"{(int)time}");
    }

    public void OnRewindTimeChange(float time)
    {
        clockFill.value = (time / totalTIme);
        clockTimeText.SetText($"{(int)time}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !BreakScreenController.Instance.isBreaking)
        {
            if (!isPause && !EndManager.Instance.IsEnd)
            {
                isPause = true;
                TimerManager.Instance.ChangeOnTimer(false);
                pauseImg.gameObject.SetActive(true);
                freeLookCamera.Rig.transform.position = new Vector3(0f, 3.35f, -13f);
                freeLookCamera._isActivated = false;
                Time.timeScale = 0f;
            }
            else if (!EndManager.Instance.IsEnd)
            {
                PauseResume();
            }
        }
    }

    public void PauseResume()
    {
        isPause = false;
        if (StageManager.Instance.GetAreaPlayCheck()) //게임 시작 도중이였을 때
        {
            TimerManager.Instance.ChangeOnTimer(true);
        }
        pauseImg.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseMenu()
    {
        LoadingSceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void PauseStage()
    {

    }
    public void PauseCollection()
    {
        pauseImg.SetActive(false); 
        collectionImg.SetActive(true);
        PhoneCollection.Instance.OnCollectionMenu();
    }
    public void PauseGimmick()
    {
        pauseImg.SetActive(false);
        gimmickEncyclopediaImage.SetActive(true);

        if (gimmickInfoParentTrm.childCount <= 0)
        {
            for (int i = 0; i < gimmickEncyclopediaSO.gimmickEncyclopedia.Count; i++)
            {
                GameObject obj = Instantiate(gimmickInfoPrefab, gimmickInfoParentTrm);

                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = gimmickEncyclopediaSO.gimmickEncyclopedia[i].gimmickIcon;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = gimmickEncyclopediaSO.gimmickEncyclopedia[i].gimmickName;
            }
        }
    }
}
