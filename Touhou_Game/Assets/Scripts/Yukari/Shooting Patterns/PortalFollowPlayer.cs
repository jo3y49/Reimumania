using UnityEngine;

public class PortalFollowPlayer : BossPattern {
    public float portalDistance = 2;
    [SerializeField] private Orientation orientation = Orientation.Horizontal;
    private enum Orientation
    {
        Horizontal,
        Vertical
    }
    private GameObject portal;

    private new void Awake() {
        base.Awake();
        portal = Instantiate(portalPrefab, playerLocation.position, Quaternion.identity);
        portal.transform.GetChild(0).gameObject.transform.position = portal.transform.position + new Vector3(portalDistance,0,0);
        portal.transform.GetChild(1).gameObject.transform.position = portal.transform.position + new Vector3(-portalDistance,0,0);
        if (orientation == Orientation.Vertical)
            portal.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    private void Update() {
        portal.transform.position = playerLocation.position;
    }
}