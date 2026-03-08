using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }
    public bool ConfirmPressed { get; private set; }
    private bool blocked = false;
    private bool pendingReset = false;

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
        blocked = true;
        ConfirmPressed = false;
        pendingReset = false;
        Debug.Log("OnSceneLoaded - escena: " + scene.name);
    }

    void Update() {
        if (pendingReset) {
            ConfirmPressed = false;
            pendingReset = false;
        }
        blocked = false;

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (!blocked) ConfirmPressed = true;
        }
    }

    void LateUpdate() {
        if (ConfirmPressed)
            pendingReset = true;
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
}