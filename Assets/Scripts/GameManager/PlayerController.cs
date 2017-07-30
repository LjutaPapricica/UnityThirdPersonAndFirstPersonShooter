using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pedestrian;
using System;

namespace GameManager
{
    [RequireComponent(typeof(PedestrianController))]
    public class PlayerController : MonoBehaviour
    {
        private PedestrianController PedControl;

        [SerializeField] private Transform pivotCamera;
        [SerializeField] private Camera cam;

        [SerializeField] private Vector3 DefaultCameraDistance = new Vector3(0, 0, -5.75f);
        [SerializeField] private Vector3 AimDistance = new Vector3(.5f, 0, 2);
        public enum CameraFollowType { DefaultDistance, AimDistance, AimRightShoulderDistance, FPSCamera }
        [SerializeField] private CameraFollowType cameraFollowType;

        [SerializeField] private float MouseXAxisSensitivity;
        private float Yaw;
        [SerializeField] private float MouseYAxisSensitivity;
        private float Pitch;
        private Vector3 CurrentRotation;
        [SerializeField] private float RotationSmoothTime;
        [SerializeField] private float PositionSmoothTime;
        [SerializeField] private float movementSmoothTime;
        private Vector3 CameraPositionSmoothVelocity;
        private Vector3 CameraLocalPositionSmoothVelocity;
        private Vector3 RotationSmoothVelocity;
        private float forwardVelocity;
        private float sidewayVelocity;
        [SerializeField] private Vector3 FPSOffset;

        private void Start()
        {
            PedControl = GetComponent<PedestrianController>();
            cam = Camera.main;
        }
        
        private void Update()
        {
            PlayerInputBehaviour();
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (PedControl.GetPedestrian().IsFPS() == false)
                {
                    PedControl.GetPedestrian().SetFPS(true);
                }
                else
                {
                    PedControl.GetPedestrian().SetFPS(false);
                }
            }
        }

        private void PlayerInputBehaviour()
        {
            DirectionalMethod();
            LocomotionalMethod();
            PedestrianBehaviourMethod();
        }

