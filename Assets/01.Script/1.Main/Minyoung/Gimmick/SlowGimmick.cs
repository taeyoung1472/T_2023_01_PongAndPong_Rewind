using UnityEngine;

public class SlowGimmick : GimmickObject
{
    public bool isCheck = false;

    public override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void Init()
    {

    }
    private void FixedUpdate()
    {
        if (isRewind)
        {
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ãæµ¹ÇÔ");
            player = other.transform.GetComponent<Player>();
            player.playerBuff.AddBuff(PlayerBuffType.Slow);
            Debug.Log(player);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (player != null)
        {
            player.playerBuff.DeleteBuff(PlayerBuffType.Slow);
            player = null;
        }
    }
}
