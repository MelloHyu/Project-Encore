using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(1)]
public class PerspectiveStateManager : MonoBehaviour
{
    public static PerspectiveStateManager instance; // Singleton to be used in other scripts
    [SerializeField] Transform player; // Player object
    [SerializeField] LayerMask groundLayer; // Layer mask for ground detection
    [SerializeField] GameObject Level2D, Level3D; // Level object for 2D perspective
    [SerializeField] CinemachineCamera perspectiveCam, orthoCam; // Camera objects for 2D and 3D perspectives

    bool is3D = false;
    float currZposition, iniZposition; // To store the initial Z position of the player

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else Destroy(gameObject); // Destroy duplicate instances


        if (is3D)
        {
            orthoCam.Priority = 0; // Disable 2D camera
            perspectiveCam.Priority = 10; // Enable 3D camera
            Level2D.SetActive(false); // Disable 2D Level
            Level3D.SetActive(true); // Enable 3D Level
        }
        else
        {
            orthoCam.Priority = 10; // Enable 2D camera
            perspectiveCam.Priority = 0; // Disable 3D camera
            Level2D.SetActive(true); // Enable 2D Level
            Level3D.SetActive(false); // Disable 3D Level
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
        InputManager.instance.PlayerActions.Disable();
    }

    void changePerspective(InputAction.CallbackContext context)
    {
        if (GameManager.GameState == 1)
        {// Code to change the perspective of the camera
            is3D = !is3D; // Toggle between 2D and 3D
            Debug.Log("Perspective changed!");

            if (is3D)
            {
                orthoCam.Priority = 0; // Disable 2D camera
                perspectiveCam.Priority = 10; // Enable 3D camera
                Level2D.SetActive(false); // Disable 2D Level
                Level3D.SetActive(true); // Enable 3D Level
            }
            else
            {
                orthoCam.Priority = 10; // Enable 2D camera
                perspectiveCam.Priority = 0; // Disable 3D camera
                Level2D.SetActive(true); // Enable 2D Level
                Level3D.SetActive(false); // Disable 3D Level
            }
        }
    }

    public bool getPerspectiveState()
    { //To get the perspective of the game
        return is3D;
    }


    //Function to store the last position of the player in 3D perspective before switch
    void perspectiveChange(InputAction.CallbackContext context)
    {
        if(GameManager.GameState == 1 && player.transform.parent == null)
        {
            if (!getPerspectiveState())
            {
                currZposition = player.transform.position.z; // Store the current Z position of the player
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, iniZposition);
            }
            else
            {
                RaycastHit hit;
                int flag = 0; // Flag to check if the raycast hit the ground

                if (!Physics.Raycast(player.transform.position, Vector3.down, out _, 4f, (int)groundLayer)) //Checks if player is already grounded
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (flag == 0)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                Vector3 offset = new Vector3(j * .5f, 2f + i * .2f, 0); // Offset to cast multiple rays

                                Ray ray = new Ray(player.transform.position - offset + Vector3.forward * 1f, Vector3.forward);
                                Debug.DrawRay(player.transform.position - offset, Vector3.forward * 50f, Color.red, 50.0f, true);

                                if (Physics.Raycast(ray, out hit, 50f, (int)groundLayer)) //Casts a ray to find the 3D equivalent of the 2D tile
                                {
                                    currZposition = hit.point.z + .45f; // Store the current Z position of the player
                                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, currZposition); // Move the player in 2D perspective
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (flag == 0)
                {
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, iniZposition);
                }
            }
        }
    }
}
