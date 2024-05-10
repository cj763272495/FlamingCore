using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirField:MonoBehaviour {
    private float _largeSpeed = 14f;
    private float _reduceSpeed = 6f;

    private float targetScale = 3f;
    private float originalScale=2;
    public Transform modelTrans;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            StartCoroutine(ScaleObjectCoroutine(modelTrans));
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.transform.position = new Vector3(modelTrans.position.x,playerController.transform.position.y,modelTrans.position.z);
            playerController.Dir = transform.forward;
            playerController.EnterOverloadMode();
        }
    }

    private IEnumerator ScaleObjectCoroutine(Transform transform) {
        float currentScale = transform.localScale.x;
        float newScale = currentScale + (_largeSpeed * Time.deltaTime);

        while(newScale < targetScale) {
            newScale = Mathf.Min(newScale + (_largeSpeed * Time.deltaTime),targetScale);
            transform.localScale = new Vector3(newScale,newScale,transform.localScale.z);
            yield return null;
        }

        while(newScale > originalScale) {
            newScale = Mathf.Max(newScale - (_reduceSpeed * Time.deltaTime),originalScale);
            transform.localScale = new Vector3(newScale,newScale,transform.localScale.z);
            yield return null;
        }
    }
}
