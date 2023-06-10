using UnityEngine;

public class EntityPortalConnector : MonoBehaviour {
    public GameObject portal1, portal2;
    public float portalCooldown = .5f;
    public int durability = 5;
    private GameObject lastPortal;
    private Collider2D portal1Collider, portal2Collider, lastCollider;
    private EntityPortalController portal1Controller, portal2Controller, lastController;

    private void Awake() {
        lastPortal = portal1;
        portal1Collider = portal1.GetComponent<Collider2D>();
        portal2Collider = portal2.GetComponent<Collider2D>();
        lastCollider = portal1Collider;
        portal1Controller = portal1.GetComponent<EntityPortalController>();
        portal2Controller = portal2.GetComponent<EntityPortalController>();
        lastController = portal1Controller;
    }

    public void Receive(BulletController bullet, GameObject portal)
    {
        if (portal == portal1)
        {
            lastPortal = portal2;
            lastCollider = portal2Collider;
            lastController = portal2Controller;
        }
        else if (portal == portal2)
        {
            lastPortal = portal1;
            lastCollider = portal1Collider;
            lastController = portal1Controller;
        }

        StartCoroutine(lastController.Cooldown(portalCooldown));

        bullet.Warp(lastPortal.transform.position, lastCollider);

        durability -= 1;

        if (durability <= 0)
            Destroy(gameObject);
    }
}