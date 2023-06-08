using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject pressAnyButtonPanel, mainMenuPanel;
    private bool pressAnyButtonSkipped;

    [SerializeField] private TMP_Text volumeValueText;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject comfirmationPrompt;
    [SerializeField] private float defaultVolume = 1f;

    [SerializeField] private TMP_Text controllerSensValue;
    [SerializeField] private Slider controllerSensSlider;
    [SerializeField] private int defaultSens = 4;
    public int mainControllerSens = 4;

    [SerializeField] private Toggle invertYToggle;

    private bool _isFullscreen;
    [SerializeField] private Toggle fullscreenToggle;

    private void Update()
    {
        if (Input.anyKeyDown && !pressAnyButtonSkipped) PressAnyButton();
    }

    private void PressAnyButton()
    {
        pressAnyButtonPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        pressAnyButtonSkipped = true;
    }

    public void GameStart()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeValueText.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSens(float sens)
    {
        mainControllerSens = Mathf.RoundToInt(sens);
        controllerSensValue.text = sens.ToString("0");
    }

    public void SetFullscreen(bool isFullscreen)
    {
        _isFullscreen = isFullscreen;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetInt("masterFullscreen", (_isFullscreen ? 1 : 0));
        Screen.fullScreen = _isFullscreen;
        StartCoroutine(ConfirmationBox());
    }

    public void GameplayApply()
    {
        if(invertYToggle.isOn)  PlayerPrefs.SetInt("masterInvertY", 1);
        else  PlayerPrefs.SetInt("masterInvertY", 0);
        
        PlayerPrefs.SetFloat("masterSens", mainControllerSens);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeValueText.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            controllerSensValue.text = defaultSens.ToString("0");
            controllerSensSlider.value = defaultSens;
            mainControllerSens = defaultSens;
            invertYToggle.isOn = false;
            GameplayApply();
        }

        if (MenuType == "Graphics")
        {
            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;
            GraphicsApply();
        }
    }
    
    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(1);
        comfirmationPrompt.SetActive(false);
    }
}
