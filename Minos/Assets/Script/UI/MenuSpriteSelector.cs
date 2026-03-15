using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSpriteSelector : MonoBehaviour {
    public static MenuSpriteSelector instance;

    [Header("Sprites del menú")]
    public GameObject startSprite;
    public GameObject settingsSprite;

    [Header("Índices de escenas")]
    public int startSceneIndex = 1;

    [Header("Joystick")]
    public FixedJoystick joystick;

    [Header("Sensibilidad joystick")]
    [Range(0.3f, 0.9f)] public float axisThreshold = 0.6f;

    [Header("Visual")]
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.9f, 0.4f);

    [Header("Sonidos al cambiar selección")]
    public AudioClip[] selectSounds;

    [Header("Sonido al confirmar")]
    public AudioClip confirmSound;

    [Header("Retraso para sonido de confirmar (segundos)")]
    public float confirmDelay = 0.2f;

    [Header("Settings")]
    public GameObject settingsCanvas;

    private GameObject[] options;
    private int currentIndex = 0;
    private float prevVertical = 0f;
    private AudioSource audioSource;
    private bool confirming = false;

    private void Awake() {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start() {
        options = new GameObject[] { startSprite, settingsSprite };
        UpdateVisuals();
    }

    void Update() {
        if (!gameObject.activeInHierarchy) return;
        Debug.Log("confirming: " + confirming + " | settingsCanvas activo: " + settingsCanvas.activeSelf);
        // Escape abre/cierra settings
        if (InputManager.Instance.EscapePressed) {
            InputManager.Instance.ResetEscape();
            if (settingsCanvas != null && !settingsCanvas.activeSelf) {
                settingsCanvas.SetActive(true);
                gameObject.SetActive(false);
                confirming = false;
            }
            return;
        
    }

        if (confirming) return;

        float keyboardVertical = Input.GetAxisRaw("Vertical");
        float joystickVertical = joystick != null ? joystick.Vertical : 0f;
        float vertical = Mathf.Abs(joystickVertical) > 0.1f ? joystickVertical : keyboardVertical;

        bool selectionChanged = false;

        if (vertical > axisThreshold && prevVertical <= axisThreshold) {
            currentIndex = (currentIndex - 1 + options.Length) % options.Length;
            selectionChanged = true;
        } else if (vertical < -axisThreshold && prevVertical >= -axisThreshold) {
            currentIndex = (currentIndex + 1) % options.Length;
            selectionChanged = true;
        }

        if (selectionChanged) {
            UpdateVisuals();
            PlayRandomSelectSound();
        }

        prevVertical = vertical;

        bool confirmTriggered = Input.GetKeyDown(KeyCode.Return) ||
                                Input.GetKeyDown(KeyCode.KeypadEnter) ||
                                Input.GetKeyDown(KeyCode.Space) ||
                                InputManager.Instance.ConfirmPressed;

        if (confirmTriggered) {
            confirming = true;
            InputManager.Instance.ResetConfirm();
            PlaySound(confirmSound);
            Invoke("ActivateCurrentOption", confirmDelay);
        }
    }

    public static void SelectCurrent() {
        if (instance != null && !instance.confirming) {
            instance.confirming = true;
            InputManager.Instance.ResetConfirm();
            instance.PlaySound(instance.confirmSound);
            instance.Invoke("ActivateCurrentOption", instance.confirmDelay);
        }
    }

    private void ActivateCurrentOption() {
        if (currentIndex == 0) {
            SceneManager.LoadScene(startSceneIndex);
        } else {
            if (settingsCanvas != null) {
                settingsCanvas.SetActive(true);
                gameObject.SetActive(false); // desactivar menú
                confirming = false;
            }
        }
    }

    private void UpdateVisuals() {
        foreach (var opt in options) {
            var sr = opt.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = normalColor;
        }
        var selectedSr = options[currentIndex].GetComponent<SpriteRenderer>();
        if (selectedSr != null) selectedSr.color = selectedColor;
    }

    private void PlayRandomSelectSound() {
        if (selectSounds == null || selectSounds.Length == 0) return;
        AudioClip clip = selectSounds[Random.Range(0, selectSounds.Length)];
        if (clip != null) {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
            audioSource.pitch = 1f;
        }
    }

    private void PlaySound(AudioClip clip) {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    public void ResetMenu() {
        confirming = false;
        UpdateVisuals();
    }
}