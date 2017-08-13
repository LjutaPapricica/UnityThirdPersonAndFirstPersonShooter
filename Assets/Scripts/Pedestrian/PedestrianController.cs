using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pedestrian
{
    [RequireComponent(typeof(Pedestrian))]
    public class PedestrianController : MonoBehaviour
    {
        private Pedestrian Ped;
        [SerializeField] [Range(-1, 1)] private float Forward;
        [SerializeField] [Range(-1, 1)] private float Sideway;
        public enum BehaviouralState { Default, Aiming, Attacking, Reloading, Relaxed }
        [SerializeField] private BehaviouralState behaviour;
        public enum LocomotionalState { Crouch, Standing}
        [SerializeField] private LocomotionalState Locomotion;
        [SerializeField] [Range(-90, 90)] private float Turn;
        [SerializeField] private bool ragdoll = false;
        [SerializeField] private Vector3 dir;
        [SerializeField] private float peakDistance;
        private float peakAxis;

        private void Start()
        {
            Ped = GetComponent<Pedestrian>();
        }

        // Update is called once per frame
        private void Update()
        {
            Ped.GettingUpSystem();
            switch (behaviour)
            {
                case BehaviouralState.Default:
                    Ped.GetWeapon().GetFireArmSystem().SetFirePointDirection(Ped.GetWeapon().transform.forward);
                    break;
                case BehaviouralState.Aiming:
                    Ped.GetWeapon().GetFireArmSystem().SetFirePointDirection(dir);
                    break;
                case BehaviouralState.Attacking:
                    Ped.GetWeapon().GetFireArmSystem().SetFirePointDirection(dir);
                    break;
                case BehaviouralState.Reloading:
                    Ped.GetWeapon().GetFireArmSystem().SetFirePointDirection(Ped.GetWeapon().transform.forward);
                    break;
                case BehaviouralState.Relaxed:
                    Ped.GetWeapon().GetFireArmSystem().SetFirePointDirection(Ped.GetWeapon().transform.forward);
                    break;
                default:
                    break;
            }
        }

        private void LateUpdate()
        {
            Ped.WeaponManager((ushort)behaviour);
            Ped.RagdollSystem(ragdoll);
            Ped.FirstPersonMode();
            BodyMovements(peakAxis);
        }

        private void FixedUpdate()
        {
            Ped.Move(Forward, Sideway, Turn, (ushort)Locomotion);
            Ped.SetIKSystem();
            Ped.GetAnimatorController().SetLookPosition(Ped.GetHeadLookPosition());
        }

        public void SetPeakAxis(float val)
        {
            peakAxis = val;
        }

        private void BodyMovements(float axis)
        {
            Ped.BodyRotation(peakDistance);
            if (GetBehavioralState() == (ushort)BehaviouralState.Aiming || GetBehavioralState() == (ushort)BehaviouralState.Attacking)
            {
                if (axis < 0)
                {
                    SetPeaking(Mathf.Lerp(GetPeakingDistance(), -1, Time.deltaTime * 5f));
                }
                else if (axis > 0)
                {
                    SetPeaking(Mathf.Lerp(GetPeakingDistance(), 1, Time.deltaTime * 5f));
                }
                else
                {
                    SetPeaking(Mathf.Lerp(GetPeakingDistance(), 0, Time.deltaTime * 5f));
                }
            }
            else
            {
                SetPeaking(Mathf.Lerp(GetPeakingDistance(), 0, Time.deltaTime * 5f));
            }


            if (IsRagdolled())
            {
                GetPedestrian().SetHeadRotate(false);
                GetPedestrian().SetSpineRotate(false);
            }
            else
            {
                if (GetPedestrian().IsAnimationInPlayableState("RagdollBelly", "Base Layer") || GetPedestrian().IsAnimationInPlayableState("RagdollBack", "Base Layer"))
                {
                    GetPedestrian().SetHeadRotate(false);
                }
                else
                {
                    GetPedestrian().SetHeadRotate(true);
                }

                if (GetBehavioralState() == (ushort)BehaviouralState.Relaxed || GetPedestrian().IsAnimationInPlayableState("RagdollBelly", "Base Layer") || GetPedestrian().IsAnimationInPlayableState("RagdollBack", "Base Layer"))
                {
                    GetPedestrian().SetSpineRotate(false);
                }
                else
                {
                    GetPedestrian().SetSpineRotate(true);
                }
            }
        }

        private float GetPeakingDistance()
        {
            return peakDistance;
        }

        private void SetPeaking(float val)
        {
            peakDistance = val;
        }

        public bool IsRagdolled()
        {
            return ragdoll;
        }

        public void SetDirection(Vector3 val)
        {
            dir = val;
        }

        public ushort GetLocomotionalState()
        {
            return (ushort)Locomotion;
        }

        public ushort GetBehavioralState()
        {
            return (ushort)behaviour;
        }

        public void SetLocomotionalState(ushort val)
        {
            Locomotion = (LocomotionalState)val;
        }

        public void SetBehaviouralState(ushort val)
        {
            behaviour = (BehaviouralState)val;
        }

        public float GetForward()
        {
            return Forward;
        }

        public void SetForward(float val)
        {
            Forward = val;
        }

        public float GetSideway()
        {
            return Sideway;
        }

        public void SetSideway(float val)
        {
            Sideway = val;
        }

        public float GetTurn()
        {
            return Turn;
        }

        public void SetTurn(float val)
        {
            Turn = val;
        }

        public Pedestrian GetPedestrian()
        {
            return Ped;
        }
    }
}

