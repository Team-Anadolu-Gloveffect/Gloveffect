using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    bool IsSlowMotion = false;
    float m_SlowMotionFactor = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (!IsSlowMotion)
            {
                Time.timeScale = m_SlowMotionFactor;
                IsSlowMotion = true;
            }
            else
            {
                Time.timeScale = 1f;
                IsSlowMotion = false;
            }
        }
    }
}