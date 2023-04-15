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
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift))
        {
            RunAnimation();
        }
        else
        {
            StopRunAnimation();
        }
    }

    void RunAnimation()
    {
        animator.SetBool("isRunning", true);
    }

    void StopRunAnimation()
    {
        animator.SetBool("isRunning", false);
    }
}