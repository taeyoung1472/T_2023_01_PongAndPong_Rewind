using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeCanvas : MonoBehaviour
{
    static Sequence loadingSeq = null;
    static bool isFirst;

    static bool isEndSequence = true;
    static bool isFade = false;
    private bool hasInstance;

    static float masterVolume;
    static float timer;

    static List<RectTransform> rectList = new();

    const string SFX = "SFX";
    const string BGM = "BGM";

    public void Awake()
    {
        if (FindObjectsOfType<SceneChangeCanvas>().Length != 1 && !hasInstance)
        {
            Destroy(gameObject);
        }
        hasInstance = true;

        if(rectList.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                rectList.Add(transform.GetChild(i).GetComponent<RectTransform>());
            }
        }

        if (!isFirst)
        {
            for (int i = 0; i < rectList.Count; i++)
            {
                rectList[i].sizeDelta = new Vector2(Screen.width, Screen.height * (1.0f / rectList.Count));
                rectList[i].anchoredPosition = new Vector2(-Screen.width, -(Screen.height * (1.0f / rectList.Count) * i));
            }
            isFirst = true;
        }
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        SetRect();
        StartCoroutine(ChangeVolume());
    }

    IEnumerator ChangeVolume()
    {
        while (true)
        {
            yield return new WaitUntil(() => !isEndSequence);
            timer = 0;

            float sfxVol;
            float bgmVol;
            AudioManager.Mixer.GetFloat(SFX, out sfxVol);
            AudioManager.Mixer.GetFloat(BGM, out bgmVol);

            float targetVol = 0;
            if (isFade)
                targetVol = -70;
            else
                targetVol = masterVolume;
            while (!isEndSequence)
            {
                timer += Time.deltaTime;

                AudioManager.Mixer.SetFloat(SFX, Mathf.Lerp(sfxVol, targetVol, timer));
                AudioManager.Mixer.SetFloat(BGM, Mathf.Lerp(bgmVol, targetVol, timer));

                if (timer > 1)
                {
                    AudioManager.Mixer.SetFloat(SFX, targetVol);
                    AudioManager.Mixer.SetFloat(BGM, targetVol);
                    isEndSequence = true;
                }
                yield return null;
            }
        }
    }

    public static void Active(Action callbackAction = null)
    {
        //Debug.Log("&&& ¿¢Æ¼ºê");
        SetRect();
        isEndSequence = false;
        isFade = true;
        timer = 0;

        Time.timeScale = 1.0f;
        loadingSeq = DOTween.Sequence();
        for (int i = 0; i < rectList.Count; i++)
        {
            loadingSeq.InsertCallback(i * 0.1f, () => AudioManager.PlayAudio(SoundType.OnSceneChange, 1 + i / 10f, 1, "Another"));
            loadingSeq.Insert(i * 0.1f, rectList[i].DOAnchorPosX(0, 0.3f)).SetEase(Ease.Linear);
        }
        loadingSeq.AppendInterval(1.5f).SetUpdate(true);
        loadingSeq.AppendCallback(() => { callbackAction?.Invoke(); });
    }
    public static void DeActive(Action callbackAction = null)
    {
        //Debug.Log("&&& µð¿¢Æ¼ºê");
        SetRect();
        isEndSequence = false;
        isFade = false;
        timer = 0;

        loadingSeq = DOTween.Sequence();
        loadingSeq.AppendCallback(() => { callbackAction?.Invoke(); });
        for (int i = rectList.Count - 1; i >= 0; i--)
        {
            loadingSeq.InsertCallback(i * 0.1f, () => AudioManager.PlayAudio(SoundType.OnSceneChange, 1 + (i / 10f), 1, "Another"));
            loadingSeq.Insert(i * 0.1f, rectList[i].DOAnchorPosX(Screen.width, 0.3f)).SetEase(Ease.Linear);
        }
        loadingSeq.AppendInterval(rectList.Count * 0.1f);
        loadingSeq.AppendCallback(() =>
        {
            for (int i = 0; i < rectList.Count; i++)
            {
                rectList[i].anchoredPosition = new Vector2(-Screen.width, rectList[i].anchoredPosition.y);
            }
        });
    }

    private static void SetRect()
    {
        for (int i = 0; i < rectList.Count; i++)
        {
            rectList[i].sizeDelta = new Vector2(Screen.width, Screen.height * (1.0f / rectList.Count));
            rectList[i].anchoredPosition = new Vector2(rectList[i].anchoredPosition.x, -(Screen.height * (1.0f / rectList.Count) * i));
        }
    }
}
