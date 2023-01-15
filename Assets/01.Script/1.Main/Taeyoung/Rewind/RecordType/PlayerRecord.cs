using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecord : TransformRecord
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private List<Sprite> spriteList;
    private List<bool> flipList;

    PlayerInput playerInput;

    public override void InitOnPlay()
    {
        base.InitOnPlay();

        if(playerInput== null)
            playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
        animator.enabled = true;
    }

    public override void InitOnRewind()
    {
        base.InitOnRewind();

        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = false;
        animator.enabled = false;
    }

    public override void Register()
    {
        base.Register();

        spriteList = new(TotalRecordCount);
        spriteList.AddRange(new Sprite[TotalRecordCount]);
        spriteList[0] = spriteRenderer.sprite;

        flipList = new(TotalRecordCount);
        flipList.AddRange(new bool[TotalRecordCount]);
        flipList[0] = spriteRenderer.transform.localScale.x > 0 ? true : false;
    }

    public override void Recorde(int index)
    {
        base.Recorde(index);

        spriteList[index] = spriteRenderer.sprite;
        flipList[index] = spriteRenderer.transform.localScale.x > 0 ? true : false;
    }

    public override void ApplyData(int index, int nextIndexDiff)
    {
        base.ApplyData(index, nextIndexDiff);

        spriteRenderer.sprite = spriteList[index];
        spriteRenderer.transform.localScale = flipList[index] ? Vector3.one : new Vector3(-1, 1, 1);
    }
}
