using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveGimmick : GimmickObject
{
    private Vector3 firstmovePos;
    private Vector3 secondmovePos;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float moveTime = 3f;
    [SerializeField] private float moveDistance = 5f;

    public DirectionType dirState;
    public override void InitOnRewind()
    {
        base.InitOnRewind();
        transform.DOKill();
    }

    private void Start()
    {
        MoveSet();
    }

    public void DirChange(DirectionType dirState)
    {
        this.dirState = dirState;
        MoveSet();
    }

    private void MoveSet()
    {
        transform.DOKill();
        firstmovePos = transform.position + (Vector3)Utility.GetDirToVector(dirState) * moveDistance;
        transform.DOMove(firstmovePos, moveTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    DirChange((DirectionType)Random.Range(0, (int)DirectionType.RightDown + 1));
        //}
    }

    public override void Init()
    {
    }
}
