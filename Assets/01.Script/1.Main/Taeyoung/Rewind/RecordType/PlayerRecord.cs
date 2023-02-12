using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecord : TransformRecord
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private List<Sprite> spriteList;
    private List<bool> flipList;

    [SerializeField] private List<MonoBehaviour> enableList;
    [SerializeField] private CharacterController characterController;

    public override void InitOnPlay()
    {
        base.InitOnPlay();

        foreach (var item in enableList)
        {
            item.enabled = true;
        }

        characterController.enabled = true;
        animator.enabled = true;
    }

    public override void InitOnRewind()
    {
        base.InitOnRewind();

        foreach (var item in enableList)
        {
            item.enabled = false;
        }

        characterController.enabled = false;
        animator.enabled = false;
    }

    public override void Register()
    {
        base.Register();

        GenerateList<Sprite>(ref spriteList, spriteRenderer.sprite);

        GenerateList<bool>(ref flipList, spriteRenderer.transform.localScale.x > 0 ? true : false);
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
        spriteRenderer.transform.localScale = flipList[index] ? Vector3.one * 0.5f : new Vector3(-1, 1, 1) * 0.5f;
    }
}
