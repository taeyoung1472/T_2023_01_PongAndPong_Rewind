using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationIK : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform handL;
    [SerializeField] private Transform handR;
    [SerializeField] private Transform footL;
    [SerializeField] private Transform footR;

    float handLWeight;
    float handRWeight;
    float footLWeight;
    float footRWeight;

    private void OnAnimatorIK(int layerIndex)
    {
        if (handL)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, handL.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handLWeight);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, handL.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handLWeight);
        }

        if (handR)
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, handR.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handRWeight);
            animator.SetIKRotation(AvatarIKGoal.RightHand, handR.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handRWeight);
        }

        if (footL)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footL.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footLWeight);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, footL.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, footLWeight);
        }

        if (footR)
        {
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footR.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, footRWeight);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, footR.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, footRWeight);
        }
    }

    public void SetIKWeightZero()
    {
        handLWeight = 0;
        handRWeight = 0;
        footLWeight = 0;
        footRWeight = 0;
    }

    public void SetIKWeightOne() 
    {
        handLWeight = 1;
        handRWeight = 1;
        footLWeight = 1;
        footRWeight = 1;
    }
}
