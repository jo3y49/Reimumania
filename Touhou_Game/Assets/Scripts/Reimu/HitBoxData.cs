using UnityEngine;

public class HitBoxData : MonoBehaviour, Shootable {
    public void Shot(float bulletDamage)
    {
        transform.parent.GetComponent<PlayerData>().Shot(bulletDamage);
    }
}