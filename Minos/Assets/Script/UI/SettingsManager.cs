using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SettingsManager : MonoBehaviour {
    [Header("Audio")]
    public Slider musicSlider;
    public Slider soundSlider;

    [Header("Language")]
    public TMP_Dropdown languageDropdown;

    [Header("Navigation")]
    public Button backButton;

    [Header("Navegacion")]
    public GameObject firstSelected;

    [Header("Canvas")]
    public GameObject settingsCanvasRoot;

    private bool initialized = false;

    void OnEnable() {
        if (musicSlider == null || soundSlider == null ||
            languageDropdown == null || backButton == null) return;

        if (!initialized) {
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
            soundSlider.onValueChanged.AddListener(OnSoundChanged);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            backButton.onClick.AddListener(OnBackPressed);
            initialized = true;
        }

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
        languageDropdown.value = PlayerPrefs.GetInt("Language", 0);

        if (firstSelected != null) {
            EventSystem.current.SetSelectedGameObject(firstSelected);
            Debug.Log("Foco asignado a: " + EventSystem.current.currentSelectedGameObject?.name);
        } else {
            Debug.Log("firstSelected es NULL");
        }
    }

    void Update() {
        if (InputManager.Instance.EscapePressed) {
            InputManager.Instance.ResetEscape();
            OnBackPressed();
        }
    }

    void OnMusicChanged(float value) {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    void OnSoundChanged(float value) {
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value);
    }

    void OnLanguageChanged(int value) {
        PlayerPrefs.SetInt("Language", value);
        PlayerPrefs.Save();
    }

    void OnBackPressed() {
        EventSystem.current.SetSelectedGameObject(null);
        if (MenuSpriteSelector.instance != null) {
            MenuSpriteSelector.instance.gameObject.SetActive(true);
            MenuSpriteSelector.instance.ResetMenu();
        }
        if (settingsCanvasRoot != null)
            settingsCanvasRoot.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}