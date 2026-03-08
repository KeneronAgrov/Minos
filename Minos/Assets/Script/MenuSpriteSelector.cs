using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSpriteSelector : MonoBehaviour {
    // Singleton para llamar desde botón UI
    public static MenuSpriteSelector instance;

    [Header("Sprites del menú")]
    public GameObject startSprite;
    public GameObject settingsSprite;

    [Header("Índices de escenas")]
    public int startSceneIndex = 1;
    public int settingsSceneIndex = 2;

    [Header("Joystick")]
    public FixedJoystick joystick;

    [Header("Sensibilidad joystick")]
    [Range(0.3f, 0.9f)] public float axisThreshold = 0.6f;

    [Header("Visual")]
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.9f, 0.4f);

    [Header("Sonidos al cambiar selección (elige 3 diferentes)")]
    public AudioClip[] selectSounds;      // ← Arrastra aquí 3 clips diferentes

    [Header("Sonido al confirmar")]
    public AudioClip confirmSound;

    [Header("Retraso para sonido de confirmar (segundos)")]
    public float confirmDelay = 0.2f;     // ← Ajusta según duración de tu clip

    private GameObject[] options;
    private int currentIndex = 0;
    private float prevVertical = 0f;
    private AudioSource audioSource;

    private void Awake() {
        instance = this;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (selectSounds == null || selectSounds.Length == 0) {
            Debug.LogWarning("No hay selectSounds asignados en MenuSpriteSelector");
        }
    }

    void Start() {
        options = new GameObject[] { startSprite, settingsSprite };
        if (options.Length > 0) {
            UpdateVisuals();
        }
    }

    void Update() {
        if (!gameObject.activeInHierarchy) return;

        float keyboardVertical = Input.GetAxisRaw("Vertical");
        float joystickVertical = joystick != null ? joystick.Vertical : 0f;

        float vertical = joystickVertical;
        if (Mathf.Abs(joystickVertical) < 0.1f) {
            vertical = keyboardVertical;
        }

        bool selectionChanged = false;

        if (vertical > axisThreshold && prevVertical <= axisThreshold) {
            currentIndex--;
            if (currentIndex < 0) currentIndex = options.Length - 1;
            selectionChanged = true;
        } else if (vertical < -axisThreshold && prevVertical >= -axisThreshold) {
            currentIndex++;
            if (currentIndex >= options.Length) currentIndex = 0;
            selectionChanged = true;
        }

        if (selectionChanged) {
            UpdateVisuals();
            PlayRandomSelectSound();
        }

        prevVertical = vertical;

        // Confirmar (sonido PRIMERO, luego retraso para cargar escena)
        if (Input.GetKeyDown(KeyCode.Return) ||
             Input.GetKeyDown(KeyCode.KeypadEnter) ||
             Input.GetKeyDown(KeyCode.Space) ||
             InputManager.Instance.ConfirmPressed) {
            Debug.Log("Confirm pressed");
            
            PlaySound(confirmSound);
            Invoke("ActivateCurrentOption", confirmDelay);
        }
    }


    // Método público para el botón UI del móvil
    public static void SelectCurrent() {
        if (instance != null) {
            instance.PlaySound(instance.confirmSound);
            instance.Invoke("ActivateCurrentOption", instance.confirmDelay);
        }
    }

    private void PlayRandomSelectSound() {
        if (selectSounds == null || selectSounds.Length == 0 || audioSource == null)
            return;

        int randomIndex = Random.Range(0, selectSounds.Length);
        AudioClip clip = selectSounds[randomIndex];

        if (clip != null) {
            audioSource.pitch = Random.Range(0.9f, 1.1f);  // Variación sutil
            audioSource.PlayOneShot(clip);
            audioSource.pitch = 1f;  // Reset
        }
    }

    private void PlaySound(AudioClip clip) {
        if (clip != null && audioSource != null) {
            audioSource.PlayOneShot(clip);
        }
    }

    private void UpdateVisuals() {
        foreach (var opt in options) {
            var sr = opt.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = normalColor;
        }

        if (options.Length > 0 && currentIndex >= 0 && currentIndex < options.Length) {
            var selectedSr = options[currentIndex].GetComponent<SpriteRenderer>();
            if (selectedSr != null) selectedSr.color = selectedColor;
        }
    }

    private void ActivateCurrentOption() {
        int targetIndex = (currentIndex == 0) ? startSceneIndex : settingsSceneIndex;
        SceneManager.LoadScene(targetIndex);
    }
}