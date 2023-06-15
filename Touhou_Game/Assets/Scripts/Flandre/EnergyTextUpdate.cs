using UnityEngine;
using TMPro;

public class EnergyTextUpdate : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private FollowerController followerController;

    private void Update() {
        stateText.text = "Flandre is ";

        switch (followerController.GetActionState())
        {
            case FollowerController.ActionState.Attack:
            stateText.text += "attacking";
            break;
            case FollowerController.ActionState.Defend:
            stateText.text += "defending";
            break;
            case FollowerController.ActionState.Mounted:
            stateText.text += "assisting";
            break;
            case FollowerController.ActionState.Tired:
            stateText.text += "resting";
            break;
        }
    }
}