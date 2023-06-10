using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Collider2D parentCollider; // The collider of the GameObject that instantiated the bullet
    private Collider2D myCollider; // The bullet's own collider
    private bool isReflecting = false;
    public float bulletDamage = 5f;
    public static Action<GameObject> protectPlayer;

    // Call this method right after instantiating the bullet to pass the reference to the parent
    public void Initialize(Collider2D parentCollider)
    {
        this.parentCollider = parentCollider;
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isReflecting)
        {
            if (CanHit(other))
            {
                Shootable shootable = other.GetComponent<Shootable>();
                if (shootable != null)
                    shootable.Shot(bulletDamage);
                Destroy(gameObject);
            } 
            else if (other.gameObject.CompareTag("Follower"))
            {
                other.gameObject.GetComponent<FollowerController>().Dodge(transform);
            } 
            else if (other.gameObject.CompareTag("Collector")  && !parentCollider.gameObject.CompareTag("Hit Box"))
            {
                protectPlayer?.Invoke(gameObject);
            }
        }
    }

    private bool CanHit(Collider2D other) 
    {
        return other != parentCollider && other.gameObject.activeSelf &&
            !(other.gameObject.CompareTag("Follower") && parentCollider.gameObject.CompareTag("Hit Box")) &&
            !(other.gameObject.CompareTag("Enemy") && parentCollider.gameObject.CompareTag("Enemy")) &&
            (other.gameObject.CompareTag("Hit Box") || other.gameObject.CompareTag("Enemy") ||
            other.gameObject.CompareTag("Environment"));
    }

    public void Reflect(Collider2D newParentColilider)
    {
        isReflecting = true;
        parentCollider = newParentColilider;
        GetComponent<Rigidbody2D>().velocity *= -1;
        isReflecting = false;
    }
}
