using UnityEngine;

public class HitBoxData : MonoBehaviour, Shootable {
    private PlayerData playerData;

    private void Awake() {
        playerData = GetComponentInParent<PlayerData>();
    }
    public void Shot(float bulletDamage)
    {
        transform.parent.GetComponent<PlayerData>().Shot(bulletDamage);
    }
}