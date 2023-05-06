using UnityEngine;
using UnityEngine.UI;

public class SkillPointManager : MonoBehaviour
{
    public int SkillPoints;
    [SerializeField] private Text skillPointText;

    private void Start()
    {
        skillPointText.text = "Skill Points: " + SkillPoints;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddSkillPoints(2);
        }
    }

    public void AddSkillPoints(int points)
    {
        SkillPoints += points;
        UpdateSkillPointText();
    }

    public void UpdateSkillPointText()
    {
        if (skillPointText != null)
        {
            skillPointText.text = "Skill Points: " + SkillPoints;
        }
    }
}