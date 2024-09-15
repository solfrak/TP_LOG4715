using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_JumpDown;
        private bool m_JumpUp;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_JumpDown)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_JumpDown = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if(!m_JumpUp)
            {
                m_JumpUp = CrossPlatformInputManager.GetButtonUp("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_JumpDown, m_JumpUp);
            m_JumpDown = false;
            m_JumpUp = false;
        }
    }
}
