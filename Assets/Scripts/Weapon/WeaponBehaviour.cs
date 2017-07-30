using UnityEngine;
using System.Collections;
using System;

namespace Weapon
{
    public class WeaponBehaviour : MonoBehaviour
    {
        private Rigidbody rb;

        [Serializable] public class BodySystem
        {
            [SerializeField] private Transform leftHandSolver;
            [SerializeField] [Range(0, 15)] private float defaultTypeUpperLayer;
            [SerializeField] [Range(0, 15)] private float aimingTypeUpperLayer;
            [SerializeField] [Range(0, 1)] private float firingWeight;

            public Transform GetLeftHandSolver()
            {
                return leftHandSolver;
            }

            public void SetLeftHandSolver(Transform transform)
            {
                leftHandSolver = transform;
            }

            public float GetAimingTypeValue()
            {
                return aimingTypeUpperLayer;
            }

            public float GetFiringWeight()
            {
                return firingWeight;
            }

            public float GetDefaultTypeValue()
            {
                return defaultTypeUpperLayer;
            }
        }

        [Serializable] public class Ammo
        {
            [SerializeField] private uint ClipSize;
            [SerializeField] private uint Bullet;
            [SerializeField] private uint MaxClipSize;

            public uint GetClipSize()
            {
                return ClipSize;
            }

            public void SetClipSize(uint val)
            {
                ClipSize = val;
            }

            public uint GetBullet()
            {
                return Bullet;
            }

            public void SetBullet(uint val)
            {
                Bullet = val;
            }

            public uint GetMaxClipSize()
            {
                return MaxClipSize;
            }

            public void SetMaxClipSize(uint val)
            {
                MaxClipSize = val;
            }
        }

        [Serializable] public class OwnerSystem
        {

            [SerializeField] private Vector3 localPosition;
            [SerializeField] private Vector3 localEulerAngles;
            //[SerializeField] private Vector3 SpineRotation;
            [SerializeField] private bool owned;

            public Vector3 GetLocalPosition()
            {
                return localPosition;
            }

            public Vector3 GetLocalEulerAngles()
            {
                return localEulerAngles;
            }

            // public Vector3 GetSpineRotation()
            //{
              //  return SpineRotation;
            //}

            public bool IsOwned()
            {
                return owned;
            }

            public void SetOwned(bool val)
            {
                owned = val;
            }
        }

        [Serializable] public class FireArmSystem
        {
            [SerializeField] private float fireRate;
            [SerializeField] private Ammo ammo;
            [SerializeField] private Transform FirePoint;
            [SerializeField] private float shootDistance = 50f;
            [SerializeField] private LayerMask shootingLayer;
            private Vector3 fireDirection;
            [SerializeField] private float hitForce;

            public Ammo GetAmmo()
            {
                return ammo;
            }

            public float GetForce()
            {
                return hitForce;
            }

            public float GetFireRate()
            {
                return fireRate;
            }

            public Vector3 GetFirePosition()
            {
                return FirePoint.position;
            }

            public float GetShootingDistance()
            {
                return shootDistance;
            }

            public LayerMask GetShootingLayers()
            {
                return shootingLayer;
            }

            public Vector3 GetFirePointDirection()
            {
                return fireDirection;
            }

            public void SetFirePointDirection(Vector3 dir)
            {
                fireDirection = dir;
            }
        }
        
        [SerializeField] private BodySystem FirstPersonOwner;
        [SerializeField] private BodySystem ThirdPersonOwner;

        [SerializeField] private FireArmSystem fireArm;

        [SerializeField] private OwnerSystem owner; // 

        public BodySystem GetOwnerValue()
        {
            return ThirdPersonOwner;
        }

        public OwnerSystem GetOwnerBehaviorValue()
        {
            return owner;
        }

        public BodySystem GetOwnerValue(bool IsFPS)
        {
            if (IsFPS == true)
            {
                return FirstPersonOwner;
            }
            else
            {
                return ThirdPersonOwner;
            }
        }

        public FireArmSystem GetFireArmSystem()
        {
            return fireArm;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            Debug.DrawRay(fireArm.GetFirePosition(), transform.forward * 50, Color.red);
            Debug.DrawRay(fireArm.GetFirePosition(), fireArm.GetFirePointDirection() * 50, Color.black);
            WeaponOwnerShipBehavior();
        }

        private void WeaponOwnerShipBehavior()
        {
            if (owner.IsOwned() == false)
            {
                transform.parent = null;
                rb.isKinematic = false;
                fireArm.SetFirePointDirection(transform.forward);
            }
            else
            {
                rb.isKinematic = true;
            }
        }

        public void Fire()
        {
            ShootingSystem();
        }

        private void ShootingSystem()
        {
            if (fireArm.GetAmmo().GetClipSize() > 0)
            {
                RaycastHit[] hits = new RaycastHit[10];
                Ray ray = new Ray(fireArm.GetFirePosition(), fireArm.GetFirePointDirection());
                hits = Physics.RaycastAll(ray, fireArm.GetShootingDistance(), fireArm.GetShootingLayers());
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    Pedestrian.Pedestrian ped = hit.collider.GetComponentInParent<Pedestrian.Pedestrian>();
                    WeaponBehaviour wep = hit.collider.GetComponent<WeaponBehaviour>();
                    if (wep != null)
                    {
                        GetOwnerBehaviorValue().SetOwned(false);
                        Rigidbody rbp = wep.GetComponentInParent<Rigidbody>();
                        if (rbp != null)
                        {
                            rbp.AddForce(-hit.normal * fireArm.GetForce());
                        }
                    }

                    if (rb != null)
                    {
                        rb.AddForce(-hit.normal * fireArm.GetForce());
                    }

                    fireArm.GetAmmo().SetClipSize(fireArm.GetAmmo().GetClipSize() - 1);
                    Debug.Log(hit.collider.gameObject.name);

                    if (ped != null)
                    {
                        ped.SetHealth(ped.GetHealth() - 36);
                    }
                }
            }
        }
    }
}
