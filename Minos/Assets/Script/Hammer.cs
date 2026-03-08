using UnityEngine;

public class Hammer : MonoBehaviour {
    public float speed = 10f;
    private Vector2 direction;
    public AudioClip flyingClip;
    public AudioClip impactClip;
    [Range(0f, 1f)] public float flyingVolume = 1f;
    [Range(0f, 2f)] public float impactVolume = 1f;
    private AudioSource audioSource;

    void Awake() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void Launch(Vector2 launchDirection) {
        direction = launchDirection.normalized;

        if (flyingClip != null) {
            audioSource.clip = flyingClip;
            audioSource.loop = true;
            audioSource.volume = flyingVolume;
            audioSource.PlayScheduled(AudioSettings.dspTime);
        }
    }

    void Update() {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Wall")) {
            if (impactClip != null) {
                AudioSource.PlayClipAtPoint(impactClip, transform.position, impactVolume);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}