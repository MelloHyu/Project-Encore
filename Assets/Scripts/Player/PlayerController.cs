using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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
    [SerializeField] private float gravityMultiplier = 13f; // Gravity multiplier for falling speed
    [SerializeField] private float lowJumpMultiplier = 2f; // Multiplier for low jump speed

    private Vector3 forceDirection = Vector3.zero;

    // Coyote time variables
    [SerializeField] private float coyoteTime = 0.15f; // Duration of coyote time in seconds
    private float lastGroundedTime;

    // Jump buffer variables
    [SerializeField] private float jumpBufferTime = 0.15f; // Duration to buffer jump input
    private float lastJumpInputTime = -10f, iniZposition = 0f;

    //jump variables
    private bool isJumpHeld = false;

    //camera variables
    [SerializeField] private Camera playerCamera;

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
        InputManager.instance.PlayerActions.Player.Jump.canceled += Jump_canceled;
        moveAction = InputManager.instance.PlayerActions.Player.Move;
    }

   

    // To Enable and Disable the input actions
    private void OnDisable()
    {
        InputManager.instance.PlayerActions.Player.Jump.performed -= Jump_performed;
        InputManager.instance.PlayerActions.Player.Jump.canceled -= Jump_canceled;
    }

    private void FixedUpdate()
    {
        forceDirection += moveAction.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        if(forceDirection != Vector3.zero)
        {
            SoundManager.Instance.PlaySoundWalk();
        }
        else
        {
            SoundManager.Instance.StopSoundWalk(); // Stop the walking sound if not moving
        }
            if (PerspectiveStateManager.instance.getPerspectiveState()) forceDirection += moveAction.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;
        else transform.position = new Vector3(transform.position.x, transform.position.y, iniZposition); // Move the player in 2D perspective

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero; // Reset force direction after applying it

        if (rb.linearVelocity.y < 0f)
        {
            // Apply gravity to the player while falling making it snappier
            rb.linearVelocity -= gravityMultiplier * Physics.gravity.y * Time.fixedDeltaTime * Vector3.down;
        }

        if (rb.linearVelocity.y > 0f && !isJumpHeld)
        {
            // Apply low jump multiplier if the player is not holding the jump button
            rb.linearVelocity -= lowJumpMultiplier * Physics.gravity.y * Time.fixedDeltaTime * Vector3.down;
        }

        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0f; // Ignore vertical component
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            //set the velocity to max speed if exceeding it
            rb.linearVelocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;
        }

        // Track last grounded time for coyote time
        if (isGrounded())
        {
            lastGroundedTime = Time.time;

            // Jump buffering: if jump was pressed recently, perform jump
            if (Time.time - lastJumpInputTime <= jumpBufferTime)
            {
                forceDirection += Vector3.up * jumpForce;
                lastJumpInputTime = -10f; // Reset buffer so jump only happens once
            }
        }

        LookAt(); // Call the LookAt method to rotate the player towards the movement direction
    }

    private void LookAt()
    {
        Vector3 direction = rb.linearVelocity;
        direction.y = 0; // Ignore vertical component

        if (moveAction.ReadValue<Vector2>().x > 0.01f)
        {
            transform.DORotate(new Vector3(0, 0, 0), 0.15f); // Rotate to face right
        }
        else if (moveAction.ReadValue<Vector2>().x < -0.01f)
        {
            transform.DORotate(new Vector3(0, 180, 0), 0.15f); // Rotate to face left
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
        
        isJumpHeld = true;
        lastJumpInputTime = Time.time;

        if (isGrounded() || (Time.time - lastGroundedTime <= coyoteTime))
        {
            forceDirection += Vector3.up * jumpForce;
            SoundManager.Instance.PlaySound(SoundManager.Instance.jumpSound); // Play jump sound
            lastJumpInputTime = -10f; // Reset buffer so jump only happens once
        }
    }

    // Action to perform when the player releases the jump button
    private void Jump_canceled(InputAction.CallbackContext obj)
    {
        isJumpHeld = false;
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
