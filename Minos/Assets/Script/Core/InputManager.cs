using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }
    public bool ConfirmPressed { get; private set; }
    public bool EscapePressed { get; private set; }
    private bool blocked = false;
    private bool pendingReset = false;
    private bool pendingEscapeReset = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        ConfirmPressed = false;
        EscapePressed = false;
        pendingReset = false;
        pendingEscapeReset = false;
        blocked = true;
        Debug.Log("OnSceneLoaded - escena: " + scene.name);
    }

    void Update() {
        if (pendingReset) {
            ConfirmPressed = false;
            pendingReset = false;
        }

        if (pendingEscapeReset) {
            EscapePressed = false;
            pendingEscapeReset = false;
        }

        if (blocked) {
            blocked = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Space)) {
            ConfirmPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            EscapePressed = true;
    }

    void LateUpdate() {
        if (ConfirmPressed) pendingReset = true;
        if (EscapePressed) pendingEscapeReset = true;
    }

    public void OnConfirmButton() {
        Debug.Log("OnConfirmButton - blocked: " + blocked);
        if (!blocked) {
            ConfirmPressed = true;
            Debug.Log("Boton presionado");
        }
    }

    public void ResetConfirm() {
        ConfirmPressed = false;
        pendingReset = false;
    }

    public void ResetEscape() {
        EscapePressed = false;
        pendingEscapeReset = false;
    }
}