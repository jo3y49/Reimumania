using System.Collections;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float duration = .5f;

    public IEnumerator MoveToPosition(Vector3 newPosition)
    {
        // Save the time when the movement started
        float startTime = Time.time;

        // Save the starting position of the object
        Vector3 initialPosition = transform.position;

        // Calculate the time spent moving
        float elapsedTime = Time.time - startTime;
        
        // Move the object until the duration has been reached
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, newPosition, elapsedTime / duration);
            
            elapsedTime = Time.time - startTime;

            yield return null;
        }

        // Ensure the object is exactly at the new position
        transform.position = newPosition;
    }
}
