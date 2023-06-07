using UnityEngine;

public class EventTutorial : MonoBehaviour {
    public TutorialManager tutorialManager;
    public string text = "";

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            tutorialManager.SetText(text);
            Destroy(gameObject);
        }
    }
}