using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Player
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [Header("References")]
        public Camera PlayerCamera;
        
        [Header("General")]
        public float GravityDownForce = 20f;
        public LayerMask GroundCheckLayers = -1;
        public float GroundCheckDistance = 0.05f;
        
        [Header("Movement")]
        public float MaxSpeedOnGround = 10f;
        public float MovementSharpnessOnGround = 15;
        [Range(0, 1)]
        public float MaxSpeedCrouchedRatio = 0.5f;
        public float MaxSpeedInAir = 10f;
        public float AccelerationSpeedInAir = 25f;
        public float SprintSpeedModifier = 2f;
        
        [Header("Rotation")]
        public float RotationSpeed = 200f;

        public float RotationMultiplier = 1f;
        
        [Header("Jump")]
        public float JumpForce = 9f;
        
        [Header("Stance")]
        public float CameraHeightRatio = 0.9f;
        public float CapsuleHeightStanding = 1.8f;
        public float CapsuleHeightCrouching = 0.9f;
        public float CrouchingSharpness = 10f;

        public UnityAction<bool> OnStanceChanged;
        
        public Vector3 CharacterVelocity { get; set; }
        public bool IsGrounded { get; private set; }
        public bool HasJumpedThisFrame { get; private set; }
        public bool IsCrouching { get; private set; }

        PlayerInputHandler m_InputHandler;
        CharacterController m_Controller;
        WallRun m_WallRunComponent;
        Vector3 m_GroundNormal;
        Vector3 m_CharacterVelocity;
        Vector3 m_LatestImpactSpeed;
        float m_LastTimeJumped = 0f;
        public bool m_HasDoubleJumped = true;
        float m_CameraVerticalAngle = 0f;
        float m_TargetCharacterHeight;
        
        const float k_JumpGroundingPreventionTime = 0.2f;
        const float k_GroundCheckDistanceInAir = 0.07f;
        
        private bool m_IsSliding = false;
        public bool activateDoubleJump = false;

        void Start()
        {
            m_Controller = GetComponent<CharacterController>();
            m_InputHandler = GetComponent<PlayerInputHandler>();
            m_WallRunComponent = GetComponent<WallRun>();
            
            m_Controller.enableOverlapRecovery = true;
            
            SetCrouchingState(false, true);
            UpdateCharacterHeight(true);
        }

        void Update()
        {
            HasJumpedThisFrame = false;
            GroundCheck();
            
            if (m_InputHandler.GetCrouchInputDown())
            {
                SetCrouchingState(!IsCrouching, false);
            }
            
            if (m_InputHandler.GetSlideInputDown() && IsGrounded && !m_IsSliding)
            {
                StartCoroutine(Slide());
            }
            
            UpdateCharacterHeight(false);
            HandleCharacterMovement();
        }
        
        void GroundCheck()
        {
            float chosenGroundCheckDistance = IsGrounded ? (m_Controller.skinWidth + GroundCheckDistance) : k_GroundCheckDistanceInAir;
            
            IsGrounded = false;
            m_GroundNormal = Vector3.up;

            if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
            {
                if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height),
                        m_Controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, GroundCheckLayers,
                        QueryTriggerInteraction.Ignore))
                {
                    m_GroundNormal = hit.normal;
                    
                    if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                        IsNormalUnderSlopeLimit(m_GroundNormal))
                    {
                        IsGrounded = true;
                        
                        if (hit.distance > m_Controller.skinWidth)
                        {
                            m_Controller.Move(Vector3.down * hit.distance);
                        }
                    }
                }
            }
            if (IsGrounded && activateDoubleJump)
            {
                m_HasDoubleJumped = false;
            }
        }
        
        void HandleCharacterMovement()
        {
            {
                transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * RotationSpeed * RotationMultiplier), 0f), Space.Self);
            }
            
            {
                m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * RotationSpeed * RotationMultiplier;
                
                m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);
                
                if (m_WallRunComponent != null)
                {
                    PlayerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, m_WallRunComponent.GetCameraRoll());
                }
                else
                {
                    PlayerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
                }
            }
            
            bool isSprinting = m_InputHandler.GetSprintInputHeld();
            {
                if (isSprinting)
                {
                    isSprinting = SetCrouchingState(false, false);
                }

                float speedModifier = isSprinting ? SprintSpeedModifier : 1f;
                
                Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

                if (IsGrounded || (m_WallRunComponent != null && m_WallRunComponent.IsWallRunning()))
                {
                    Vector3 targetVelocity = worldspaceMoveInput * MaxSpeedOnGround * speedModifier;
                    if (IsCrouching)
                        targetVelocity *= MaxSpeedCrouchedRatio;
                    targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) *
                                     targetVelocity.magnitude;

                    CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity,
                        MovementSharpnessOnGround * Time.deltaTime);
                }

                if ((IsGrounded || (m_WallRunComponent != null && m_WallRunComponent.IsWallRunning())) &&
                    m_InputHandler.GetJumpInputDown())
                {
                    if (SetCrouchingState(false, false))
                    {
                        if (IsGrounded)
                        {
                            CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);
                            CharacterVelocity += Vector3.up * JumpForce;
                        }
                        else
                        {
                            CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);
                            CharacterVelocity += m_WallRunComponent.GetWallJumpDirection() * JumpForce;
                        }

                        m_LastTimeJumped = Time.time;
                        HasJumpedThisFrame = true;

                        IsGrounded = false;
                        m_GroundNormal = Vector3.up;
                    }
                }

                else
                {
                    if (m_InputHandler.GetJumpInputDown() && !m_HasDoubleJumped)
                    {
                        CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);
                        CharacterVelocity += Vector3.up * JumpForce;

                        m_HasDoubleJumped = true;
                    }

                    CharacterVelocity += worldspaceMoveInput * AccelerationSpeedInAir * Time.deltaTime;
                    
                    float verticalVelocity = CharacterVelocity.y;
                    Vector3 horizontalVelocity = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
                    horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, MaxSpeedInAir * speedModifier);
                    CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
                    CharacterVelocity += Vector3.down * GravityDownForce * Time.deltaTime;
                }
            }
            
            Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
            Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(m_Controller.height);
            m_Controller.Move(CharacterVelocity * Time.deltaTime);
            
            m_LatestImpactSpeed = Vector3.zero;
            if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, m_Controller.radius,
                CharacterVelocity.normalized, out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime, -1,
                QueryTriggerInteraction.Ignore))
            {
                m_LatestImpactSpeed = CharacterVelocity;

                CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
            }
        }
        
        bool IsNormalUnderSlopeLimit(Vector3 normal)
        {
            return Vector3.Angle(transform.up, normal) <= m_Controller.slopeLimit;
        }
        
        Vector3 GetCapsuleBottomHemisphere()
        {
            return transform.position + (transform.up * m_Controller.radius);
        }
        
        Vector3 GetCapsuleTopHemisphere(float atHeight)
        {
            return transform.position + (transform.up * (atHeight - m_Controller.radius));
        }
        
        public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
        {
            Vector3 directionRight = Vector3.Cross(direction, transform.up);
            return Vector3.Cross(slopeNormal, directionRight).normalized;
        }
        
        void UpdateCharacterHeight(bool force)
        {
            if (force)
            {
                m_Controller.height = m_TargetCharacterHeight;
                m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
                PlayerCamera.transform.localPosition = Vector3.up * m_TargetCharacterHeight * CameraHeightRatio;
            }
            else if (m_Controller.height != m_TargetCharacterHeight)
            {
                m_Controller.height = Mathf.Lerp(m_Controller.height, m_TargetCharacterHeight,
                    CrouchingSharpness * Time.deltaTime);
                m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
                PlayerCamera.transform.localPosition = Vector3.Lerp(PlayerCamera.transform.localPosition,
                    Vector3.up * m_TargetCharacterHeight * CameraHeightRatio, CrouchingSharpness * Time.deltaTime);
            }
        }
        
        bool SetCrouchingState(bool crouched, bool ignoreObstructions)
        {
            if (crouched)
            {
                m_TargetCharacterHeight = CapsuleHeightCrouching;
            }
            else
            {
                if (!ignoreObstructions)
                {
                    Collider[] standingOverlaps = Physics.OverlapCapsule(
                        GetCapsuleBottomHemisphere(),
                        GetCapsuleTopHemisphere(CapsuleHeightStanding),
                        m_Controller.radius,
                        -1,
                        QueryTriggerInteraction.Ignore);
                    foreach (Collider c in standingOverlaps)
                    {
                        if (c != m_Controller)
                        {
                            return false;
                        }
                    }
                }

                m_TargetCharacterHeight = CapsuleHeightStanding;
            }

            if (OnStanceChanged != null)
            {
                OnStanceChanged.Invoke(crouched);
            }

            IsCrouching = crouched;
            return true;
        }
        
        IEnumerator Slide()
        {
            m_IsSliding = true;

            float slideDuration = 0.4f;
            float elapsedTime = 0f;

            while (elapsedTime < slideDuration)
            {
                Vector3 slideMove = transform.forward * MaxSpeedOnGround * SprintSpeedModifier;
                m_Controller.Move(slideMove * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                m_TargetCharacterHeight = CapsuleHeightCrouching;

                yield return null;
            }
            m_TargetCharacterHeight = CapsuleHeightStanding;

            m_IsSliding = false;
        }
    }
}
