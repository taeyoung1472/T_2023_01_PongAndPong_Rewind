using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VideoManager : MonoSingleTon<VideoManager>
{
    private RewindManager rewindManager;
    private float videoTickDelay = 0.0f;
    private float videoTimer = 0.0f;
    private bool isActive = false;

    [SerializeField] private VideoPlayType playType;
    [SerializeField] private Slider videoSlider;

    public void Start()
    {
        Find();
        StartCoroutine(WaitForEnd());
    }

    public void VideoPlay()
    {
        playType = VideoPlayType.Play;
    }

    public void VideoStop()
    {
        playType = VideoPlayType.Stop;
    }

    public void VideoRewind()
    {
        playType = VideoPlayType.Rewind;
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitUntil(() => rewindManager.IsEnd);
        Set();
        isActive = true;
    }

    private void Find()
    {
        rewindManager = RewindManager.Instance;
    }

    private void Set()
    {
        videoTickDelay = rewindManager.RecordeTurm;
        videoTimer = Time.time + videoTickDelay;
        playType = VideoPlayType.Stop;
        videoSlider.maxValue = rewindManager.TotalRecordCount - 1;

        rewindManager.OnTimeChanging += (int t) => videoSlider.value = t;
    }

    public void Update()
    {
        if (!isActive) return;
        Play();
    }

    public void Play()
    {
        if (Time.time > videoTimer)
        {
            videoTimer = Time.time + videoTickDelay;

            switch (playType)
            {
                case VideoPlayType.Play:
                    rewindManager.CurRecordingIndex++;
                    break;
                case VideoPlayType.Rewind:
                    rewindManager.CurRecordingIndex--;
                    break;
            }
        }
    }
}

public enum VideoPlayType
{
    Play,
    Stop,
    Rewind
}
