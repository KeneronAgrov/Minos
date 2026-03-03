using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public AudioClip[] footstepClips;
    public float stepInterval = 0.4f;
    public FixedJoystick joystick;
    private AudioSource audioSource;
    private float stepTimer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true;

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Si existe joystick móvil, usarlo
        if (joystick != null) {
            if (Mathf.Abs(joystick.Horizontal) > 0.1f ||
                Mathf.Abs(joystick.Vertical) > 0.1f) {
                horizontal = joystick.Horizontal;
                vertical = joystick.Vertical;
            }
        }

        // 🚫 Evitar movimiento diagonal
        float absH = Mathf.Abs(horizontal);
        float absV = Mathf.Abs(vertical);

        if (absH > 0.1f && absV > 0.1f) {
            if (absH > absV) {
                vertical = 0f;     // prioriza horizontal
            } else {
                horizontal = 0f;   // prioriza vertical
            }
        }

        movement = new Vector2(horizontal, vertical);
        bool isMoving = movement.magnitude > 0.1f;

        if (isMoving) {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval) {
                PlayFootstep();
                stepTimer = 0f;
            }
        } else {
            stepTimer = stepInterval;
        }
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
    void PlayFootstep() {
        if (footstepClips.Length == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.PlayOneShot(clip);
    }
}