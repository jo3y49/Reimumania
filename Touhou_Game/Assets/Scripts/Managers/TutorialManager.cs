using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    public TextMeshProUGUI tutorialText;

    public void SetText(string text)
    {
        tutorialText.text = text;
    }
}