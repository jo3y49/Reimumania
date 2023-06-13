using UnityEngine;

public class SlowFloatMovement : BossMovement {
    public float amplitude = 5f;
    public float topBuffer = 1f;

    private float initialX;

    private new void Start() {
        base.Start();
        transform.position = new Vector3(0, topLocation - topBuffer, transform.position.z);
        initialX = transform.position.x;
    }

    private void OnEnable() {
        transform.position = new Vector3(0, topLocation - topBuffer, transform.position.z);
        initialX = transform.position.x;
    }

    private void Update() {
        // Move the boss left and right using a sine wave
        float x = initialX + amplitude * Mathf.Sin(Time.time * moveSpeed);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}