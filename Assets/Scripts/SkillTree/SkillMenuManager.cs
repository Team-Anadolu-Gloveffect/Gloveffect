using UnityEngine;

public class SkillMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject skillTreePanel;

    private void Awake()
    {
        skillTreePanel.SetActive(false);
    }

    private void Update()
    {
        HandleSkillTreePanelActivation();
    }

    private void HandleSkillTreePanelActivation()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            skillTreePanel.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            skillTreePanel.SetActive(false);
        }
    }
}