using UnityEngine;

public class TeleportationMovement : BossMovement {
    private float nextTeleportTime = 0f;
    private int nextTeleport = 0;
    private Vector2[] teleportLocations = new Vector2[4];
    private new void Start()
    {
        base.Start();
        teleportLocations[0] = new Vector2(0, topLocation);
        teleportLocations[1] = new Vector2(rightLocation, 0);
        teleportLocations[2] = new Vector2(0, bottomLocation);
        teleportLocations[3] = new Vector2(leftLocation, 0);
    }

    private void Update() {
        if (Time.time > nextTeleportTime)
        {
            Teleport();
            nextTeleportTime = Time.time + 5/moveSpeed;
        }
    }

    private void Teleport()
    {
        gameObject.transform.position = teleportLocations[nextTeleport];

        if (nextTeleport < teleportLocations.Length - 1)
        {
            nextTeleport += 1;
        } else {
            nextTeleport = 0;
        }
    }

    private void OnDisable() {
        nextTeleport = 0;
    }

}