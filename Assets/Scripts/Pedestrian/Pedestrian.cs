using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pedestrian
{
    [RequireComponent(typeof(CharacterController))]
    public class Pedestrian : MonoBehaviour
    {
        [SerializeField] private AnimatorController ThirdPersonAnimator;
        [SerializeField] private AnimatorController FirstPersonAnimator;
        [SerializeField] private float mass = 20;
        private CharacterController charControl;
        private Vector3 HeadLook;
        [SerializeField] private Weapon.WeaponBehaviour weapon;
        [SerializeField] private float Health = 100;
        private float locomotionalStateVelocity;
        [SerializeField] private float locomotionalDampSpeed = .36f;
        [SerializeField] private float pushPower = 4.0f;
        [SerializeField] private bool IsFirstPerson = false;
        [SerializeField] private Vector3 FirstPersonOffset;
        [SerializeField] private bool SpineRotate;

        private void Start()
        {
            FirstPersonAnimator = transform.Find("FirstPerson").GetComponent<AnimatorController>();
            ThirdPersonAnimator = transform.Find("ThirdPerson").GetComponent<AnimatorController>();
            charControl = GetComponent<CharacterController>();
        }

        public Animator GetAnimator()
        {
            return ThirdPersonAnimator.GetAnimator();
        }

        public bool IsFPS()
        {
            return IsFirstPerson;
        }

        public void SetFPS(bool val)
        {
            IsFirstPerson = val;
        }

        public void SpineRotation()
        {
            if (SpineRotate == true)
            {
                GetAnimator(true).GetBoneTransform(HumanBodyBones.Spine).LookAt(HeadLook);
                GetAnimator(true).GetBoneTransform(HumanBodyBones.Spine).Rotate(FirstPersonOffset);
                GetAnimator(false).GetBoneTransform(HumanBodyBones.Spine).LookAt(HeadLook);
                GetAnimator(false).GetBoneTransform(HumanBodyBones.Spine).Rotate(FirstPersonOffset);

                if (GetAnimator(true).GetBoneTransform(HumanBodyBones.Head).gameObject.name == "Head")
                {
                    GetAnimator(true).GetBoneTransform(HumanBodyBones.Head).LookAt(HeadLook);
                    GetAnimator(false).GetBoneTransform(HumanBodyBones.Head).LookAt(HeadLook);
                }
                else
                {
                    GetAnimator(true).GetBoneTransform(HumanBodyBones.Head).Find("Head").LookAt(HeadLook);
                    GetAnimator(false).GetBoneTransform(HumanBodyBones.Head).Find("Head").LookAt(HeadLook);
                }
            }
        }

        public void FirstPersonMode()
        {
            if (GetAnimator(true) != null && GetAnimator(false) != null)
            {
                if (IsFPS() == true)
                {
                    if (GetComponent<PedestrianController>() != null)
                    {
                        if (GetComponent<PedestrianController>().IsRagdolled() == false)
                        {

                        }
                    }

                    SkinnedMeshRenderer[] FPSskinMeshes = GetAnimator(true).GetComponentsInChildren<SkinnedMeshRenderer>();
                    SkinnedMeshRenderer[] TPSskinMeshes = GetAnimator(false).GetComponentsInChildren<SkinnedMeshRenderer>();

                    if (weapon != null)
                    {
                        MeshRenderer[] wepMesh = weapon.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer mesh in wepMesh)
                        {
                            mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                        }
                    }

                    foreach (SkinnedMeshRenderer skinmesh in FPSskinMeshes)
                    {
                        skinmesh.enabled = IsFPS();
                    }

                    foreach(SkinnedMeshRenderer skinmesh in TPSskinMeshes)
                    {
                        skinmesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                    }
                }
                else
                {
                    SkinnedMeshRenderer[] FPSskinMeshes = GetAnimator(true).GetComponentsInChildren<SkinnedMeshRenderer>();
                    SkinnedMeshRenderer[] TPSskinMeshes = GetAnimator(false).GetComponentsInChildren<SkinnedMeshRenderer>();

                    if (weapon != null)
                    {
                        MeshRenderer[] wepMesh = weapon.GetComponentsInChildren<MeshRenderer>();
                        foreach(MeshRenderer mesh in wepMesh)
                        {
                            mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        }
                        if (GetAnimator(false).GetBoneTransform(HumanBodyBones.RightHand).GetComponentInChildren<Weapon.WeaponBehaviour>() != null)
                        {

                        }
                    }

                    foreach (SkinnedMeshRenderer skinmesh in FPSskinMeshes)
                    {
                        skinmesh.enabled = IsFPS();
                    }

                    foreach (SkinnedMeshRenderer skinmesh in TPSskinMeshes)
                    {
                        skinmesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }
                }
            }
        }

        public AnimatorController GetAnimatorController()
        {
            return ThirdPersonAnimator;
        }

        public AnimatorController GetAnimatorController(bool IsFirstPerson)
        {
            if (IsFirstPerson)
            {
                return FirstPersonAnimator;
            }
            else
            {
                return ThirdPersonAnimator;
            }
        }

        public void SetIKSystem()
        {
            if (GetAnimator(true) != null)
            {
                //GetAnimatorController(true).SetFloat(1, AvatarIKGoal.RightFoot, true);
                //GetAnimatorController(true).SetFloat(1, AvatarIKGoal.LeftFoot, true);
                //GetAnimatorController(true).SetFloat(1, AvatarIKGoal.RightFoot, false);
                //GetAnimatorController(true).SetFloat(1, AvatarIKGoal.LeftFoot, false);
                //GetAnimatorController(true).SetPosition(GetAnimatorController(false).GetPosition(AvatarIKGoal.RightFoot, true), AvatarIKGoal.RightFoot);
                //GetAnimatorController(true).SetPosition(GetAnimatorController(false).GetPosition(AvatarIKGoal.LeftFoot, true), AvatarIKGoal.LeftFoot);
            }

            if (GetAnimator(false) != null)
            {
            //    GetAnimatorController(false).SetFloat(1, AvatarIKGoal.LeftHand, true);
            //    GetAnimatorController(false).SetPosition(weapon.GetOwnerValue().GetLeftHandSolver().position, AvatarIKGoal.LeftHand);
            //    GetAnimatorController(false).SetRotation(weapon.GetOwnerValue().GetLeftHandSolver().rotation, AvatarIKGoal.LeftHand);
            }
        }

        public Animator GetAnimator(bool IsFirstPerson)
        {
            if (IsFirstPerson == true)
            {
                return FirstPersonAnimator.GetAnimator();
            }
            else
            {
                return ThirdPersonAnimator.GetAnimator();
            }
        }

        public Vector3 GetHeadLookPosition()
        {
            return HeadLook;
        }

        public void SetHeadLookPosition(Vector3 val)
        {
            HeadLook = val;
        }

        public float GetHealth()
        {
            return Health;
        }

        public void SetHealth(float val)
        {
            Health = val;
        }

        public void RagdollSystem(bool value)
        {
            if (value == true)
            {
                GetAnimator(false).enabled = !value;
                GetAnimator(true).enabled = !value;
                charControl.enabled = !value;
                transform.position += GetAnimator(false).GetBoneTransform(HumanBodyBones.Hips).localPosition;
                GetAnimator(false).GetBoneTransform(HumanBodyBones.Hips).localPosition = Vector3.zero;
                transform.eulerAngles += Vector3.up * GetAnimator(false).GetBoneTransform(HumanBodyBones.Hips).localEulerAngles.y;
                GetAnimator(false).GetBoneTransform(HumanBodyBones.Hips).localEulerAngles = new Vector3(GetAnimator(false).GetBoneTransform(HumanBodyBones.Hips).localEulerAngles.x, 0, GetAnimator(false).GetBoneTransform(HumanBodyBones.Hips).localEulerAngles.z);

                for (int i = 0; i < Enum.GetValues(typeof(HumanBodyBones)).Length; i++)
                {
                    if (GetAnimator(true).GetBoneTransform((HumanBodyBones)i) != null)
                    {
                        if (GetAnimator(true).GetBoneTransform((HumanBodyBones)i).GetComponent<Rigidbody>() != null)
                        {
                            GetAnimator(true).GetBoneTransform((HumanBodyBones)i).GetComponent<Rigidbody>().isKinematic = !value;
                        }

                        GetAnimator(true).GetBoneTransform((HumanBodyBones)i).position = GetAnimator(false).GetBoneTransform((HumanBodyBones)i).position;
                        GetAnimator(true).GetBoneTransform((HumanBodyBones)i).rotation = GetAnimator(false).GetBoneTransform((HumanBodyBones)i).rotation;
                    }
                }
            }
            else
            {
                GetAnimator(false).enabled = !value;
                GetAnimator(true).enabled = !value;
                charControl.enabled = !value;
                for (int i = 0; i < Enum.GetValues(typeof(HumanBodyBones)).Length; i++)
                {
                    if (GetAnimator(true).GetBoneTransform((HumanBodyBones)i) != null)
                    {
                        if (GetAnimator(true).GetBoneTransform((HumanBodyBones)i).GetComponent<Rigidbody>() != null)
                        {
                            GetAnimator(true).GetBoneTransform((HumanBodyBones)i).GetComponent<Rigidbody>().isKinematic = !value;
                        }
                    }
                }
            }
        }

        public void GettingUpSystem()
        {
            GetAnimator().SetBool("RagdollBelly", false);
            GetAnimator().SetBool("RagdollBack", false);

            GetAnimator(true).SetBool("RagdollBelly", false);
            GetAnimator(true).SetBool("RagdollBack", false);

            if (GetComponent<PedestrianController>().IsRagdolled())
            {
                if (GetAnimator().GetBoneTransform(HumanBodyBones.Hips).forward.y > 0)
                {
                    GetAnimator().SetBool("RagdollBack", true);
                    GetAnimator(true).SetBool("RagdollBack", true);
                }
                else
                {
                    GetAnimator().SetBool("RagdollBelly", true);
                    GetAnimator(true).SetBool("RagdollBelly", true);
                }
            }
        }

        public void Fire()
        {
            if (weapon != null)
            {
                weapon.Fire();
            }
        }

        public void Move(float forward, float sideway, float turn, ushort locomotionalState)
        {
            if (GetAnimator() != null)
            {
                GetAnimator().SetFloat("Forward", forward);
                GetAnimator().SetFloat("Sideway", sideway);
                GetAnimator().SetFloat("LocomotionalState", Mathf.SmoothDamp(GetAnimator().GetFloat("LocomotionalState"), locomotionalState, ref locomotionalStateVelocity, locomotionalDampSpeed));
                GetAnimator().SetFloat("Turn", turn);
            }

            if (GetAnimator(true) != null)
            {
                GetAnimator(true).SetFloat("Forward", GetAnimator().GetFloat("Forward"));
                GetAnimator(true).SetFloat("Sideway", GetAnimator().GetFloat("Sideway"));
                GetAnimator(true).SetFloat("Speed", new Vector2(GetAnimator().GetFloat("Sideway"), GetAnimator().GetFloat("Forward")).magnitude);
                GetAnimator(true).SetFloat("LocomotionalState", GetAnimator().GetFloat("LocomotionalState"));
                GetAnimator(true).SetFloat("Turn", GetAnimator().GetFloat("Turn"));
            }

            if (charControl != null)
            {
                if (GetAnimator(false) != null)
                {
                    Vector3 move = Vector3.zero;
                    
                    if (charControl.enabled)
                    {
                        if (charControl.isGrounded)
                        {
                            move = GetAnimator(false).deltaPosition;
                        }
                        else
                        {
                            move = new Vector3(GetAnimator(false).deltaPosition.x, mass * Physics.gravity.y * Time.deltaTime, GetAnimator(false).deltaPosition.z);
                        }
                        charControl.Move(move);
                        transform.rotation *= GetAnimator(false).deltaRotation;
                    }
                    if (GetAnimator(true) != null)
                    {
                        GetAnimator(true).rootPosition = GetAnimator(false).rootPosition;
                        GetAnimator(true).rootRotation = GetAnimator(false).rootRotation;
                    }
                }
            }
        }

        public void WeaponManager(ushort behaviour)
        {
            if (weapon != null)
            {
                if (weapon.GetOwnerBehaviorValue().IsOwned() == false)
                {
                    weapon = null;
                }
                else
                {
                    if (IsFPS())
                    {
                        weapon.transform.parent = GetAnimator(true).GetBoneTransform(HumanBodyBones.RightHand);
                        weapon.transform.localPosition = weapon.GetOwnerBehaviorValue().GetLocalPosition();
                        weapon.transform.localEulerAngles = weapon.GetOwnerBehaviorValue().GetLocalEulerAngles();
                    }
                    else
                    {
                        weapon.transform.parent = GetAnimator().GetBoneTransform(HumanBodyBones.RightHand);
                        weapon.transform.localPosition = weapon.GetOwnerBehaviorValue().GetLocalPosition();
                        weapon.transform.localEulerAngles = weapon.GetOwnerBehaviorValue().GetLocalEulerAngles();
                    }
                }
            }

            if (GetComponent<PedestrianController>().IsRagdolled() == false)
            {
                if (weapon != null)
                {
                    if (GetAnimator() != null)
                    {
                        GetAnimator().SetFloat("AimingTypeUpperLayer", weapon.GetOwnerValue().GetAimingTypeValue());
                        GetAnimator().SetFloat("FireAnimWeight", weapon.GetOwnerValue().GetFiringWeight());
                        GetAnimator().SetFloat("FiringSpeed", weapon.GetFireArmSystem().GetFireRate());
                        GetAnimator().SetInteger("BehaviouralState", behaviour);
                    }

                    if (GetAnimator(true) != null)
                    {
                        GetAnimator(true).SetInteger("BehaviouralState", GetAnimator().GetInteger("BehaviouralState"));
                        GetAnimator(true).SetFloat("DefaultType", weapon.GetOwnerValue(true).GetDefaultTypeValue());
                        GetAnimator(true).SetFloat("AimingType", weapon.GetOwnerValue(true).GetAimingTypeValue());
                    }

                    switch (behaviour)
                    {
                        case (ushort)PedestrianController.BehaviouralState.Default:

                            GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 0, Time.deltaTime * 3f));

                            if (SpineRotate == true)
                            {
                                //GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 3f));
                            }
                            else
                            {
                                //GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 0, Time.deltaTime * 3f));
                            }
                            break;
                        case (ushort)PedestrianController.BehaviouralState.Aiming:
                            GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 4f));

                            // GetAnimator().GetBoneTransform(HumanBodyBones.Spine).LookAt(HeadLook);
                            // GetAnimator().GetBoneTransform(HumanBodyBones.Spine).Rotate(weapon.GetOwnerBehaviorValue().GetSpineRotation());

                            if (GetAnimator().GetCurrentAnimatorStateInfo(GetAnimator().GetLayerIndex("Upper Layer")).IsName("Aiming") || GetAnimator().GetAnimatorTransitionInfo(GetAnimator().GetLayerIndex("Upper Layer")).IsName("Aiming"))
                            {
                            }

                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(((HeadLook - transform.position).normalized).x, 0, (HeadLook - transform.position).normalized.z)), Time.deltaTime * 20);
                            break;
                        case (ushort)PedestrianController.BehaviouralState.Reloading:
                            GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 4f));
                            break;
                        case (ushort)PedestrianController.BehaviouralState.Attacking:
                            GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 4f));

                            //GetAnimator().GetBoneTransform(HumanBodyBones.Spine).LookAt(HeadLook);
                            //GetAnimator().GetBoneTransform(HumanBodyBones.Spine).Rotate(weapon.GetOwnerBehaviorValue().GetSpineRotation());

                            if (GetAnimator().GetCurrentAnimatorStateInfo(GetAnimator().GetLayerIndex("Upper Layer")).IsName("Firing") || GetAnimator().GetAnimatorTransitionInfo(GetAnimator().GetLayerIndex("Upper Layer")).IsName("Firing"))
                            {
                            }

                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(((HeadLook - transform.position).normalized).x, 0, (HeadLook - transform.position).normalized.z)), Time.fixedUnscaledDeltaTime * 20);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), 0);
                    GetAnimator().SetFloat("AimingTypeUpperLayer", 0);
                    GetAnimator().SetFloat("FireAnimWeight", 0);
                    GetAnimator().SetFloat("FiringSpeed", 1);
                    GetAnimator().SetInteger("BehaviouralState", 0);
                }
            }
            else
            {
                GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), 0);
                GetAnimator().SetFloat("AimingTypeUpperLayer", 0);
                GetAnimator().SetFloat("FireAnimWeight", 0);
                GetAnimator().SetFloat("FiringSpeed", 1);
                GetAnimator().SetInteger("BehaviouralState", 0);
            }
        }

        public Weapon.WeaponBehaviour GetWeapon()
        {
            if (weapon != null)
            {
                return weapon;
            }
            else
            {
                return null;
            }
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            PedestrianController hitPed = hit.collider.GetComponent<PedestrianController>();
            if (hitPed != null)
            {
                Vector3 dir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                hitPed.SetForward(Vector3.Dot(hitPed.transform.forward, dir));
                hitPed.SetSideway(Vector3.Dot(hitPed.transform.right, dir));
            }

            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;

            if (hit.moveDirection.y < -0.3F)
                return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = pushDir * pushPower;
        }
        
        public void CustomIK(Transform upperArm, Transform forearm, Transform hand, Transform target, Transform elbowTarget, float Weight)
        {
            Vector3 targetRelativeStartPosition, elbowTargetRelativeStartPosition;
            Quaternion upperArmStartRotation, forearmStartRotation, handStartRotation;
            GameObject upperArmAxisCorrection, forearmAxisCorrection, handAxisCorrection;
            Vector3 lastUpperArmPosition, lastTargetPosition, lastElbowTargetPosition;
            //assign the starting rotations
            upperArmStartRotation = upperArm.rotation;
            forearmStartRotation = forearm.rotation;
            handStartRotation = hand.rotation;

            //assign realtive starting positions
            elbowTargetRelativeStartPosition = elbowTarget.position - upperArm.position;

            //Create helper GOs
            upperArmAxisCorrection = new GameObject("upperArmAxisCorrection");
            forearmAxisCorrection = new GameObject("forearmAxisCorrection");
            handAxisCorrection = new GameObject("handAxisCorrection");

            //set helper hierarchy
            upperArmAxisCorrection.transform.parent = transform;
            forearmAxisCorrection.transform.parent = upperArmAxisCorrection.transform;
            handAxisCorrection.transform.parent = forearmAxisCorrection.transform;

            //guarantee first-frame update
            lastUpperArmPosition = upperArm.position + 5 * Vector3.up;


            //if we have no target then reset the relative position
            if (target == null)
            {
                targetRelativeStartPosition = Vector3.zero;
                return;
            }

            //if we have a target and the relative start position is zeroed
            //if (targetRelativeStartPosition == Vector3.zero && target != null)
            //{
            //targetRelativeStartPosition = target.position - upperArm.position;
            //}

            //save our positions
            lastUpperArmPosition = upperArm.position;
            lastTargetPosition = target.position;
            lastElbowTargetPosition = elbowTarget.position;

            //Calculate ikAngle variable which defines the angle that will be subtracted from the upperarm axis
            float upperArmLength = Vector3.Distance(upperArm.position, forearm.position);
            float forearmLength = Vector3.Distance(forearm.position, hand.position);

            //find the arm length
            float armLength = upperArmLength + forearmLength;
            //find the hypoethenuse,  is the longest side of a right-angled triangle 90o degrees angle
            float hypotenuse = upperArmLength;

            //find the distance betwen our upperarm and the target
            float targetDistance = Vector3.Distance(upperArm.position, target.position);

            //Do not allow target distance be further away than the arm's length.
            targetDistance = Mathf.Min(targetDistance, armLength - 0.0001f);

            //(of a pair of angles) formed on the same side of a straight line when intersected by another line.
            float adjacent = (hypotenuse * hypotenuse - forearmLength * forearmLength + targetDistance * targetDistance) / (2 * targetDistance);

            //find the ik Angle
            float ikAngle = Mathf.Acos(adjacent / hypotenuse) * Mathf.Rad2Deg;

            //Store pre-ik info.
            Vector3 targetPosition = target.position;
            Vector3 elbowTargetPosition = elbowTarget.position;

            //Store the parent of each bone
            Transform upperArmParent = upperArm.parent;
            Transform forearmParent = forearm.parent;
            Transform handParent = hand.parent;

            //Store the scale
            Vector3 upperArmScale = upperArm.localScale;
            Vector3 forearmScale = forearm.localScale;
            Vector3 handScale = hand.localScale;

            //Store the local positions
            Vector3 upperArmLocalPosition = upperArm.localPosition;
            Vector3 forearmLocalPosition = forearm.localPosition;
            Vector3 handLocalPosition = hand.localPosition;

            //Store the rotations
            Quaternion upperArmRotation = upperArm.rotation;
            Quaternion forearmRotation = forearm.rotation;
            Quaternion handRotation = hand.rotation;
            Quaternion handLocalRotation = hand.localRotation;

            //Reset arm so that the ik starts from a known postion
            //target.position = targetRelativeStartPosition + upperArm.position;
            elbowTarget.position = elbowTargetRelativeStartPosition + upperArm.position;
            upperArm.rotation = upperArmStartRotation;
            forearm.rotation = forearmStartRotation;
            hand.rotation = handStartRotation;

            //Work with temporaty game objects and align & parent them to the arm.
            transform.position = upperArm.position;
            //position the elbow using as an up axis a vector from the upperArm position to the target of the elbow
            //that will orient the elbow to the correct orientation
            transform.LookAt(targetPosition, elbowTargetPosition - transform.position);

            upperArmAxisCorrection.transform.position = upperArm.position;
            upperArmAxisCorrection.transform.LookAt(forearm.position, upperArm.up);
            upperArm.parent = upperArmAxisCorrection.transform;

            forearmAxisCorrection.transform.position = forearm.position;
            forearmAxisCorrection.transform.LookAt(hand.position, forearm.up);
            forearm.parent = forearmAxisCorrection.transform;

            handAxisCorrection.transform.position = hand.position;
            hand.parent = handAxisCorrection.transform;

            //Reset targets.
            target.position = targetPosition;
            elbowTarget.position = elbowTargetPosition;

            //Apply rotation for temporary game objects.
            upperArmAxisCorrection.transform.LookAt(target, elbowTarget.position - upperArmAxisCorrection.transform.position);
            upperArmAxisCorrection.transform.localRotation = Quaternion.Euler(upperArmAxisCorrection.transform.localRotation.eulerAngles - new Vector3(ikAngle, 0, 0));
            forearmAxisCorrection.transform.LookAt(target, elbowTarget.position - upperArmAxisCorrection.transform.position);
            handAxisCorrection.transform.rotation = target.rotation;

            //Restore limbs.
            upperArm.parent = upperArmParent;
            forearm.parent = forearmParent;
            hand.parent = handParent;
            upperArm.localScale = upperArmScale;
            forearm.localScale = forearmScale;
            hand.localScale = handScale;
            upperArm.localPosition = upperArmLocalPosition;
            forearm.localPosition = forearmLocalPosition;
            hand.localPosition = handLocalPosition;

            //Transition.
            Weight = Mathf.Clamp01(Weight);
            upperArm.rotation = Quaternion.Slerp(upperArmRotation, upperArm.rotation, Weight);
            forearm.rotation = Quaternion.Slerp(forearmRotation, forearm.rotation, Weight);
            hand.rotation = target.rotation;
        }
    }
}
