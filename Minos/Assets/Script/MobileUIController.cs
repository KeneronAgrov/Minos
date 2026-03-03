using UnityEngine;

public class MobileUIController : MonoBehaviour {
    [SerializeField]
    private GameObject mobileControls;  // ← Arrastra AQUÍ el Fixed Joystick o el Canvas completo de controles móviles

    void Awake() {
        if (mobileControls == null) {
            Debug.LogWarning("MobileUIController: Asigna el joystick o Canvas en 'mobileControls' del Inspector.");
            return;
        }

        // 1. Chequeo principal: ¿Es plataforma móvil real o simulada?
        bool isMobile = UnityEngine.Device.Application.isMobilePlatform;

        // 2. En Editor: si estamos en Simulator view, forzamos true (porque a veces falla el chequeo)
#if UNITY_EDITOR
        // Detectamos si el Game view está en modo Simulator mirando si hay "Simulator" en el título o por resolución típica de móvil
        // Pero la forma más simple y confiable: si la altura > ancho → probablemente móvil/Simulator (portrait)
        if (UnityEngine.Screen.height > UnityEngine.Screen.width) {
            isMobile = true;  // Fuerza mostrar en portrait (típico de Simulator)
        }
#endif

        mobileControls.SetActive(isMobile);

        // Log para diagnosticar
        Debug.Log(
            $"MobileUI → Mostrar: {isMobile} | " +
            $"Device.isMobilePlatform: {UnityEngine.Device.Application.isMobilePlatform} | " +
            $"Screen: {UnityEngine.Screen.width}x{UnityEngine.Screen.height} | " +
            $"Editor: {Application.isEditor}"
        );
    }
}