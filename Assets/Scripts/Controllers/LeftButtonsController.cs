using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeftButtonsController : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickedButton);
    }

    public void ClickedButton()
    {
        TagManager.ChangeLeftGloveTag(button.tag);
        Debug.Log("Butona týklandý.");
    }
}
