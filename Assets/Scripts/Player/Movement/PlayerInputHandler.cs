using System;
using UnityEngine;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public float LookSensitivity = 1f;
        PlayerCharacterController m_PlayerCharacterController;
        public GameObject skillTreeUI;
        public bool activateSprint = false;
        public bool activateSlide = false;

        void Start()
        {
            m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (skillTreeUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
            else if (!skillTreeUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public bool CanProcessInput()
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }
        
        public Vector3 GetMoveInput()
        {
            if (CanProcessInput())
            {
                Vector3 move = new Vector3(Input.GetAxisRaw(Game.GameConstants.k_AxisNameHorizontal), 0f, Input.GetAxisRaw(Game.GameConstants.k_AxisNameVertical));


                move = Vector3.ClampMagnitude(move, 1);

                return move;
            }

            return Vector3.zero;
        }
        
        public float GetLookInputsHorizontal()
        {
            return GetMouseLookAxis(Game.GameConstants.k_MouseAxisNameHorizontal);
        }
        
        public float GetLookInputsVertical()
        {
            return GetMouseLookAxis(Game.GameConstants.k_MouseAxisNameVertical);
        }
        
        public bool GetJumpInputDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(Game.GameConstants.k_ButtonNameJump);
            }

            return false;
        }

        public bool GetSprintInputHeld()
        {
            if (CanProcessInput() && activateSprint)
            {
                return Input.GetButton(Game.GameConstants.k_ButtonNameSprint);
            }

            return false;
        }
        
        public bool GetCrouchInputDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(Game.GameConstants.k_ButtonNameCrouch);
            }

            return false;
        }
        
        public bool GetCrouchInputReleased()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonUp(Game.GameConstants.k_ButtonNameCrouch);
            }

            return false;
        }
        
        public bool GetSlideInputDown()
        {
            if (CanProcessInput() && activateSlide)
            {
                return Input.GetButtonDown(Game.GameConstants.k_ButtonNameSlide);
            }

            return false;
        }
        
        
        float GetMouseLookAxis(string mouseInputName)
        {
            if (CanProcessInput())
            {
                float i = Input.GetAxisRaw(mouseInputName);

                i *= LookSensitivity;
                
                i *= 0.01f;

                return i;
            }

            return 0f;
        }
    }
}
