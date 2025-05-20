using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class GameEnd : MonoBehaviour
{
    [SerializeField] GameObject FinalCam;

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.GameState == 1)
        {
            if (other.CompareTag("Player"))
            {
                FinalCam.SetActive(true); // Activate the final camera
                GetComponent<PlayableDirector>().Play(); // Play the timeline
                GameManager.setGameState(0);
                GetComponent<PlayableDirector>().stopped += gameEnd;
            }
        }
    }

    void gameEnd(PlayableDirector obj)
    {
        StartCoroutine(waitForEnd());
    }
    
    IEnumerator waitForEnd()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.RestartGame();
    }
}
