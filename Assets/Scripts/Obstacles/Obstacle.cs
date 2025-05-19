using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Call the RestartGame method from GameManager
            GameManager.instance.RestartGame();
        }
    }
}
