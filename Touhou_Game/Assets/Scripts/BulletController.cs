using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Collider2D parentCollider; // The collider of the GameObject that instantiated the bullet
    private Collider2D myCollider; // The bullet's own collider

    // Call this method right after instantiating the bullet to pass the reference to the parent
    public void Initialize(Collider2D parentCollider)
    {
        this.parentCollider = parentCollider;
        myCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (parentCollider != null && myCollider != null)
        {
            Physics2D.IgnoreCollision(parentCollider, myCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != parentCollider && !other.gameObject.CompareTag("Projectile")) // Don't destroy if we're still colliding with the parent
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2f);
    }
}