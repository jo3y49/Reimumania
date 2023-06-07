using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float minOrtho = 1f;
    public float midOrtho = 10f;
    public float maxOrtho = 20f;
    public KeyCode cameraToggle = KeyCode.Tab;

    void Update()
    {
        if (Input.GetKeyDown(cameraToggle))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        float orthoSize = Camera.main.orthographicSize;

        if (Camera.main.orthographicSize == minOrtho)
        {
            orthoSize = midOrtho;
        } else if (Camera.main.orthographicSize == midOrtho)
        {
            orthoSize = maxOrtho;
        } else 
        {
            orthoSize = minOrtho;
        }

        Camera.main.orthographicSize = Mathf.Clamp(orthoSize, minOrtho, maxOrtho);
    }
}
