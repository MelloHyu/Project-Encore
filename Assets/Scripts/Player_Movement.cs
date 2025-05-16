using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    InputSystem_Actions inputAction;
    CharacterController controller;
    Vector2 moveInput;

    void Awake()
    {
        if (inputAction == null) inputAction = new InputSystem_Actions();
        inputAction.Player.Enable();

        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        inputAction.Enable();
    }

    void OnDisable()
    {
        inputAction.Disable();
    }

    void Update()
    {
        moveInput = inputAction.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * Time.deltaTime * speed);
    }
}
