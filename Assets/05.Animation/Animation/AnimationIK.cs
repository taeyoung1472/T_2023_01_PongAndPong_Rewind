using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationIK : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform tabletHandL;
    [SerializeField] private Transform tabletHandR;

    [SerializeField] private Transform lookTrm;
    [SerializeField] private Transform handL;
    [SerializeField] private Transform handR;
    [SerializeField] private Transform footL;
    [SerializeField] private Transform footR;

    public Transform LookTrm { get => lookTrm; set => lookTrm = value; }
    public Transform HandL { get => handL; set => handL = value; }
    public Transform HandR { get => handR; set => handR = value; }
    public Transform FootL { get => footL; set => footL = value; }
    public Transform FootR { get => footR; set => footR = value; }

    float lookWeight = 1f;
    float handLWeight;
    float handRWeight;
    float footLWeight;
    float footRWeight;

    private bool _rotationLock = false;
    public bool RotationLock { get => _rotationLock; set => _rotationLock = value; }

    private void OnAnimatorIK(int layerIndex)
    {
        IKSet();
    }

    public void IKUpdate()
    {
        IKSet();
    }

    public void TabletSetStart()
    {
        transform.GetComponentInParent<Player>().PlayerActionExit(PlayerActionType.Attack);
        transform.GetComponentInParent<Player>().GetPlayerAction<PlayerAttack>().WeaponSwitching(AttackState.Melee, true);
        lookTrm = null;
        HandL = tabletHandL;
        HandR = tabletHandR;
        RotationLock = false;
        SetIKWeightOne();
    }

    public void TabletSetEnd()
    {
        HandL = null;
        HandR = null;
        SetIKWeightZero();
    }

    public void SetIKWeight(float weight)
    {
        lookWeight = weight;
        handLWeight = weight;
        handRWeight = weight;
        footLWeight = weight;
        footRWeight = weight;
    }

    public void SetIKWeightZero()
    {
        lookWeight = 0;
        handLWeight = 0;
        handRWeight = 0;
        footLWeight = 0;
        footRWeight = 0;
    }

    public void SetIKWeightOne()
    {
        lookWeight = 1;
        handLWeight = 1;
        handRWeight = 1;
        footLWeight = 1;
        footRWeight = 1;
    }

    private void IKSet()
    {
        if (lookTrm)
        {
            animator.SetLookAtPosition(lookTrm.position);
            animator.SetLookAtWeight(lookWeight);
        }
        if (handL)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, handL.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handLWeight);
            if (_rotationLock == false)
            {
                animator.SetIKRotation(AvatarIKGoal.LeftHand, handL.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handLWeight);
            }
        }

        if (handR)
        {
            if (_rotationLock == false)
            {
                animator.SetIKRotation(AvatarIKGoal.RightHand, handR.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handRWeight);
            }

            animator.SetIKPosition(AvatarIKGoal.RightHand, handR.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handRWeight);
        }

        if (footL)
        {

            if (_rotationLock == false)
            {
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, footL.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, footLWeight);
            }
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footL.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footLWeight);
        }

        if (footR)
        {

            if (_rotationLock == false)
            {
                animator.SetIKRotation(AvatarIKGoal.RightFoot, footR.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, footRWeight);
            }
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footR.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, footRWeight);
        }
    }
}
