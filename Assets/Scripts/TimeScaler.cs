using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    private bool isSlowMotion = false;
    private float slowMotionFactor = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (!isSlowMotion)
            {
                Time.timeScale = slowMotionFactor;
                isSlowMotion = true;
            }
            else
            {
                Time.timeScale = 1f;
                isSlowMotion = false;
            }
        }
    }
}