        private void LocomotionalMethod()
        {
            if (PedControl.GetPedestrian().IsFPS() == true)
            {
                PedControl.SetDirection(PedControl.GetPedestrian().GetHeadLookPosition() - PedControl.GetPedestrian().GetWeapon().GetFireArmSystem().GetFirePosition().normalized);
            }
            else
            {
                if (PedControl.GetBehavioralState() != (ushort)PedestrianController.BehaviouralState.Aiming || PedControl.GetBehavioralState() != (ushort)PedestrianController.BehaviouralState.Attacking)
                {
                    PedControl.SetDirection(PedControl.GetPedestrian().GetHeadLookPosition() - PedControl.GetPedestrian().GetWeapon().GetFireArmSystem().GetFirePosition().normalized);
                }
                else
                {
                    PedControl.SetDirection(PedControl.GetPedestrian().GetWeapon().transform.forward);
                }
            }

            if (PedControl.GetLocomotionalState() == (ushort)PedestrianController.LocomotionalState.Crouch)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    PedControl.SetLocomotionalState((ushort)PedestrianController.LocomotionalState.Standing);
                }
            }
            else if (PedControl.GetLocomotionalState() == (ushort)PedestrianController.LocomotionalState.Standing)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    PedControl.SetLocomotionalState((ushort)PedestrianController.LocomotionalState.Crouch);
                }
            }
        }

        private void PedestrianBehaviourMethod()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    PedControl.SetBehaviouralState((ushort)PedestrianController.BehaviouralState.Attacking);
                }
                else
                {
                    PedControl.SetBehaviouralState((ushort)PedestrianController.BehaviouralState.Aiming);
                }

                if (PedControl.GetPedestrian().IsFPS())
                {
                    if (cameraFollowType != CameraFollowType.AimRightShoulderDistance && cameraFollowType != CameraFollowType.FPSCamera)
                    {
                        cameraFollowType = CameraFollowType.FPSCamera;
                        PedControl.GetPedestrian().SetFPS(true);
                    }

                    if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
                    {
                        cameraFollowType = CameraFollowType.FPSCamera;
                        PedControl.GetPedestrian().SetFPS(true);
                    }
                    else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
                    {
                        cameraFollowType = CameraFollowType.AimRightShoulderDistance;
                        PedControl.GetPedestrian().SetFPS(false);
                    }
                    else if (Input.GetKeyDown(KeyCode.V))
                    {
                        if (PedControl.GetPedestrian().IsFPS() == true)
                        {
                            PedControl.GetPedestrian().SetFPS(false);
                            cameraFollowType = CameraFollowType.AimRightShoulderDistance;
                        }
                        else
                        {
                            PedControl.GetPedestrian().SetFPS(true);
                            cameraFollowType = CameraFollowType.FPSCamera;
                        }
                    }
                }
                else
                {
                    if (cameraFollowType != CameraFollowType.AimRightShoulderDistance && cameraFollowType != CameraFollowType.FPSCamera)
                    {
                        cameraFollowType = CameraFollowType.AimRightShoulderDistance;
                    }

                    if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
                    {
                        cameraFollowType = CameraFollowType.FPSCamera;
                        PedControl.GetPedestrian().SetFPS(true);
                    }
                    else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
                    {
                        cameraFollowType = CameraFollowType.AimRightShoulderDistance;
                        PedControl.GetPedestrian().SetFPS(false);
                    }
                }

            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                PedControl.SetBehaviouralState((ushort)PedestrianController.BehaviouralState.Attacking);
                if (PedControl.GetPedestrian().IsFPS())
                {
                    cameraFollowType = CameraFollowType.FPSCamera;
                }
                else
                {
                    cameraFollowType = CameraFollowType.AimDistance;
                }
            }
            else
            {
                if (PedControl.GetPedestrian().GetAnimator().GetCurrentAnimatorStateInfo(PedControl.GetPedestrian().GetAnimator().GetLayerIndex("Upper Layer")).IsName("Firing"))
                {
                    PedControl.SetBehaviouralState((ushort)PedestrianController.BehaviouralState.Aiming);
                }
                else
                {
                    PedControl.SetBehaviouralState((ushort)PedestrianController.BehaviouralState.Default);
                }

                if (PedControl.GetPedestrian().IsFPS())
                {
                    cameraFollowType = CameraFollowType.FPSCamera;
                }
                else
                {
                    cameraFollowType = CameraFollowType.DefaultDistance;
                }
            }
        }

        private void DirectionalMethod()
        {
            Vector2 camForward = new Vector2(cam.transform.forward.x, cam.transform.forward.z);
            Vector2 camRight = new Vector2(cam.transform.right.x, cam.transform.right.z);
            Vector2 v = Input.GetAxis("Vertical") * camForward;
            Vector2 h = Input.GetAxis("Horizontal") * camRight;
            Vector2 moveDir = (v + h).normalized;
            float movementValue = Input.GetKey(KeyCode.LeftShift) ? 1f : .2f;
            Vector2 PedForward = new Vector2(PedControl.transform.forward.x, PedControl.transform.forward.z);
            Vector2 PedRight = new Vector2(PedControl.transform.right.x, PedControl.transform.right.z);
            float InputX = Vector2.Dot(PedForward, moveDir * movementValue);
            float InputY = Vector2.Dot(PedRight, moveDir) * movementValue;
            float Forward = Mathf.Clamp(Mathf.SmoothDamp(PedControl.GetForward(), InputX, ref forwardVelocity, movementSmoothTime), -(PedControl.GetBehavioralState() != (ushort)PedestrianController.BehaviouralState.Default ? .5f : 1f), (PedControl.GetBehavioralState() == (ushort)PedestrianController.BehaviouralState.Default ? 1f : .5f));
            float Sideway = Mathf.Clamp(Mathf.SmoothDamp(PedControl.GetSideway(), InputY, ref sidewayVelocity, movementSmoothTime), -(PedControl.GetBehavioralState() != (ushort)PedestrianController.BehaviouralState.Default ? .5f : 1f), (PedControl.GetBehavioralState() == (ushort)PedestrianController.BehaviouralState.Default ? 1f : .5f));
            PedControl.SetForward(Forward);
            PedControl.SetSideway(Sideway);
            
            if (Vector2.Angle(PedForward, camForward) > 6)
            {
                if (PedControl.GetPedestrian().IsFPS() == false)
                {
                    if (PedControl.GetBehavioralState() != (ushort)PedestrianController.BehaviouralState.Default)
                    {
                        PedControl.SetTurn(0);
                    }
                    else
                    {
                        float negCheck = (Vector2.Angle(PedRight, camForward) > 90 ? -1 : 1);
                        PedControl.SetTurn(Vector2.Angle(PedForward, camForward) * negCheck);
                    }
                }
                else
                {
                    PedControl.SetTurn(0);
                    if (PedControl.IsRagdolled())
                    {

                    }
                    else
                    {
                        PedControl.transform.eulerAngles = Vector3.up * Quaternion.LookRotation(PedControl.GetPedestrian().GetHeadLookPosition() - PedControl.transform.position).eulerAngles.y;
                    }
                }
            }
            else
            {
                PedControl.SetTurn(0);
            }

            if (moveDir.magnitude != 0)
            {
                PedControl.transform.rotation = Quaternion.Slerp(PedControl.transform.rotation, Quaternion.LookRotation(new Vector3(((PedControl.GetPedestrian().GetHeadLookPosition() - PedControl.transform.position).normalized).x, 0, (PedControl.GetPedestrian().GetHeadLookPosition() - PedControl.transform.position).normalized.z)), Time.deltaTime * 4);
            }
        }

        private void LateUpdate()
        {
            CameraSystem();
        }

        private void CameraSystem()
        {
            switch (cameraFollowType)
            {
                case CameraFollowType.DefaultDistance:
                    DefaultCameraType();
                    break;
                case CameraFollowType.AimDistance:
                    AimDistanceCamera();
                    break;
                case CameraFollowType.AimRightShoulderDistance:
                    AimRightShoulderDistance();
                    break;
                case CameraFollowType.FPSCamera:
                    FPSCameraSystem();
                    break;
                default:
                    break;
            }
        }

        private void AimRightShoulderDistance()
        {
            pivotCamera.transform.position = Vector3.SmoothDamp(pivotCamera.transform.position, PedControl.GetPedestrian().GetAnimator().GetBoneTransform(HumanBodyBones.Head).position, ref CameraPositionSmoothVelocity, PositionSmoothTime);
            cam.transform.parent = pivotCamera;
            cam.transform.localPosition = Vector3.SmoothDamp(cam.transform.localPosition, AimDistance, ref CameraLocalPositionSmoothVelocity, .1f);
            Yaw += Input.GetAxis("Mouse X") * MouseXAxisSensitivity;
            Vector2 YawLimit = new Vector2(-Mathf.Infinity, Mathf.Infinity);
            Yaw = Mathf.Clamp(Yaw, YawLimit.x, YawLimit.y);
            Pitch -= Input.GetAxis("Mouse Y") * MouseYAxisSensitivity;
            Vector2 PitchLimit = new Vector2(-45, 45);
            Pitch = Mathf.Clamp(Pitch, PitchLimit.x, PitchLimit.y);
            CurrentRotation = Vector3.SmoothDamp(CurrentRotation, new Vector3(Pitch, Yaw), ref RotationSmoothVelocity, RotationSmoothTime);
            pivotCamera.eulerAngles = CurrentRotation;
            FPSHeadLook();
        }

        private void FPSCameraSystem()
        {
            if (PedControl.IsRagdolled() ==  true)
            {
                cam.transform.localPosition = FPSOffset;
                cam.transform.localEulerAngles = Vector3.zero;
                pivotCamera.transform.position = PedControl.GetPedestrian().GetAnimator(true).GetBoneTransform(HumanBodyBones.Head).position;
                pivotCamera.eulerAngles = Quaternion.LookRotation(PedControl.GetPedestrian().GetAnimator(true).GetBoneTransform(HumanBodyBones.Head).forward).eulerAngles;
            }
            else
            {
                pivotCamera.transform.position = PedControl.GetPedestrian().GetAnimator(true).GetBoneTransform(HumanBodyBones.Head).position;
                cam.transform.parent = pivotCamera;
                cam.transform.localPosition = FPSOffset;
                cam.transform.localEulerAngles = Vector3.zero;
                Yaw += Input.GetAxis("Mouse X") * MouseXAxisSensitivity;
                Vector2 YawLimit = new Vector2(-Mathf.Infinity, Mathf.Infinity);
                Yaw = Mathf.Clamp(Yaw, YawLimit.x, YawLimit.y);
                Pitch -= Input.GetAxis("Mouse Y") * MouseYAxisSensitivity;
                Vector2 PitchLimit = new Vector2(-80, 80);
                Pitch = Mathf.Clamp(Pitch, PitchLimit.x, PitchLimit.y);
                CurrentRotation = Vector3.SmoothDamp(CurrentRotation, new Vector3(Pitch, Yaw), ref RotationSmoothVelocity, RotationSmoothTime);
                pivotCamera.eulerAngles = new Vector3(CurrentRotation.x, CurrentRotation.y, 0);
            }
            FPSHeadLook();
        }

        private void AimDistanceCamera()
        {
            pivotCamera.transform.position = Vector3.SmoothDamp(pivotCamera.transform.position, PedControl.GetPedestrian().GetAnimator().GetBoneTransform(HumanBodyBones.Head).position, ref CameraPositionSmoothVelocity, PositionSmoothTime);
            cam.transform.parent = pivotCamera;
            cam.transform.localPosition = Vector3.SmoothDamp(cam.transform.localPosition, new Vector3(AimDistance.x, DefaultCameraDistance.y, DefaultCameraDistance.z), ref CameraLocalPositionSmoothVelocity, .1f);
            Yaw += Input.GetAxis("Mouse X") * MouseXAxisSensitivity;
            Vector2 YawLimit = new Vector2(-Mathf.Infinity, Mathf.Infinity);
            Yaw = Mathf.Clamp(Yaw, YawLimit.x, YawLimit.y);
            Pitch -= Input.GetAxis("Mouse Y") * MouseYAxisSensitivity;
            Vector2 PitchLimit = new Vector2(-45, 45);
            Pitch = Mathf.Clamp(Pitch, PitchLimit.x, PitchLimit.y);
            CurrentRotation = Vector3.SmoothDamp(CurrentRotation, new Vector3(Pitch, Yaw), ref RotationSmoothVelocity, RotationSmoothTime);
            pivotCamera.eulerAngles = CurrentRotation;
            FPSHeadLook();
        }

        private void DefaultCameraType()
        {
            pivotCamera.transform.position = Vector3.SmoothDamp(pivotCamera.transform.position, PedControl.GetPedestrian().GetAnimator(false).GetBoneTransform(HumanBodyBones.Head).position, ref CameraPositionSmoothVelocity, PositionSmoothTime);
            cam.transform.parent = pivotCamera;
            cam.transform.localPosition = Vector3.SmoothDamp(cam.transform.localPosition, DefaultCameraDistance, ref CameraLocalPositionSmoothVelocity, .1f);
            Yaw += Input.GetAxis("Mouse X") * MouseXAxisSensitivity;
            Vector2 YawLimit = new Vector2(-Mathf.Infinity, Mathf.Infinity);
            Yaw = Mathf.Clamp(Yaw, YawLimit.x, YawLimit.y);
            Pitch -= Input.GetAxis("Mouse Y") * MouseYAxisSensitivity;
            Vector2 PitchLimit = new Vector2(-20, 85);
            Pitch = Mathf.Clamp(Pitch, PitchLimit.x, PitchLimit.y);
            CurrentRotation = Vector3.SmoothDamp(CurrentRotation, new Vector3(Pitch, Yaw), ref RotationSmoothVelocity, RotationSmoothTime);
            pivotCamera.eulerAngles = CurrentRotation;
            FPSHeadLook();
        }

        void FPSHeadLook()
        {
            PedControl.GetPedestrian().SetHeadLookPosition(cam.transform.position + cam.transform.forward * cam.farClipPlane);
        }

        private void HeadLookSystem()
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (!Physics.Raycast(ray, out hit, cam.farClipPlane))
            {
                PedControl.GetPedestrian().SetHeadLookPosition(cam.transform.position + cam.transform.forward * cam.farClipPlane);
            }
            else
            {
                if (hit.distance < 4f)
                {
                    PedControl.GetPedestrian().SetHeadLookPosition(cam.transform.position + cam.transform.forward * 4f);
                }
                else
                {
                    PedControl.GetPedestrian().SetHeadLookPosition(hit.point);
                }
            }
        }
    }
}

