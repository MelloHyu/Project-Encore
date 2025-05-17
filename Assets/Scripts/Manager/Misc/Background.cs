using UnityEngine;

[ExecuteInEditMode]
public class Background : MonoBehaviour
{
    Camera cam;
    [SerializeField] float distance = 10f;

    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Camera cam = Camera.main;

        float pos = cam.nearClipPlane + distance;

        transform.position = cam.transform.position + cam.transform.forward * pos;
        transform.LookAt(cam.transform);
        float h = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f * cam.aspect / 10.0f;
        float w = h * Screen.height / Screen.width;
        transform.localScale = new Vector3(h, w, 1);
    }
}
