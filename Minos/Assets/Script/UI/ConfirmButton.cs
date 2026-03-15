using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConfirmButton : MonoBehaviour, IPointerDownHandler {
    private Button btn;

    void Start() {
        btn = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (btn.interactable) {
            btn.interactable = false;
            InputManager.Instance.OnConfirmButton(); // ← solo esto
        }
    }

    void OnEnable() {
        if (btn != null)
            btn.interactable = true;
    }
}