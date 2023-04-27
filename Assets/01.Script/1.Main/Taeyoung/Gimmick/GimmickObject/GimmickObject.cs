using UnityEngine;
public abstract class GimmickObject : MonoBehaviour
{
    public Player player { get; set; }
    public bool isRewind = false;
    public abstract void Init();
    public virtual void InitOnRewind() 
    {
        isRewind = true;
        Debug.Log(isRewind);
        player = null;
    }
    public virtual void InitOnPlay()
    {
        isRewind = false;
        player = null;
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        //Debug.Log(isRewind);
    }
    public virtual void InitOnRestart()
    {

    }

    public virtual void Awake()
    {
        if (RewindManager.Instance)
        {
            RewindManager.Instance.InitRewind += InitOnRewind;
            RewindManager.Instance.InitPlay += InitOnPlay;
        }
    }
}
