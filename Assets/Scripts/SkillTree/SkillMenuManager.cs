using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenuManager : MonoBehaviour
{
    public GameObject skillTreePanel;

    private void Awake()
    {
        skillTreePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            skillTreePanel.SetActive(true);
        }
        if (Input.GetKeyUp("e"))
        {
            skillTreePanel.SetActive(false);
        }
    }
}