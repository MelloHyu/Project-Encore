using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public GameObject[] objects;
    public float openTime;
    public float waitTime;

    WaitForSecondsRealtime wait;

    private void Start() {
        wait = new WaitForSecondsRealtime(waitTime);

        StartCoroutine(opened());
    }

    IEnumerator opened(){
        foreach(GameObject obj in objects){
            obj.transform.localScale = Vector3.zero;
        }

        foreach(GameObject obj in objects){
            obj.transform.DOScale(Vector3.one, openTime).SetEase(Ease.OutBounce).SetUpdate(true);
            yield return wait;
        }
    }
}
