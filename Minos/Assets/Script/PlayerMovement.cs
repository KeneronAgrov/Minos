using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public AudioClip[] footstepClips;
    private float stepInterval = 0.4f;
    public FixedJoystick joystick;
    private AudioSource audioSource;
    private float stepTimer;
    private float xRange = 9f;
    private float yRange = 4.5f;
    public GameObject hammerPrefab;
    private bool hammerUsed = false;
    private Vector2 lastDirection = Vector2.up;
    private float inputTimer = 0f;
    private bool inputReady = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update() {
        if (!inputReady) {
            inputTimer += Time.deltaTime;
            if (inputTimer >= 0.5f)
                inputReady = true;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (transform.position.x < -xRange)
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        if (transform.position.x > xRange)
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        if (transform.position.y < -yRange)
            transform.position = new Vector3(transform.position.x, -yRange, transform.position.z);
        if (transform.position.y > yRange)
            transform.position = new Vector3(transform.position.x, yRange, transform.position.z);

        if (joystick != null) {
            if (Mathf.Abs(joystick.Horizontal) > 0.1f || Mathf.Abs(joystick.Vertical) > 0.1f) {
                horizontal = joystick.Horizontal;
                vertical = joystick.Vertical;
            }
        }

        float absH = Mathf.Abs(horizontal);
        float absV = Mathf.Abs(vertical);
        if (absH > 0.1f && absV > 0.1f) {
            if (absH > absV) vertical = 0f;
            else horizontal = 0f;
        }

        movement = new Vector2(horizontal, vertical);
        if (movement.magnitude > 0.1f)
            lastDirection = movement;

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

        if (movement.magnitude > 0.1f) {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void LateUpdate() {
        if (inputReady && InputManager.Instance != null && InputManager.Instance.ConfirmPressed && !hammerUsed) {
            hammerUsed = true;
            GameObject hammer = Instantiate(hammerPrefab, transform.position, transform.rotation);
            hammer.GetComponent<Hammer>().Launch(movement != Vector2.zero ? movement : lastDirection);
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