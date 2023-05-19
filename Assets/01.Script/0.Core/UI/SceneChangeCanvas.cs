using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeCanvas : MonoBehaviour
{
    static Sequence loadingSeq = null;

    static bool isEndSequence = true;
    static bool isFade = false;
    private bool hasInstance;

    static float masterVolume;
    static float timer;

    static List<RectTransform> rectList = new();

    const string MASTER = "Master";

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
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(ChangeVolume());
    }

    IEnumerator ChangeVolume()
    {
        while (true)
        {
            yield return new WaitUntil(() => !isEndSequence);
            timer = 0;

            float startVol;
            AudioManager.Mixer.GetFloat(MASTER, out startVol);

            float targetVol = 0;
            if (isFade)
                targetVol = -70;
            else
                targetVol = masterVolume;
            while (!isEndSequence)
            {
                timer += Time.deltaTime;

                AudioManager.Mixer.SetFloat(MASTER, Mathf.Lerp(startVol, targetVol, timer));

                if (timer > 1)
                {

                    AudioManager.Mixer.SetFloat(MASTER, targetVol);
                    isEndSequence = true;
                }
                yield return null;
            }
        }
    }

    public static void Active(Action callbackAction = null)
    {
        isEndSequence = false;
        isFade = true;
        timer = 0;
        AudioManager.Mixer.GetFloat(MASTER, out masterVolume);

        Time.timeScale = 1.0f;
        loadingSeq = DOTween.Sequence();
        for (int i = 0; i < rectList.Count; i++)
        {
            loadingSeq.Insert(i * 0.1f, rectList[i].DOAnchorPosX(0, 0.3f)).SetEase(Ease.Linear);
        }
        loadingSeq.AppendInterval(1.5f).SetUpdate(true);
        loadingSeq.AppendCallback(() => { callbackAction?.Invoke(); });
    }
    public static void DeActive(Action callbackAction = null)
    {
        isEndSequence = false;
        isFade = false;
        timer = 0;

        loadingSeq = DOTween.Sequence();
        loadingSeq.AppendCallback(() => { callbackAction?.Invoke(); });
        for (int i = rectList.Count - 1; i >= 0; i--)
        {
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
}
