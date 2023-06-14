using UnityEngine;

public class WallManager : MonoBehaviour {
    [SerializeField] int trees, treeGap;
    [SerializeField] GameObject blankTree;
    private BoxCollider2D hitbox;
    private enum Orientation{ Horizontal, Vertical }
    [SerializeField] private Orientation orientation;

    private void Awake() {
        hitbox = GetComponent<BoxCollider2D>();
        Vector3 treeOffset = Vector3.zero;
        int i = 0;

        switch (orientation)
        {
            case Orientation.Vertical:
            treeOffset = new Vector3(0,-treeGap,0);
            hitbox.offset += new Vector2(0,-1f * trees);
            hitbox.size += new Vector2(0, 2f * trees);
            break;
            case Orientation.Horizontal:
            treeOffset = new Vector3(treeGap,0,0);
            hitbox.offset += new Vector2(1f * trees,0);
            hitbox.size += new Vector2(2f * trees,0);
            break;
        }

        for (i=0;i<trees;i++)
        {
            GameObject newTree = Instantiate(blankTree, transform.position + treeOffset * (i+1), Quaternion.identity);
        }

        
    }
}