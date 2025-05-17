using UnityEngine;

public class PlayerZiplineController : MonoBehaviour
{
    [SerializeField] private float checkOffset = 1f; // Offset for the raycast check
    [SerializeField] private float checkRadius = 2f;

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
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0,checkOffset,0), checkRadius, Vector3.up);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Zipline")
            {
                hit.collider.GetComponent<Zipline>().StartZipline(gameObject);
            }
        }
    }
}
