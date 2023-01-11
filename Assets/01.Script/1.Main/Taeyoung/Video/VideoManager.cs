public class VideoManager : MonoSingleTon<VideoManager>
{
    private RewindManager rewindManager;
    private float videoTickTimer = 0.0f;
    private float videoTimer = 0.0f;

    private bool isRewinding = false;

    public void Awake()
    {
        Find();
        Set();
    }

    private void Find()
    {
        rewindManager = RewindManager.Instance;
    }

    private void Set()
    {
        videoTickTimer = rewindManager.RecordeTurm;
    }

    public void Play()
    {

    }
}
