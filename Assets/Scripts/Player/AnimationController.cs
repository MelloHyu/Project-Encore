using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator; // Reference to the Animator component

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
    }

    void triggerJumpAnimation(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Jump");
    }
}
