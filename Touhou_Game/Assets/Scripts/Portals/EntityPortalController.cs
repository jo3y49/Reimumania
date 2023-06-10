using UnityEngine;
using System.Collections;

public class EntityPortalController : MonoBehaviour {
    private EntityPortalConnector entityPortalConnector;
    private bool active = true;

    private void Awake() {
        entityPortalConnector = GetComponentInParent<EntityPortalConnector>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Projectile") && active)
        {
            entityPortalConnector.Receive(other.gameObject.GetComponent<BulletController>(), gameObject);
        }
    }

    public IEnumerator Cooldown(float cooldown)
    {
        active = false;

        yield return new WaitForSeconds(cooldown);

        active = true;
    }
}