using UnityEngine;
using TMPro; 
public class Timer : MonoBehaviour
{
    private float LowestScore = 999999999f; // Total time duration in seconds

    private float timer;

    [SerializeField] private TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component
    [SerializeField] private TextMeshProUGUI lowestScoreText; // Reference to the TextMeshProUGUI component for the lowest score

    private void Start()
    {
        LowestScore = PlayerPrefs.GetInt("LowestScore", 999999999); // Load the lowest score from PlayerPrefs, default to a high value
        UpdateTimerText(lowestScoreText, LowestScore); // Update the text with the lowest score
        timer = 0f; // Initialize the timer with the total duration
        UpdateTimerText(timerText, timer); // Update the text at the start
    }

    private void Update()
    {
        timer += Time.deltaTime;
        UpdateTimerText(timerText,timer);
    }

    private void UpdateTimerText(TextMeshProUGUI textBox, float timer)
    {
        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        // Format the time as MM:SS
        string formattedTime = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        // Update the TextMeshProUGUI component with the formatted time
        textBox.text = formattedTime;
    }

    public void SetLowestScore()
    {
        if (timer < LowestScore)
        {
            LowestScore = timer; // Update the lowest score if the current timer is lower
            PlayerPrefs.SetInt("LowestScore", Mathf.FloorToInt(LowestScore)); // Save the lowest score to PlayerPrefs
            PlayerPrefs.Save(); // Save the changes to PlayerPrefs
        }
    }

}
