using UnityEngine;
using UnityEngine.UI;
using Enums;

public class SpellSelectionButtonController : MonoBehaviour
{
    [SerializeField] private Gloves hand; 
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void ClickedButton()
    {
        if (hand == Gloves.Left)
        {
            TagManager.ChangeLeftGloveTag(button.tag);
        }
        else if (hand == Gloves.Right)
        {
            TagManager.ChangeRightGloveTag(button.tag);
        }
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        button.onClick.AddListener(ClickedButton);
    }

    private void UnsubscribeEvents()
    {
        button.onClick.RemoveListener(ClickedButton);
    }

}
