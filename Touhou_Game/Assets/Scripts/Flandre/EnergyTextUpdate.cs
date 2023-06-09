using UnityEngine;
using TMPro;

public class EnergyTextUpdate : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private FollowerController followerController;

    private void Update() {
        text.text = "Flandre's Energy: " + followerController.energy.ToString();
    }
}