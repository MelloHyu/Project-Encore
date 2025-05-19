using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; } //Singleton to be used in other scripts

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }




}
