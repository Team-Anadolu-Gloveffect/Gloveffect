using System.Linq;
using UnityEngine;

namespace Player
{
    public class WallRun : MonoBehaviour
    {
        [Header("Properties")]
        public float maxWallDistance = 1;
        public float minHeight = 0.6f;
        public float jumpDuration = 0.25f;
        public float wallBounceForce = 3;
        public float cameraTransitionDuration = 1;
        [Range(0.0f, 1.0f)]
        public float angleThreshold=0.3f;
        public float wallSpeedMultiplier = 12f;
        public float wallGravityDown = 20f;
        public float maxAngleRoll = 30;

        PlayerInputHandler m_InputHandler;
        PlayerCharacterController m_Controller;

        Vector3[] m_Directions;
        RaycastHit[] m_Hits;
        Vector3 m_LastWallPosition;
        Vector3 m_LastwallNormal;

        float m_VerticalInput;
        bool isJumping;
        public bool isWallRunning = false;
        float m_ElapsedTimeSinceJump = 0;
        float m_ElapsedTimeSinceWallAttach = 0;
        float m_ElapsedTimeSinceWallDetach = 0;
        public bool activateWallrun = false;
        
        void Start()
        {
            m_Controller = GetComponent<PlayerCharacterController>();
            m_InputHandler = GetComponent<PlayerInputHandler>();

            m_Directions = new Vector3[]
            {
                Vector3.forward,
                Vector3.right,
                Vector3.left,
                Vector3.forward + Vector3.right,
                Vector3.forward + Vector3.left
            };
        }
        
        void LateUpdate()
        {
            isWallRunning = false;
            m_VerticalInput = Input.GetAxisRaw(Game.GameConstants.k_AxisNameVertical);
            
            if (m_InputHandler.GetJumpInputDown())
            {
                isJumping = true;
            }
            
            if (CanAttach())
            {
                m_Hits = new RaycastHit[m_Directions.Length];
                for (int i = 0; i < m_Directions.Length; i++)
                {
                    Vector3 dir = transform.TransformDirection(m_Directions[i]);
                    Physics.Raycast(transform.position, dir, out m_Hits[i], maxWallDistance);
                }

                if (CanWallRun() && activateWallrun)
                {
                    var hitQuery =
                        (from h in m_Hits
                            where (h.collider != null)
                            orderby h.distance ascending
                            select h).ToArray();

                    if (hitQuery.Length>0)
                    {
                        OnWall(hitQuery[0]);
                        m_LastWallPosition = hitQuery[0].point;
                        m_LastwallNormal = hitQuery[0].normal;
                    }
                }
            }

            if (isWallRunning)
            {
                m_ElapsedTimeSinceWallDetach = 0;
                m_ElapsedTimeSinceWallAttach += Time.deltaTime;
            }
            else
            {
                m_ElapsedTimeSinceWallAttach = 0;
                m_ElapsedTimeSinceWallDetach += Time.deltaTime;
            }
        }

        bool CanAttach()
        {
            if (isJumping)
            {
                m_ElapsedTimeSinceJump += Time.deltaTime;
                if (m_ElapsedTimeSinceJump > jumpDuration)
                {
                    m_ElapsedTimeSinceJump = 0;
                    isJumping = false;
                }
                return false;

            }
            return true;
        }

        bool CanWallRun()
        {
            return !m_Controller.IsGrounded && m_VerticalInput > 0 && VerticalCheck();
        }

        bool VerticalCheck()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minHeight);
        }

        void OnWall(RaycastHit hit)
        {
            if(hit.collider.CompareTag("Wall"))
            {
                float a = Vector3.Dot(hit.normal, Vector3.up);
                if (a >=- angleThreshold && a <= angleThreshold)
                {
                    isWallRunning = true;
                    Vector3 alongWall = transform.TransformDirection(Vector3.forward);
                    m_Controller.CharacterVelocity = alongWall * m_VerticalInput * wallSpeedMultiplier;
                    m_Controller.CharacterVelocity += Vector3.down * wallGravityDown * Time.deltaTime;
                }
            }
        }

        public float GetCameraRoll()
        {
            float side = GetSide();
            float cameraAngle = m_Controller.PlayerCamera.transform.eulerAngles.z;
            float targetAngle = 0;
            if (side!=0)
            {
                targetAngle = Mathf.Sign(side) * maxAngleRoll;
            }
            return Mathf.LerpAngle(cameraAngle, targetAngle, Mathf.Max(m_ElapsedTimeSinceWallAttach, m_ElapsedTimeSinceWallDetach) / cameraTransitionDuration);
        }

        public float GetSide()
        {
            if (isWallRunning)
            {
                Vector3 p = Vector3.Cross(transform.forward, -1*m_LastwallNormal);
                return Vector3.Dot(p, transform.up);
            }
            return 0;
        }


        public bool IsWallRunning()
        {
            return isWallRunning;
        }

        public Vector3 GetWallJumpDirection()
        {
            if (isWallRunning)
            {
                return m_LastwallNormal * wallBounceForce + Vector3.up;
            }
            return Vector3.zero;
        }
    }
}