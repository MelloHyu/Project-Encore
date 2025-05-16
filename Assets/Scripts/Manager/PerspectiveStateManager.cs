using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(1)]
public class PerspectiveStateManager : MonoBehaviour
{
    public static PerspectiveStateManager instance; // Singleton to be used in other scripts
    [SerializeField] Transform player; // Player object
    [SerializeField] int groundLayer; // Layer mask for ground detection
    [SerializeField] GameObject Level2D; // Level object for 2D perspective
    [SerializeField] CinemachineCamera perspectiveCam, orthoCam; // Camera objects for 2D and 3D perspectives

    bool is3D = false;
    float currZposition, iniZposition; // To store the initial Z position of the player

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); // Destroy duplicate instances

        if (is3D)
        {
            orthoCam.Priority = 0; // Disable 2D camera
            perspectiveCam.Priority = 10; // Enable 3D camera
            Level2D.SetActive(false); // Disable 2D Level
        }
        else
        {
            orthoCam.Priority = 10; // Enable 2D camera
            perspectiveCam.Priority = 0; // Disable 3D camera
            Level2D.SetActive(true); // Enable 2D Level
        }
    }

    void OnEnable()
    {
        InputManager.instance.PlayerActions.Enable();
        InputManager.instance.PlayerActions.Player.Interact.performed += changePerspective;
        InputManager.instance.PlayerActions.Player.Interact.performed += perspectiveChange;

        Debug.Log(InputManager.instance);
    }

    void OnDisable()
    {
        InputManager.instance.PlayerActions.Player.Interact.performed -= changePerspective;
        InputManager.instance.PlayerActions.Player.Interact.performed -= perspectiveChange;
    }

    void changePerspective(InputAction.CallbackContext context)
    {
        // Code to change the perspective of the camera
        is3D = !is3D; // Toggle between 2D and 3D
        Debug.Log("Perspective changed!");

        if (is3D)
        {
            orthoCam.Priority = 0; // Disable 2D camera
            perspectiveCam.Priority = 10; // Enable 3D camera
            Level2D.SetActive(false); // Disable 2D Level
        }
        else
        {
            orthoCam.Priority = 10; // Enable 2D camera
            perspectiveCam.Priority = 0; // Disable 3D camera
            Level2D.SetActive(true); // Enable 2D Level
        }
    }

    public bool getPerspectiveState()
    { //To get the perspective of the game
        return is3D;
    }


    //Function to store the last position of the player in 3D perspective before switch
    void perspectiveChange(InputAction.CallbackContext context)
    {
        if (PerspectiveStateManager.instance.getPerspectiveState())
        {
            currZposition = player.transform.position.z; // Store the current Z position of the player
            player.transform.position = new Vector3(transform.position.x, transform.position.y, iniZposition);
        }
        else
        {
            RaycastHit hit;
            Ray ray = new Ray(player.transform.position + Vector3.forward * 1f, Vector3.forward);

            if (Physics.Raycast(ray, out hit, 10f, groundLayer)) //Casts a ray to find the 3D equivalent of the 2D tile
            {
                currZposition = hit.point.z; // Store the current Z position of the player
            }
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, currZposition); // Move the player in 2D perspective
        }
    }
}
