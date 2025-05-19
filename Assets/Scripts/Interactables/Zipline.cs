using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{
    [SerializeField] private Zipline targetZipline;
    [SerializeField] private float zipSpeed = 5f;
    [SerializeField] private float zipScale = .2f; // Scale of the zipline object
    [SerializeField] private float arrivalThreshold = 0.4f; // Minimum distance to trigger the zipline
    [SerializeField] private LineRenderer cable;
    [SerializeField] private float yOffsetCable = 0.5f; // Offset for the cable line renderer

    public Transform zipTransform;

    private bool isZipping = false;
    private GameObject localZip;

    private void Awake()
    {
        cable.SetPosition(0, zipTransform.position);
        cable.SetPosition(1, targetZipline.zipTransform.position);
    }

    private void Update()
    {
        if(GameManager.GameState == 1)
        {
            if (!isZipping || localZip == null) return;
            localZip.GetComponent<Rigidbody>().AddForce((targetZipline.zipTransform.position - zipTransform.position).normalized * zipSpeed * Time.deltaTime, ForceMode.Acceleration);

            if (Vector3.Distance(localZip.transform.position, targetZipline.zipTransform.position) <= arrivalThreshold)
            {
                ResetZipline();
            }
        }
    }

    public void StartZipline(GameObject player)
    {
        if (isZipping) return;

        localZip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        localZip.transform.position = zipTransform.position;
        localZip.transform.localScale = new Vector3(zipScale, zipScale, zipScale);
        localZip.AddComponent<Rigidbody>().useGravity = false;
        localZip.GetComponent<Collider>().isTrigger = true;
        localZip.GetComponent<MeshRenderer>().enabled = false;
        SoundManager.Instance.PlaySoundZip();
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.transform.position = zipTransform.position - new Vector3(0, yOffsetCable, 0);
        player.GetComponent<PlayerController>().enabled = false;

        player.transform.parent = localZip.transform;

        isZipping = true;
    }

    private void ResetZipline()
    {
        if (!isZipping) return;

        GameObject player = localZip.transform.GetChild(0).gameObject;
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player.GetComponent<PlayerController>().enabled = true;
        SoundManager.Instance.StopSoundZip();
        player.transform.parent = null;
        Destroy(localZip);
        localZip = null;
        isZipping = false;
        Debug.Log("Resetting zipline");
    }

    public List<Vector3> getPoints()
    {
        return new List<Vector3> { zipTransform.position, targetZipline.zipTransform.position };
    }
}
