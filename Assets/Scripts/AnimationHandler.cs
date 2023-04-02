using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Kick animation
        if (Input.GetKeyDown(KeyCode.C))
        {
            KickAnimation();
        }
        else
        {
            StopKickAnimation();
        }

        // Running animation
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            RunAnimation();
        }
        else
        {
            StopRunAnimation();
        }

        // Sprint animation
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            SprintAnimation();
        }
        else
        {
            StopSprintAnimation();
        }

        
        // Jump animation
        if (Input.GetKey(KeyCode.Space))
        {
            JumpAnimation();
        }
        else
        {
            StopJumpAnimation();
        }
    }

    void KickAnimation()
    {
        animator.SetBool("isKicking", true);
    }

    void StopKickAnimation()
    {
        animator.SetBool("isKicking", false);
    }

    void RunAnimation()
    {
        animator.SetBool("isRunning", true);
    }

    void StopRunAnimation()
    {
        animator.SetBool("isRunning", false);
    }

    void SprintAnimation()
    {
        animator.SetBool("isSprinting", true);
    }

    void StopSprintAnimation()
    {
        animator.SetBool("isSprinting", false);
    }
    
    void JumpAnimation()
    {
        animator.SetBool("isJumping", true);
    }

    void StopJumpAnimation()
    {
        animator.SetBool("isJumping", false);
    }
}