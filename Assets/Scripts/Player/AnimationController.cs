using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator; // Reference to the Animator component
    [SerializeField] private float jumpAnimDuration = 0.5f, minDist = 0.4f; // Duration of the jump animation
    Vector3 direction, end;
    bool isZipping = false;

    void OnEnable()
    {
        InputManager.instance.PlayerActions.Player.Jump.performed += triggerJumpAnimation; // Subscribe to the jump event
        InputManager.instance.PlayerActions.Enable(); // Enable input actions
    }

    void OnDisable()
    {
        InputManager.instance.PlayerActions.Player.Jump.performed -= triggerJumpAnimation; // Subscribe to the jump event
    }

    private void Update()
    {
        if (InputManager.instance.PlayerActions.Player.Move.ReadValue<Vector2>().magnitude > 0.01f) animator.SetBool("Moving", true);
        else animator.SetBool("Moving", false);

        if(isZipping)
        {
            if (Vector3.Distance(transform.parent.position, end) <= minDist)
            {
                ziplineAnimationDone(); // Call the zipline animation done function
            }
        }
    }

    void triggerJumpAnimation(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Jump");
    }

    public void ziplineAnimation(Vector3 start, Vector3 end)
    {
        animator.SetTrigger("Zipline_Start"); // Trigger the zipline start animation
        this.end = end;
        direction = end - start;
        isZipping = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Debug.Log(angle);
        Debug.Log(direction);

        //Rotate the player to face the direction of movement on zipline
        if (angle > 0) transform.DORotate(new Vector3(0, 180, angle), .2f);
        else transform.DORotate(new Vector3(180, 180, -90 + angle), .2f);
    }

    void ziplineAnimationDone()
    {
        isZipping = false;
        animator.SetTrigger("Zipline_Done"); // Trigger the zipline done animation
        transform.DORotate(new Vector3(0, 0, 0), 1f); // Rotate the player back to the original position
    }
}
