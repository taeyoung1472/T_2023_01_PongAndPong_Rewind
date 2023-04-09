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
    public enum DirState
    {
        Up,
        Left,
        LeftCross,
        RightCross,
    }
    public DirState dirState;

    private void Start()
    {
        secondmovePos = transform.position;
        switch (dirState)
        {
            case DirState.Up:
                firstmovePos = new Vector3(transform.position.x, transform.position.y + moveDistance, transform.position.z);
                StartCoroutine(Move());
                break;

            case DirState.Left:
                firstmovePos = new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z);
                StartCoroutine(Move());
                break;
            case DirState.LeftCross:
                firstmovePos = new Vector3(transform.position.x - moveDistance, transform.position.y - moveDistance, transform.position.z);
                StartCoroutine(Move());
                break;
            case DirState.RightCross:
                firstmovePos = new Vector3(transform.position.x - moveDistance, transform.position.y + moveDistance, transform.position.z);
                StartCoroutine(Move());
                break;
        }
    }
    IEnumerator Move()
    {
        while(true)
        {
            transform.DOMove(firstmovePos, moveTime);
            yield return new WaitForSeconds(waitTime);
            transform.DOMove(secondmovePos, moveTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
    public override void Init()
    {
    }
}
