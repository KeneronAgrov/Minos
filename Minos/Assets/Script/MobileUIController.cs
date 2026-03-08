using UnityEngine;

public class MobileUIController : MonoBehaviour {
    [SerializeField] private GameObject mobileControls;
    [SerializeField] private GameObject confirmButton;

    void Awake() {
        if (mobileControls == null) {
            Debug.LogWarning("MobileUIController: Asigna el joystick o Canvas en 'mobileControls' del Inspector.");
            return;
        }

        bool isMobile = UnityEngine.Device.Application.isMobilePlatform;

#if UNITY_EDITOR
        if (UnityEngine.Screen.height > UnityEngine.Screen.width)
            isMobile = true;
#endif

        mobileControls.SetActive(isMobile);

        if (confirmButton != null)
            confirmButton.SetActive(isMobile);

        Debug.Log(
            $"MobileUI → Mostrar: {isMobile} | " +
            $"Device.isMobilePlatform: {UnityEngine.Device.Application.isMobilePlatform} | " +
            $"Screen: {UnityEngine.Screen.width}x{UnityEngine.Screen.height} | " +
            $"Editor: {Application.isEditor}"
        );
    }
}