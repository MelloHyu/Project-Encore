using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.Playables;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; } //Singleton to be used in other scripts
    public static int GameState { get; private set; } = 0; // Game state variable

    PlayableDirector director; // Reference to the PlayableDirector component
    [SerializeField] private GameObject startGameUI; // UI element to show at the start of the game

    private void Awake()
    {
        GameState = 0;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        director = GetComponent<PlayableDirector>(); // Get the PlayableDirector component
        director.stopped += GameAfterTimeline;
    }

    void OnEnable()
    {
        InputManager.instance.PlayerActions.Enable();
        InputManager.instance.PlayerActions.Player.Interact.performed += StartGame;
    }

    void OnDisable()
    {
        InputManager.instance.PlayerActions.Player.Interact.performed -= StartGame;
    }

    void StartGame(InputAction.CallbackContext context)
    {
        if (GameState == 0)
        {
            director.Play(); // Play the timeline
        }
    }

    void GameAfterTimeline(PlayableDirector obj)
    {
        startGameUI.SetActive(false); // Hide the start game UI
        // This function is called when the timeline stops
        GameState = 1; // Set game state to 1
        Time.timeScale = 1; // Resume the game
        InputManager.instance.PlayerActions.Player.Interact.performed -= StartGame; // Unsubscribe from the start game event
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
        Debug.Log("Game is quitting");
    }

    public void MainMenu()
    {
        RestartGame();
        Time.timeScale = 1;
        GameState = 0;
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Pause the game
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
    }

    public void Game()
    {
        Time.timeScale = 1; // Start the game
        GameState = 1; // Set game state to 1
    }
}
