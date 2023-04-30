using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    bool IsSlowMotion = false;
    float m_SlowMotionFactor = 0.5f;
    public bool activateSlowMotion = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2) && activateSlowMotion)
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