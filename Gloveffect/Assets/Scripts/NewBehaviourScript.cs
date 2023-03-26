using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public static bool rF,rL,rC, lF,lL,lC;

    public Button butonRF, butonRL, butonRC, butonLF, butonLL, butonLC;
    void Start()
    {
        butonRF.onClick.AddListener(ClickedButton);
    }

    void ClickedButton()
    {
        rF= true;
        Debug.Log("Butona týklandý.");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Ahmet Emir");
    }
}
