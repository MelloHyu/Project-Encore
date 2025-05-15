using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickableItem : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 100f * Time.deltaTime, 0, Space.Self);
    }
}
