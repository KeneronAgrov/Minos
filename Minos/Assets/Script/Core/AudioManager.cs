using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    public AudioMixer mainMixer;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        // Cargar valores guardados al iniciar
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SoundVolume", 1f));
    }

    public void SetMusicVolume(float value) {
        // Convertir de 0-1 a decibeles
        float db = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        mainMixer.SetFloat("MusicVolume", db);
    }

    public void SetSFXVolume(float value) {
        float db = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        mainMixer.SetFloat("SFXVolume", db);
    }
}