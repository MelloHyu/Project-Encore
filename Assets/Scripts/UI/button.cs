using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class button : MonoBehaviour
{
    public void OnClick()
    {
        transform.DOPunchScale(new Vector3(.5f, .5f, .5f), 0.25f).SetUpdate(true);
    }
}
