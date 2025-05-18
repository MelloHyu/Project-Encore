using System.Collections.Generic;
using UnityEngine;

public class PlayerZiplineController : MonoBehaviour
{
    [SerializeField] private float checkOffset = 1f; // Offset for the raycast check
    [SerializeField] private float checkRadius = 2f;
    AnimationController animationController;

    void Awake()
    {
        animationController = GetComponent<AnimationController>();
    }

    private void OnEnable()
    {
        InputManager.instance.PlayerActions.Player.Zipline.performed += Zipline_performed; ;
    }

    private void OnDisable()
    {
        InputManager.instance.PlayerActions.Player.Zipline.performed -= Zipline_performed; ;
    }

    private void Zipline_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (PerspectiveStateManager.instance.getPerspectiveState()) //Only allow player to use the zipline in 3D
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, checkOffset, 0), checkRadius, Vector3.up);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == "Zipline")
                {
                    hit.collider.GetComponent<Zipline>().StartZipline(gameObject);
                    List<Vector3> extremePoints = hit.collider.GetComponent<Zipline>().getPoints();
                    animationController.ziplineAnimation(extremePoints[0], extremePoints[1]);
                }
            }
        }
    }
}
