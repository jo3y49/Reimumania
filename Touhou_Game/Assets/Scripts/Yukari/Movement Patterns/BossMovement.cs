using UnityEngine;

public abstract class BossMovement : MonoBehaviour{
    public float arenaWidth, arenaHeight, leftLocation, rightLocation, topLocation, bottomLocation;
    public BossData bossData;

    protected void Start() {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        bossData = boss.GetComponent<BossData>();
        arenaHeight = bossData.arenaHeight;
        arenaWidth = bossData.arenaWidth;
        leftLocation = bossData.leftLocation;
        rightLocation = bossData.rightLocation;
        topLocation = bossData.topLocation;
        bottomLocation = bossData.bottomLocation;
    }
}