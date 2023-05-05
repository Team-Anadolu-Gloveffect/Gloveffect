using UnityEngine;
using UnityEngine.UI;
using Enums;

public class SpellSelectionButtonController : MonoBehaviour
{
    [SerializeField] private Gloves hand; 
    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void ClickedButton()
    {
        if (hand == Gloves.Left)
        {
            TagManager.ChangeLeftGloveTag(_button.tag);
        }
        else if (hand == Gloves.Right)
        {
            TagManager.ChangeRightGloveTag(_button.tag);
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
        _button.onClick.AddListener(ClickedButton);
    }

    private void UnsubscribeEvents()
    {
        _button.onClick.RemoveListener(ClickedButton);
    }

}
