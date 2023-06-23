using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornGimmick : GimmickObject
{
    private Collider _col;

    public bool isCheck = false;

    [SerializeField] private float rayDistance = 0.5f;

    RaycastHit hit;

    public bool isDie;
    public override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void Init()
    {
        _col = GetComponent<BoxCollider>();
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