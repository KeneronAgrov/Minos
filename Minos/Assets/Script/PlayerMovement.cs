using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        // 🔒 Evita que el personaje rote
        rb.freezeRotation = true;
    }

    void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Invertido
        float vertical = Input.GetAxisRaw("Vertical");

        // 🚫 Evitar movimiento diagonal
        if (horizontal != 0) {
            vertical = 0;
        }

        movement = new Vector2(horizontal, vertical);
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}