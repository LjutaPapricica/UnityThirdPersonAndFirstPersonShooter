using UnityEngine;
using System.Collections;
using System;

namespace UnityEngine
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorController : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private Vector3 rightHandPos, leftHandPos, rightFootPos, leftFootPos, leftElbowPos, rightElbowPos, rightKneePos, leftKneePos, lookAtPosition;
        [SerializeField] private Quaternion rightHandRot, leftHandRot, rightFootRot, leftFootRot;
        [SerializeField] [Range(0, 1)] private float rightHandPosWeight, leftHandPosWeight, rightFootPosWeight, leftFootPosWeight, rightElbowPosWeight, leftElbowPosWeight, rightKneePosWeight, leftKneePosWeight, rightHandRotWeight, leftHandRotWeight, rightFootRotWeight, leftFootRotWeight, lookAtWeight, lookAtBodyWeight, lookAtHeadWeight, lookAtEyesWeight, lookAtClampWeight;
        private Vector3 internalLeftFootPos, internalRightFootPos, internalLeftHandPos, internalRightHandPos, internalLeftKneePos, internalRightKneePos, internalLeftElbowPos, internalRightElbowPos;
        private Quaternion internalLeftFootRot, internalRightFootRot, internalLeftHandRot, internalRightHandRot;
        private float internalRightKneeWeight, internalRightElbowWeight, internalLeftFootPosWeight, internalRightFootPosWeight, internalLeftHandPosWeight, internalRightHandPosWeight, internalLeftKneeWeight, internalLeftElbowWeight, internalRightHandRotWeight, internalLeftHandRotWeight, internalLeftFootRotWeight, internalRightFootRotWeight;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public float GetFloat(AvatarIKGoal goal, bool IsPos)
        {
            float val = 0;
            if (IsPos == true)
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        val = leftFootPosWeight;
                        break;
                    case AvatarIKGoal.RightFoot:
                        val = rightFootPosWeight;
                        break;
                    case AvatarIKGoal.LeftHand:
                        val = leftHandPosWeight;
                        break;
                    case AvatarIKGoal.RightHand:
                        val = rightHandPosWeight;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        val = leftFootRotWeight;
                        break;
                    case AvatarIKGoal.RightFoot:
                        val = rightFootRotWeight;
                        break;
                    case AvatarIKGoal.LeftHand:
                        val = leftHandRotWeight;
                        break;
                    case AvatarIKGoal.RightHand:
                        val = rightHandRotWeight;
                        break;
                    default:
                        break;
                }
            }
            return val;
        }

        public float GetFloat(AvatarIKGoal goal, bool IsPos, bool Internal)
        {
            float val = 0;
            if (Internal == true)
            {
                if (IsPos == true)
                {
                    switch (goal)
                    {
                        case AvatarIKGoal.LeftFoot:
                            val = internalLeftFootPosWeight;
                            break;
                        case AvatarIKGoal.RightFoot:
                            val = internalRightFootPosWeight;
                            break;
                        case AvatarIKGoal.LeftHand:
                            val = internalLeftHandPosWeight;
                            break;
                        case AvatarIKGoal.RightHand:
                            val = internalRightHandPosWeight;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (goal)
                    {
                        case AvatarIKGoal.LeftFoot:
                            val = internalLeftFootRotWeight;
                            break;
                        case AvatarIKGoal.RightFoot:
                            val = internalRightFootRotWeight;
                            break;
                        case AvatarIKGoal.LeftHand:
                            val = internalLeftHandRotWeight;
                            break;
                        case AvatarIKGoal.RightHand:
                            val = internalRightHandRotWeight;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                if (IsPos == true)
                {
                    switch (goal)
                    {
                        case AvatarIKGoal.LeftFoot:
                            val = leftFootPosWeight;
                            break;
                        case AvatarIKGoal.RightFoot:
                            val = rightFootPosWeight;
                            break;
                        case AvatarIKGoal.LeftHand:
                            val = leftHandPosWeight;
                            break;
                        case AvatarIKGoal.RightHand:
                            val = rightHandPosWeight;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (goal)
                    {
                        case AvatarIKGoal.LeftFoot:
                            val = leftFootRotWeight;
                            break;
                        case AvatarIKGoal.RightFoot:
                            val = rightFootRotWeight;
                            break;
                        case AvatarIKGoal.LeftHand:
                            val = leftHandRotWeight;
                            break;
                        case AvatarIKGoal.RightHand:
                            val = rightHandRotWeight;
                            break;
                        default:
                            break;
                    }
                }
            }
            
            return val;
        }

        public float GetFloat(AvatarIKHint goal)
        {
            float val = 0;
            switch (goal)
            {
                case AvatarIKHint.LeftKnee:
                    val = leftKneePosWeight;
                    break;
                case AvatarIKHint.RightKnee:
                    val = rightKneePosWeight;
                    break;
                case AvatarIKHint.LeftElbow:
                    val = leftElbowPosWeight;
                    break;
                case AvatarIKHint.RightElbow:
                    val = rightElbowPosWeight;
                    break;
                default:
                    break;
            }
            return val;
        }

        public float GetFloat(AvatarIKHint goal, bool Internal)
        {
            float val = 0;
            if (Internal == true)
            {
                switch (goal)
                {
                    case AvatarIKHint.LeftKnee:
                        val = internalLeftKneeWeight;
                        break;
                    case AvatarIKHint.RightKnee:
                        val = internalRightKneeWeight;
                        break;
                    case AvatarIKHint.LeftElbow:
                        val = internalLeftElbowWeight;
                        break;
                    case AvatarIKHint.RightElbow:
                        val = internalRightElbowWeight;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (goal)
                {
                    case AvatarIKHint.LeftKnee:
                        val = leftKneePosWeight;
                        break;
                    case AvatarIKHint.RightKnee:
                        val = rightKneePosWeight;
                        break;
                    case AvatarIKHint.LeftElbow:
                        val = leftElbowPosWeight;
                        break;
                    case AvatarIKHint.RightElbow:
                        val = rightElbowPosWeight;
                        break;
                    default:
                        break;
                }
            }
            return val;
        }

        public Vector3 GetPosition(AvatarIKGoal goal)
        {
            Vector3 pos = Vector3.zero;
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    pos = leftFootPos;
                    break;
                case AvatarIKGoal.RightFoot:
                    pos = rightFootPos;
                    break;
                case AvatarIKGoal.LeftHand:
                    pos = leftHandPos;
                    break;
                case AvatarIKGoal.RightHand:
                    pos = rightHandPos;
                    break;
                default:
                    break;
            }
            return pos;
        }

        public Vector3 GetPosition(AvatarIKGoal goal, bool Internal)
        {
            Vector3 pos = Vector3.zero;
            if (Internal == true)
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        pos = internalLeftFootPos;
                        break;
                    case AvatarIKGoal.RightFoot:
                        pos = internalRightFootPos;
                        break;
                    case AvatarIKGoal.LeftHand:
                        pos = internalLeftHandPos;
                        break;
                    case AvatarIKGoal.RightHand:
                        pos = internalRightHandPos;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        pos = leftFootPos;
                        break;
                    case AvatarIKGoal.RightFoot:
                        pos = rightFootPos;
                        break;
                    case AvatarIKGoal.LeftHand:
                        pos = leftHandPos;
                        break;
                    case AvatarIKGoal.RightHand:
                        pos = rightHandPos;
                        break;
                    default:
                        break;
                }
            }
            return pos;
        }

        public Vector3 GetPosition(AvatarIKHint goal)
        {
            Vector3 pos = Vector3.zero;
            switch (goal)
            {
                case AvatarIKHint.LeftKnee:
                    pos = leftKneePos;
                    break;
                case AvatarIKHint.RightKnee:
                    pos = rightKneePos;
                    break;
                case AvatarIKHint.LeftElbow:
                    pos = leftElbowPos;
                    break;
                case AvatarIKHint.RightElbow:
                    pos = rightElbowPos;
                    break;
                default:
                    break;
            }
            return pos;
        }

        public Vector3 GetPosition(AvatarIKHint goal, bool Internal)
        {
            Vector3 pos = Vector3.zero;
            if (Internal == true)
            {
                switch (goal)
                {
                    case AvatarIKHint.LeftKnee:
                        pos = internalLeftKneePos;
                        break;
                    case AvatarIKHint.RightKnee:
                        pos = internalRightKneePos;
                        break;
                    case AvatarIKHint.LeftElbow:
                        pos = internalLeftElbowPos;
                        break;
                    case AvatarIKHint.RightElbow:
                        pos = internalRightElbowPos;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (goal)
                {
                    case AvatarIKHint.LeftKnee:
                        pos = leftKneePos;
                        break;
                    case AvatarIKHint.RightKnee:
                        pos = rightKneePos;
                        break;
                    case AvatarIKHint.LeftElbow:
                        pos = leftElbowPos;
                        break;
                    case AvatarIKHint.RightElbow:
                        pos = rightElbowPos;
                        break;
                    default:
                        break;
                }
            }
            return pos;
        }

        public Quaternion GetRotation(AvatarIKGoal goal)
        {
            Quaternion rot = Quaternion.identity;
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    rot = leftFootRot;
                    break;
                case AvatarIKGoal.RightFoot:
                    rot = rightFootRot;
                    break;
                case AvatarIKGoal.LeftHand:
                    rot = leftHandRot;
                    break;
                case AvatarIKGoal.RightHand:
                    rot = rightHandRot;
                    break;
                default:
                    break;
            }
            return rot;
        }
        
        public Quaternion GetRotation(AvatarIKGoal goal, bool Internal)
        {
            Quaternion rot = Quaternion.identity;
            if (Internal)
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        rot = internalLeftFootRot;
                        break;
                    case AvatarIKGoal.RightFoot:
                        rot = internalRightFootRot;
                        break;
                    case AvatarIKGoal.LeftHand:
                        rot = internalLeftHandRot;
                        break;
                    case AvatarIKGoal.RightHand:
                        rot = internalRightHandRot;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        rot = leftFootRot;
                        break;
                    case AvatarIKGoal.RightFoot:
                        rot = rightFootRot;
                        break;
                    case AvatarIKGoal.LeftHand:
                        rot = leftHandRot;
                        break;
                    case AvatarIKGoal.RightHand:
                        rot = rightHandRot;
                        break;
                    default:
                        break;
                }
            }
            return rot;
        }

        public Vector3 GetLookPosition()
        {
            return lookAtPosition;
        }

        public void SetFloat(float val, AvatarIKGoal goal, bool IsPos)
        {
            if (IsPos == true)
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        leftFootPosWeight = val;
                        break;
                    case AvatarIKGoal.RightFoot:
                        rightFootPosWeight = val;
                        break;
                    case AvatarIKGoal.LeftHand:
                        leftHandPosWeight = val;
                        break;
                    case AvatarIKGoal.RightHand:
                        rightHandPosWeight = val;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        leftFootRotWeight = val;
                        break;
                    case AvatarIKGoal.RightFoot:
                        rightFootRotWeight = val;
                        break;
                    case AvatarIKGoal.LeftHand:
                        leftHandRotWeight = val;
                        break;
                    case AvatarIKGoal.RightHand:
                        rightHandRotWeight = val;
                        break;
                    default:
                        break;
                }
            }
        }

        public void SetFloat(float val, AvatarIKHint goal)
        {
            switch (goal)
            {
                case AvatarIKHint.LeftKnee:
                    leftKneePosWeight = val;
                    break;
                case AvatarIKHint.RightKnee:
                    rightKneePosWeight = val;
                    break;
                case AvatarIKHint.LeftElbow:
                    leftElbowPosWeight = val;
                    break;
                case AvatarIKHint.RightElbow:
                    rightElbowPosWeight = val;
                    break;
                default:
                    break;
            }
        }

        public void SetPosition(Vector3 pos, AvatarIKGoal goal)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    leftFootPos = pos;
                    break;
                case AvatarIKGoal.RightFoot:
                    rightFootPos = pos;
                    break;
                case AvatarIKGoal.LeftHand:
                    leftHandPos = pos;
                    break;
                case AvatarIKGoal.RightHand:
                    rightHandPos = pos;
                    break;
                default:
                    break;
            }
        }

        public void SetPosition(Vector3 pos, AvatarIKHint goal)
        {
            switch (goal)
            {
                case AvatarIKHint.LeftKnee:
                    leftKneePos = pos;
                    break;
                case AvatarIKHint.RightKnee:
                    rightKneePos = pos;
                    break;
                case AvatarIKHint.LeftElbow:
                    leftElbowPos = pos;
                    break;
                case AvatarIKHint.RightElbow:
                    rightElbowPos = pos;
                    break;
                default:
                    break;
            }
        }

        public void SetRotation(Quaternion rot, AvatarIKGoal goal)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    leftFootRot = rot;
                    break;
                case AvatarIKGoal.RightFoot:
                    rightFootRot = rot;
                    break;
                case AvatarIKGoal.LeftHand:
                    leftHandRot = rot;
                    break;
                case AvatarIKGoal.RightHand:
                    rightHandRot = rot;
                    break;
                default:
                    break;
            }
        }

        public void SetLookPosition(Vector3 val)
        {
            lookAtPosition = val;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        void Fire()
        {
            Pedestrian.Pedestrian ped = GetComponentInParent<Pedestrian.Pedestrian>();
            if (ped != null)
            {
                ped.Fire();
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (animator != null)
            {
                animator.SetLookAtWeight(lookAtWeight, lookAtBodyWeight, lookAtHeadWeight, lookAtEyesWeight, lookAtClampWeight);
                animator.SetLookAtPosition(lookAtPosition);

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandPosWeight);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandPosWeight);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPosWeight);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPosWeight);

                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandRotWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotWeight);

                animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightElbowPosWeight);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftElbowPosWeight);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, rightKneePosWeight);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, leftKneePosWeight);

                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);

                animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowPos);
                animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowPos);
                animator.SetIKHintPosition(AvatarIKHint.RightKnee, rightKneePos);
                animator.SetIKHintPosition(AvatarIKHint.LeftKnee, leftKneePos);
                
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);

                internalLeftFootPos = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
                internalRightFootPos = animator.GetIKPosition(AvatarIKGoal.RightFoot);
                internalLeftHandPos = animator.GetIKPosition(AvatarIKGoal.LeftHand);
                internalRightHandPos = animator.GetIKPosition(AvatarIKGoal.RightHand);

                internalLeftFootRot = animator.GetIKRotation(AvatarIKGoal.LeftFoot);
                internalRightFootRot = animator.GetIKRotation(AvatarIKGoal.RightFoot);
                internalLeftHandRot = animator.GetIKRotation(AvatarIKGoal.LeftHand);
                internalRightHandRot = animator.GetIKRotation(AvatarIKGoal.RightHand);

                internalLeftKneePos = animator.GetIKHintPosition(AvatarIKHint.LeftKnee);
                internalRightKneePos = animator.GetIKHintPosition(AvatarIKHint.RightKnee);
                internalLeftElbowPos = animator.GetIKHintPosition(AvatarIKHint.LeftElbow);
                internalRightElbowPos = animator.GetIKHintPosition(AvatarIKHint.RightElbow);


                internalLeftFootPosWeight = animator.GetIKPositionWeight(AvatarIKGoal.LeftFoot);
                internalRightFootPosWeight = animator.GetIKPositionWeight(AvatarIKGoal.RightFoot);
                internalLeftHandPosWeight = animator.GetIKPositionWeight(AvatarIKGoal.LeftHand);
                internalRightHandPosWeight = animator.GetIKPositionWeight(AvatarIKGoal.RightHand);

                internalLeftFootRotWeight = animator.GetIKRotationWeight(AvatarIKGoal.LeftFoot);
                internalRightFootRotWeight = animator.GetIKRotationWeight(AvatarIKGoal.RightFoot);
                internalLeftHandRotWeight = animator.GetIKRotationWeight(AvatarIKGoal.LeftHand);
                internalRightHandRotWeight = animator.GetIKRotationWeight(AvatarIKGoal.RightHand);

                internalLeftKneeWeight = animator.GetIKHintPositionWeight(AvatarIKHint.LeftKnee);
                internalRightKneeWeight = animator.GetIKHintPositionWeight(AvatarIKHint.RightKnee);
                internalLeftElbowWeight = animator.GetIKHintPositionWeight(AvatarIKHint.LeftElbow);
                internalRightElbowWeight = animator.GetIKHintPositionWeight(AvatarIKHint.RightElbow);
            }
        }
    }
}