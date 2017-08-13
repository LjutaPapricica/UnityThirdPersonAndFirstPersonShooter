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
        [SerializeField] private bool HeadRotate;
        [SerializeField] [Range(0, 1)] private float maxPeakDistance;

        private void Start()
        {
            FirstPersonAnimator = transform.Find("FirstPerson").GetComponent<AnimatorController>();
            ThirdPersonAnimator = transform.Find("ThirdPerson").GetComponent<AnimatorController>();
            charControl = GetComponent<CharacterController>();
        }

        public bool GetHeadRotate()
        {
            return HeadRotate;
        }

        public bool GetSpineRotate()
        {
            return SpineRotate;
        }

        public void SetHeadRotate(bool val)
        {
            HeadRotate = val;
        }

        public void SetSpineRotate(bool val)
        {
            SpineRotate = val;
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

        public void BodyRotation(float peakDistance)
        {
            if (SpineRotate == true)
            {
                Vector3 SpineRotateTPS = GetAnimator(false).GetBoneTransform(HumanBodyBones.Spine).eulerAngles - transform.eulerAngles; // -Quaternion.FromToRotation(GetAnimator(false).GetBoneTransform(HumanBodyBones.Spine).forward, transform.forward).eulerAngles;
                //Vector3 SpineRotateFPS = GetAnimator(true).GetBoneTransform(HumanBodyBones.Spine).eulerAngles - transform.eulerAngles; // -Quaternion.FromToRotation(GetAnimator(true).GetBoneTransform(HumanBodyBones.Spine).forward, transform.forward).eulerAngles;
                
                GetAnimator(true).GetBoneTransform(HumanBodyBones.Spine).rotation = Quaternion.LookRotation((HeadLook - GetAnimator(true).GetBoneTransform(HumanBodyBones.Spine).position).normalized, transform.up + transform.right * (peakDistance * maxPeakDistance));
                GetAnimator(false).GetBoneTransform(HumanBodyBones.Spine).rotation = Quaternion.LookRotation((HeadLook - GetAnimator(false).GetBoneTransform(HumanBodyBones.Spine).position).normalized, transform.up + transform.right * (peakDistance * maxPeakDistance));
                GetAnimator(true).GetBoneTransform(HumanBodyBones.Spine).Rotate(weapon != null ? weapon.GetFirstPersonSystem().GetSpineRotation() : FirstPersonOffset);
                
                if (GetAnimator(false).GetInteger("BehaviouralState") == (int)PedestrianController.BehaviouralState.Aiming || GetAnimator(false).GetInteger("BehaviouralState") == (int)PedestrianController.BehaviouralState.Attacking)
                {
                    GetAnimator(false).GetBoneTransform(HumanBodyBones.Spine).Rotate(weapon != null ? weapon.GetThirdPersonSystem().GetSpineRotation() : FirstPersonOffset);
                }
                else
                {
                    GetAnimator(false).GetBoneTransform(HumanBodyBones.Spine).Rotate(SpineRotateTPS);
                }

            }

            if (HeadRotate == true)
            {
                GetHeadBone().rotation = Quaternion.LookRotation((HeadLook - GetHeadBone().position).normalized, transform.up + transform.right * (peakDistance * maxPeakDistance));
                GetHeadBone(true).rotation = Quaternion.LookRotation((HeadLook - GetHeadBone(false).position).normalized, transform.up + transform.right * (peakDistance * maxPeakDistance));

                //GetHeadBone().localRotation = Quaternion.RotateTowards(Quaternion.identity, GetHeadBone().rotation, 80); //GetHeadBone().localEulerAngles = new Vector3(Mathf.Clamp(GetHeadBone().localEulerAngles.x, -70f, 70f), Mathf.Clamp(GetHeadBone().localEulerAngles.y, -80f, 80f), Mathf.Clamp(GetHeadBone().localEulerAngles.x, -45f, 45f));
                //GetHeadBone(true).localRotation = Quaternion.RotateTowards(Quaternion.identity, GetHeadBone(true).rotation, 80);// new Vector3(Mathf.Clamp(GetHeadBone().localEulerAngles.x, -70f, 70f), Mathf.Clamp(GetHeadBone().localEulerAngles.y, -80f, 80f), Mathf.Clamp(GetHeadBone().localEulerAngles.x, -45f, 45f));

            }
        }

        public Transform GetHeadBone()
        {
            if (GetAnimator().GetBoneTransform(HumanBodyBones.Head).gameObject.name == "Head")
            {
                return GetAnimator().GetBoneTransform(HumanBodyBones.Head);
            }
            else
            {
                return GetAnimator().GetBoneTransform(HumanBodyBones.Head).Find("Head");
            }
        }

        public Transform GetHeadBone(bool IsFPS)
        {
            if (GetAnimator(IsFPS).GetBoneTransform(HumanBodyBones.Head).gameObject.name == "Head")
            {
                return GetAnimator(IsFPS).GetBoneTransform(HumanBodyBones.Head);
            }
            else
            {
                return GetAnimator(IsFPS).GetBoneTransform(HumanBodyBones.Head).Find("Head");
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
                Collider[] wepCol = weapon.GetComponent<Rigidbody>().GetComponents<Collider>();
                foreach (Collider col in wepCol)
                {
                    col.isTrigger = true;
                }

                if (GetAnimator(false) != null)
                {
                    GetAnimator(false).enabled = !value;
                    GetAnimator(false).GetBoneTransform(HumanBodyBones.Hips).localPosition = Vector3.zero;
                }
                if (GetAnimator(true) != null)
                {
                    GetAnimator(true).enabled = !value;
                }
                if (charControl != null)
                {
                    charControl.enabled = !value;
                }

                transform.position += GetAnimator(false).GetBoneTransform(HumanBodyBones.Hips).localPosition;
            }
            else
            {
                Collider[] wepCol = weapon.GetComponent<Rigidbody>().GetComponents<Collider>();
                foreach (Collider col in wepCol)
                {
                    col.isTrigger = false;
                }

                if (GetAnimator(false) != null)
                {
                    GetAnimator(false).enabled = !value;
                }
                if (GetAnimator(true) != null)
                {
                    GetAnimator(true).enabled = !value;
                }
                if (charControl != null)
                {
                    charControl.enabled = !value;
                }
                
            }

            if (GetAnimator(true) != null)
            {
                if (IsAnimationInPlayableState("RagdollBelly", "Base Layer"))
                {
                    GetAnimator(true).CrossFade("RagdollBelly", 0f, GetAnimator(true).GetLayerIndex("Base Layer"), GetAnimator().GetCurrentAnimatorStateInfo(GetAnimator().GetLayerIndex("Base Layer")).normalizedTime);
                }
                else if (IsAnimationInPlayableState("RagdollBack", "Base Layer"))
                {
                    GetAnimator(true).CrossFade("RagdollBack", 0f, GetAnimator(true).GetLayerIndex("Base Layer"), GetAnimator().GetCurrentAnimatorStateInfo(GetAnimator().GetLayerIndex("Base Layer")).normalizedTime);
                    //GetAnimator(true).Play("", GetAnimator(true).GetLayerIndex("Base Layer"));
                }

                if ( GetAnimator(true).enabled == true)
                {
                    for (int i = 0; i < Enum.GetValues(typeof(HumanBodyBones)).Length; i++)
                    {
                        if (GetAnimator(true).GetBoneTransform((HumanBodyBones)i) != null)
                        {
                            if (GetAnimator(true).GetBoneTransform((HumanBodyBones)i).GetComponent<Rigidbody>() != null)
                            {
                                GetAnimator(true).GetBoneTransform((HumanBodyBones)i).GetComponent<Rigidbody>().isKinematic = true;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Enum.GetValues(typeof(HumanBodyBones)).Length; i++)
                    {
                        if (GetAnimator(true).GetBoneTransform((HumanBodyBones)i) != null)
                        {
                            if (GetAnimator(true).GetBoneTransform((HumanBodyBones)i).GetComponent<Rigidbody>() != null)
                            {
                                GetAnimator(true).GetBoneTransform((HumanBodyBones)i).GetComponent<Rigidbody>().isKinematic = false;
                            }

                            GetAnimator(true).GetBoneTransform((HumanBodyBones)i).position = GetAnimator(false).GetBoneTransform((HumanBodyBones)i).position;
                            GetAnimator(true).GetBoneTransform((HumanBodyBones)i).rotation = GetAnimator(false).GetBoneTransform((HumanBodyBones)i).rotation;
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
                            RaycastHit hit;
                            Ray ray = new Ray(transform.position, -transform.up);
                            if (Physics.Raycast(ray, out hit, 1f))
                            {

                            }
                        }
                        else
                        {
                            move = new Vector3(GetAnimator(false).deltaPosition.x, mass * Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2), GetAnimator(false).deltaPosition.z);
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

        public bool IsAnimationInPlayableState(string state, string layer)
        {
            return GetAnimator().GetCurrentAnimatorStateInfo(GetAnimator().GetLayerIndex(layer)).IsName(state) || GetAnimator().GetAnimatorTransitionInfo(GetAnimator().GetLayerIndex(layer)).IsName(state);
        }

        public bool IsAnimationInPlayableState(string state, string layer, bool IsFPS)
        {
            return GetAnimator(IsFPS).GetCurrentAnimatorStateInfo(GetAnimator().GetLayerIndex(layer)).IsName(state) || GetAnimator(IsFPS).GetAnimatorTransitionInfo(GetAnimator().GetLayerIndex(layer)).IsName(state);
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
                    weapon.GetComponent<Rigidbody>().useGravity = false;
                    if (IsFPS() == false)
                    {
                        weapon.transform.parent = GetAnimator(false).GetBoneTransform(HumanBodyBones.RightHand);
                        weapon.transform.localPosition = weapon.GetOwnerBehaviorValue().GetLocalPosition();
                        weapon.transform.localEulerAngles = weapon.GetOwnerBehaviorValue().GetLocalEulerAngles();
                    }
                    else
                    {
                        weapon.transform.parent = GetAnimator(true).GetBoneTransform(HumanBodyBones.RightHand);
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
                        switch (behaviour)
                        {
                            case (ushort)PedestrianController.BehaviouralState.Default:

                                if (IsAnimationInPlayableState("Reloading", "Upper Layer")/*GetAnimator().GetCurrentAnimatorStateInfo(GetAnimator().GetLayerIndex()).IsName() || GetAnimator().GetAnimatorTransitionInfo(GetAnimator().GetLayerIndex("Upper Layer")).IsName("Reloading")*/)
                                {
                                    GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 4f));
                                }
                                else
                                {

                                    GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 0, Time.deltaTime * 3f));
                                }
                                break;
                            case (ushort)PedestrianController.BehaviouralState.Aiming:
                                if (GetAnimator(false).GetBool("IsRelaxed"))
                                {

                                }
                                else
                                {
                                    GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 4f));


                                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(((HeadLook - transform.position).normalized).x, 0, (HeadLook - transform.position).normalized.z)), Time.deltaTime * 20);

                                }

                                break;
                            case (ushort)PedestrianController.BehaviouralState.Reloading:
                                GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 4f));
                                break;
                            case (ushort)PedestrianController.BehaviouralState.Attacking:
                                if (GetAnimator(false).GetBool("IsRelaxed"))
                                {

                                }
                                else
                                {
                                    if (GetWeapon().GetFireArmSystem().GetAmmo().GetClipSize() > 0)
                                    {
                                        behaviour = (ushort)PedestrianController.BehaviouralState.Attacking;
                                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(((HeadLook - transform.position).normalized).x, 0, (HeadLook - transform.position).normalized.z)), Time.deltaTime * 20);

                                        GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 4f));
                                    }
                                    else
                                    {
                                        if (IsFPS() == true)
                                        {
                                            behaviour = (ushort)PedestrianController.BehaviouralState.Default;
                                        }
                                        else
                                        {
                                            behaviour = (ushort)PedestrianController.BehaviouralState.Aiming;
                                        }
                                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(((HeadLook - transform.position).normalized).x, 0, (HeadLook - transform.position).normalized.z)), Time.deltaTime * 20);


                                        GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 4f));

                                        if (GetAnimator().GetCurrentAnimatorStateInfo(GetAnimator().GetLayerIndex("Upper Layer")).IsName("Firing") || GetAnimator().GetAnimatorTransitionInfo(GetAnimator().GetLayerIndex("Upper Layer")).IsName("Firing"))
                                        {
                                        }

                                    }

                                }
                                break;
                            case (ushort)PedestrianController.BehaviouralState.Relaxed:
                                if (weapon.GetThirdPersonSystem().GetBoolUseUpperLayerRelaxedAnimation() == true)
                                {
                                    GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 3f));
                                    GetAnimator(true).SetFloat("RelaxedType", weapon.GetFirstPersonSystem().GetRelaxedType());
                                }
                                else
                                {
                                    if (IsAnimationInPlayableState("Reloading", "Upper Layer"))
                                    {

                                        GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 1, Time.deltaTime * 3f));
                                    }
                                    else
                                    {
                                        GetAnimator().SetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer"), Mathf.Lerp(GetAnimator().GetLayerWeight(GetAnimator().GetLayerIndex("Upper Layer")), 0, Time.deltaTime * 3f));
                                    }
                                    GetAnimator(true).SetFloat("RelaxedType", weapon.GetFirstPersonSystem().GetDefaultRelaxedType());
                                }
                                break;
                            default:
                                break;
                        }

                        GetAnimator().SetFloat("AimingTypeUpperLayer", weapon.GetThirdPersonSystem().GetAimingTypeValue());
                        GetAnimator().SetFloat("FireAnimWeight", weapon.GetThirdPersonSystem().GetFiringWeight());
                        GetAnimator().SetFloat("FiringSpeed", weapon.GetThirdPersonSystem().GetFireRate());
                        GetAnimator().SetInteger("BehaviouralState", behaviour);
                        GetAnimator().SetBool("IsRelaxed", weapon.GetThirdPersonSystem().GetBoolUseUpperLayerRelaxedAnimation());

                        if (GetAnimator(true) != null)
                        {
                            GetAnimator(true).SetInteger("BehaviouralState", GetAnimator().GetInteger("BehaviouralState"));
                            GetAnimator(true).SetFloat("DefaultType", weapon.GetFirstPersonSystem().GetDefaultTypeValue());
                            GetAnimator(true).SetFloat("AimingType", weapon.GetFirstPersonSystem().GetAimingTypeValue());
                            GetAnimator(true).SetFloat("DefaultFiringType", weapon.GetFirstPersonSystem().GetDefaultFiringTypeValue());
                            GetAnimator(true).SetFloat("AimingFiringType", weapon.GetFirstPersonSystem().GetAimingFiringTypeValue());
                            GetAnimator(true).SetFloat("FiringSpeed", weapon.GetFirstPersonSystem().GetFireRate());
                            GetAnimator(true).SetBool("IsRelaxed", GetAnimator().GetBool("IsRelaxed"));

                            if (IsAnimationInPlayableState("RagdollBack", "Base Layer") || IsAnimationInPlayableState("RagdollBelly", "Base Layer"))
                            {
                                GetAnimator(true).SetLayerWeight(GetAnimator(true).GetLayerIndex("Lower Layer"), Mathf.Lerp(GetAnimator(true).GetLayerWeight(GetAnimator(true).GetLayerIndex("Lower Layer")), 0, Time.deltaTime * 3f));
                            }
                            else
                            {

                                GetAnimator(true).SetLayerWeight(GetAnimator(true).GetLayerIndex("Lower Layer"), Mathf.Lerp(GetAnimator(true).GetLayerWeight(GetAnimator(true).GetLayerIndex("Lower Layer")), 1, Time.deltaTime * 3f));
                            }
                        }
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

        public void Reload()
        {
            if (weapon != null)
            {
                weapon.ReloadLogic();
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
    }
}
