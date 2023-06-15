using UnityEngine;
using UnityEngine.UI;

public class BombDisplay : MonoBehaviour {
    public Sprite bomb, noBomb;
    private Image[] bombs;

    private void Start() {
        bombs = GetComponentsInChildren<Image>();
    }

    public void ChangeBombs(int newBombs)
    {
        int i = 0;

        for (i = 0; i < newBombs && i < bombs.Length; i++)
        {
            bombs[i].sprite = bomb;
        }
        while (i < bombs.Length)
        {
            bombs[i].sprite = noBomb;
            i++;
        }
    }
}