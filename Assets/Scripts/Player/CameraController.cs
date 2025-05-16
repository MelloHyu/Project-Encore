using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class CameraController : MonoBehaviour
{
    Vector3 pos, offset;
    [SerializeField] Transform player;

    void Awake()
    {
        if(player != null) offset = player.position - transform.position;
    }

    void Update()
    {
        pos = player.position - offset;
        transform.position = Vector3.Lerp(transform.position, pos, 1f);
    }
}
