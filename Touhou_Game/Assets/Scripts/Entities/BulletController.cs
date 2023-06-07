using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Collider2D parentCollider; // The collider of the GameObject that instantiated the bullet
    private Collider2D myCollider; // The bullet's own collider
    public float bulletDamage = 5f;

    // Call this method right after instantiating the bullet to pass the reference to the parent
    public void Initialize(Collider2D parentCollider)
    {
        this.parentCollider = parentCollider;
        myCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (parentCollider != null && myCollider != null)
        {
            Physics2D.IgnoreCollision(parentCollider, myCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CanHit(other))
        {
            Shootable shootable = other.GetComponent<Shootable>();
            if (shootable != null)
                shootable.Shot(bulletDamage);
                
            if (!other.gameObject.CompareTag("Follower"))
                Destroy(gameObject);
        }
    }

    private bool CanHit(Collider2D other) 
    {
        return other != parentCollider && other.gameObject.activeSelf &&
            !(other.gameObject.CompareTag("Follower") && parentCollider.gameObject.CompareTag("Player")) &&
            !(other.gameObject.CompareTag("Enemy") && parentCollider.gameObject.CompareTag("Enemy")) &&
            (other.gameObject.CompareTag("Hit Box") || other.gameObject.CompareTag("Enemy") ||
            other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("Follower"));
    }
}
