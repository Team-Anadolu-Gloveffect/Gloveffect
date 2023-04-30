using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointManager : MonoBehaviour
{
    public int skillPoints = 0;
    private Text skillPointText;

    public void AddSkillPoints(int points)
    {
        skillPoints += points;
        UpdateSkillPointText();
    }

    private void Start()
    {
        skillPointText = GameObject.Find("SkillPoint").GetComponent<Text>();
        UpdateSkillPointText();
    }

    private void UpdateSkillPointText()
    {
        if (skillPointText != null)
        {
            skillPointText.text = "Skill Points: " + skillPoints;
        }
    }

    private void Update()
    {
        SkillPoint();
        UpdateSkillPointText();
    }

    void SkillPoint()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddSkillPoints(2);
            Debug.Log("Skill Points: " + skillPoints);
        }
    }
}