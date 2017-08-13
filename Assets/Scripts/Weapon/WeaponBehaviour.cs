using UnityEngine;
using System.Collections;
using System;

namespace Weapon
{
    public class WeaponBehaviour : MonoBehaviour
    {
        private Rigidbody rb;

        [Serializable] public class FirstPersonSystem
        {
            [SerializeField] [Range(0, 15)] private float defaultType;
            [SerializeField] [Range(0, 15)] private float aimingType;
            [SerializeField] [Range(0, 15)] private float defaultFiringTypeUpperLayer;
            [SerializeField] [Range(0, 15)] private float aimingFiringTypeUpperLayer;
            [SerializeField] private float fireRate;
            [SerializeField] [Range(0, 15)] private float defaultRelaxedType = 1;
            [SerializeField] [Range(0, 15)] private float relaxedType;
            [SerializeField] private Vector3 SpineRotation;

            public float GetFireRate()
            {
                return fireRate;
            }

            public float GetDefaultFiringTypeValue()
            {
                return defaultFiringTypeUpperLayer;
            }

            public float GetAimingFiringTypeValue()
            {
                return aimingFiringTypeUpperLayer;
            }

            public float GetDefaultTypeValue()
            {
                return defaultType;
            }

            public float GetAimingTypeValue()
            {
                return aimingType;
            }

            public float GetDefaultRelaxedType()
            {
                return defaultRelaxedType;
            }

            public float GetRelaxedType()
            {
                return relaxedType;
            }

            public Vector3 GetSpineRotation()
            {
              return SpineRotation;
            }
        }

        [Serializable] public class ThirdPersonSystem
        {
            [SerializeField] private Transform leftHandSolver;
            [SerializeField] [Range(0, 15)] private float aimingTypeUpperLayer;
            [SerializeField] [Range(0, 1)] private float firingWeight;
            [SerializeField] private float fireRate;
            [SerializeField] private bool UpperLayerRelaxedAnimation;
            [SerializeField] private Vector3 SpineRotation;

            public float GetFireRate()
            {
                return fireRate;
            }

            public Transform GetLeftHandSolver()
            {
                return leftHandSolver;
            }

            public float GetAimingTypeValue()
            {
                return aimingTypeUpperLayer;
            }

            public float GetFiringWeight()
            {
                return firingWeight;
            }

            public bool GetBoolUseUpperLayerRelaxedAnimation()
            {
                return UpperLayerRelaxedAnimation;
            }

            public Vector3 GetSpineRotation()
            {
              return SpineRotation;
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

            public Transform GetFirePoint()
            {
                return FirePoint;
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
        
        [SerializeField] private FirstPersonSystem FirstPersonOwner;
        [SerializeField] private ThirdPersonSystem ThirdPersonOwner;

        [SerializeField] private FireArmSystem fireArm;

        [SerializeField] private OwnerSystem owner; // 

        public ThirdPersonSystem GetThirdPersonSystem()
        {
            return ThirdPersonOwner;
        }

        public OwnerSystem GetOwnerBehaviorValue()
        {
            return owner;
        }

        public FirstPersonSystem GetFirstPersonSystem()
        {
            return FirstPersonOwner;
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
            Debug.DrawRay(fireArm.GetFirePoint().position, fireArm.GetFirePoint().forward * 50, Color.red);
            Debug.DrawRay(fireArm.GetFirePoint().position, fireArm.GetFirePointDirection() * 50, Color.black);
            WeaponOwnerShipBehavior();
        }

        public void ReloadLogic()
        {
            uint BulletSize = GetFireArmSystem().GetAmmo().GetClipSize() - GetFireArmSystem().GetAmmo().GetMaxClipSize();
            GetFireArmSystem().GetAmmo().SetClipSize(GetFireArmSystem().GetAmmo().GetMaxClipSize() > GetFireArmSystem().GetAmmo().GetBullet() ? GetFireArmSystem().GetAmmo().GetBullet() : GetFireArmSystem().GetAmmo().GetMaxClipSize());
            GetFireArmSystem().GetAmmo().SetBullet(GetFireArmSystem().GetAmmo().GetMaxClipSize() > GetFireArmSystem().GetAmmo().GetBullet() ? 0 : GetFireArmSystem().GetAmmo().GetBullet() + BulletSize);
        }

        private void WeaponOwnerShipBehavior()
        {
            if (owner.IsOwned() == false)
            {
                transform.parent = null;
                rb.isKinematic = false;
                fireArm.SetFirePointDirection(transform.forward);
                rb.useGravity = true;

                MeshRenderer[] wepMesh = GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer mesh in wepMesh)
                {
                    mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }

            }
            else
            {
                rb.isKinematic = true;
                Collider[] wepCol = rb.GetComponents<Collider>();
                foreach (Collider col in wepCol)
                {
                    col.isTrigger = false;
                }
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
                fireArm.GetAmmo().SetClipSize(fireArm.GetAmmo().GetClipSize() - 1);
                RaycastHit[] hits = new RaycastHit[10];
                Ray ray = new Ray(fireArm.GetFirePoint().position, fireArm.GetFirePointDirection());
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

                    Debug.Log(hit.collider.gameObject.name);
                    
                    if (ped != null)
                    {
                        if (GetComponent<WeaponBehaviour>() != ped.GetWeapon())
                        {
                            ped.SetHealth(ped.GetHealth() - 36);
                        }
                    }
                }
            }
        }
    }
}
