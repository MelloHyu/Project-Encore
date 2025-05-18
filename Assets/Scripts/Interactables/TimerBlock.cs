using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TimerBlock : MonoBehaviour
{
    [SerializeField] private float fallTimer = 3f; // Time before the block falls
    [SerializeField] private float destroyTimer = 2f; // Time after which the block is destroyed

    private bool isFalling = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallAndDestroy());
        }
    }

    private IEnumerator FallAndDestroy()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallTimer); // Wait for fall timer
        rb.useGravity = true; // Enable gravity to make the block fall
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        Destroy(gameObject, destroyTimer); // Destroy the block after the destroy timer
    }
}

