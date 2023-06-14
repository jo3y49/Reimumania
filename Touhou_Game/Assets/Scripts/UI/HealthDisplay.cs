using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {
    public Sprite life, noLife;
    private Image[] hearts;

    private void Start() {
        hearts = GetComponentsInChildren<Image>();
    }

    public void ChangeHearts(int lives)
    {
        int i = 0;

        for (i = 0; i < lives && i < hearts.Length; i++)
        {
            hearts[i].sprite = life;
        }
        while (i < hearts.Length)
        {
            hearts[i].sprite = noLife;
            i++;
        }
    }
}