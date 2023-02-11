using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class SceneChangeCanvas : MonoBehaviour
{
    static CanvasGroup group;
    static Sequence loadingSeq = null;

    static bool isEndSequence = true;
    static bool isFade = false;
    private bool hasInstance;

    static float masterVolume;
    static float timer;

    const string MASTER = "Master";

    public void Awake()
    {
        if (FindObjectsOfType<SceneChangeCanvas>().Length != 1 && !hasInstance)
        {
            Destroy(gameObject);
        }
        hasInstance = true;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        group = GetComponent<CanvasGroup>();
        DeActive();
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
        loadingSeq.Append(DOTween.To(() => group.alpha, x => group.alpha = x, 1, 0.5f)).SetUpdate(true);
        loadingSeq.AppendInterval(1.5f).SetUpdate(true);
        loadingSeq.AppendCallback(() => { callbackAction?.Invoke(); });
    }
    public static void DeActive(Action callbackAction = null)
    {
        isEndSequence = false;
        isFade = false;
        timer = 0;

        loadingSeq = DOTween.Sequence();
        loadingSeq.Append(DOTween.To(() => group.alpha, x => group.alpha = x, 0, 0.5f)).SetUpdate(true);
        loadingSeq.AppendCallback(() => { callbackAction?.Invoke(); });
    }
}
