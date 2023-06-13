using UnityEngine;

public class PortalFollowPlayer : BossPattern {
    public float portalDistance = 2;
    public float rotationSpeed = 20;
    private GameObject portal;

    private new void Start() {
        base.Start();
        portal = Instantiate(portalPrefab, playerLocation.position, Quaternion.identity);
        portal.transform.GetChild(0).gameObject.transform.position = portal.transform.position + new Vector3(portalDistance,0,0);
        portal.transform.GetChild(1).gameObject.transform.position = portal.transform.position + new Vector3(-portalDistance,0,0);
    }

    private void OnEnable() {
        portal = Instantiate(portalPrefab, playerLocation.position, Quaternion.identity);
        portal.transform.GetChild(0).gameObject.transform.position = portal.transform.position + new Vector3(portalDistance,0,0);
        portal.transform.GetChild(1).gameObject.transform.position = portal.transform.position + new Vector3(-portalDistance,0,0);
    }

    private void OnDisable() {
        Destroy(portal);
    }

    private void Update() {
        portal.transform.position = playerLocation.position;
        portal.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}