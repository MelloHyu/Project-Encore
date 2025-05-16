using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //Input fields
    private InputAction moveAction;

    //movement variables
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float playerheight = 1.6f;
    private Vector3 forceDirection = Vector3.zero;

    //camera variables
    [SerializeField] private Camera playerCamera;

    float iniZposition, currZposition; //To store the initial Z position of the player

    //inilialize the fields
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        iniZposition = transform.position.z; // Store the initial Z position of the player
    }

    // To Enable and Disable the input actions
    private void OnEnable()
    {
        InputManager.instance.PlayerActions.Player.Jump.performed += Jump_performed;
        moveAction = InputManager.instance.PlayerActions.Player.Move;

        Debug.Log(InputManager.instance.PlayerActions);
    }

    // To Enable and Disable the input actions
    private void OnDisable()
    {
        InputManager.instance.PlayerActions.Player.Jump.performed -= Jump_performed;
    }

    private void FixedUpdate()
    {
        forceDirection += moveAction.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        if (PerspectiveStateManager.instance.getPerspectiveState()) forceDirection += moveAction.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;
        else transform.position = new Vector3(transform.position.x, transform.position.y, iniZposition); // Move the player in 2D perspective

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero; // Reset force direction after applying it

        if (rb.linearVelocity.y < 0f)
        {
            // Apply gravity to the player while falling making it snappier
            rb.linearVelocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime * 2f;
        }

        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0f; // Ignore vertical component
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            //set the velocity to max speed if exceeding it
            rb.linearVelocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;
        }

        LookAt(); // Call the LookAt method to rotate the player towards the movement direction
    }

    private void LookAt()
    {
        Vector3 direction = rb.linearVelocity;
        direction.y = 0; // Ignore vertical component

        if (moveAction.ReadValue<Vector2>().sqrMagnitude > 0.01f && direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            rb.angularVelocity = Vector3.zero; // Reset angular velocity if not moving
        }
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        // Get the right direction of the camera
        Vector3 right = playerCamera.transform.right;
        right.y = 0; // Ignore vertical component
        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        // Get the forward direction of the camera
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0; // Ignore vertical component
        return forward.normalized;
    }

    // Action to perform when the player presses the jump button
    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if (isGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool isGrounded()
    {
        // Check if the player is grounded using a raycast
        Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);

        if (Physics.Raycast(ray, out _, playerheight))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
