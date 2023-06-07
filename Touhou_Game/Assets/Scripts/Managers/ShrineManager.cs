using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShrineManager : MonoBehaviour
{
    public Button enterTestArea, enterTutorial;

    private void Start() {
        enterTestArea.onClick.AddListener(EnterTestArea);
        enterTutorial.onClick.AddListener(EnterTutorial);
    }
    private void SwitchScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    private void EnterTestArea()
    {
        SwitchScene("TestArea");
    }
    private void EnterTutorial(){
        SwitchScene("Tutorial");
    }
}
