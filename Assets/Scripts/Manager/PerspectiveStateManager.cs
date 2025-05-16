using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(1)]
public class PerspectiveStateManager : MonoBehaviour
{
    public static PerspectiveStateManager instance; // Singleton to be used in other scripts

    [SerializeField]GameObject perspectiveCam, Level2D, orthoCam; // Camera objects for 2D and 3D perspectives

    bool is3D = true;

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject); // Destroy duplicate instances
    }

    void OnEnable()
    {
        InputManager.instance.PlayerActions.Enable();
        InputManager.instance.PlayerActions.Player.Interact.performed += changePerspective;

        Debug.Log(InputManager.instance);
    }

    void OnDisable()
    {
        InputManager.instance.PlayerActions.Player.Interact.performed -= changePerspective;
    }

    void changePerspective(InputAction.CallbackContext context)
    {
        // Code to change the perspective of the camera
        is3D = !is3D; // Toggle between 2D and 3D
        Debug.Log("Perspective changed!");

        if(is3D){
            orthoCam.SetActive(false); // Disable 2D camera
            Level2D.SetActive(false); // Disable 2D Level
        }
        else{
            orthoCam.SetActive(true); // Enable 2D camera
            Level2D.SetActive(true); // Enable 2D Level
        }
    }

    public bool getPerspectiveState(){ //To get the perspective of the game
        return is3D;
    }
}
