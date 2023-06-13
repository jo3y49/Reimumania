using UnityEngine;

public class SlowFloatMovement : BossMovement {
    public float speed = 1f;
    public float amplitude = 5f;

    private float initialX;

    private new void Start() {
        base.Start();
        // Initialize the boss at the top of the arena
        transform.position = new Vector3(transform.position.x, topLocation, transform.position.z);
        initialX = transform.position.x;
    }

    private void Update() {
        // Move the boss left and right using a sine wave
        float x = initialX + amplitude * Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}