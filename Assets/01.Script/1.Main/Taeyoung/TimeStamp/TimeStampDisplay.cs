using UnityEngine;
using UnityEngine.UI;

public class TimeStampDisplay : MonoBehaviour
{
    [SerializeField] private Image stampIcon;
    [SerializeField] private Image stampLine;

    private float myTime;

    private void Awake()
    {
        TimerManager.Instance.OnTimeChange += OnTimeChange;
    }

    public void Set(Sprite icon, Color color, float t)
    {
        stampIcon.sprite = icon;
        stampIcon.color = color;
        stampLine.color = color;
        myTime = t;
    }

    void OnTimeChange(float t)
    {
        if (RewindManager.Instance.IsBeingRewinded)
        {
            if (Mathf.Abs(myTime - t) < Time.deltaTime * 5)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (TimerManager.Instance)
        {
            TimerManager.Instance.OnTimeChange -= OnTimeChange;
        }
    }
}
