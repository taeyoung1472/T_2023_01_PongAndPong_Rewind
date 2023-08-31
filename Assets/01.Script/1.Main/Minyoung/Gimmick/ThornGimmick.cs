using UnityEngine;

public class ThornGimmick : GimmickObject
{
    public bool isCheck = false;
    public bool isDie;

    public override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void Init()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (isDie)
                return;
            player.playerHP.Die();
            isDie = true;
        }
    }

    public override void InitOnRestart()
    {
        base.InitOnRestart();
        isDie = false;
    }
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        isDie = false;
    }
}