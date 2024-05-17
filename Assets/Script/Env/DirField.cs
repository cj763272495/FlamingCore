using System.Collections;
using UnityEngine;

public class DirField:MonoBehaviour {
    private float _largeSpeed = 14f;
    private float _reduceSpeed = 6f;

    private float targetScale = 2f;
    private float originalScale=1;
    public Transform modelTrans;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            StartCoroutine(ScaleObjectCoroutine(modelTrans));
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.transform.position = new Vector3(modelTrans.position.x,playerController.transform.position.y,modelTrans.position.z);
            playerController.SetDir(transform.forward);
            playerController.EnterOverloadMode();
            ParticleMgr.Instance.PlayOverLoadParticle(playerController);
        }
    }

    private IEnumerator ScaleObjectCoroutine(Transform transform) {
        float currentScale = transform.localScale.x;
        float newScale = currentScale + (_largeSpeed * Time.deltaTime);

        while(newScale < targetScale) {
            newScale = Mathf.Min(newScale + (_largeSpeed * Time.deltaTime),targetScale);
            transform.localScale = new Vector3(newScale,transform.localScale.y,newScale);
            yield return null;
        }

        while(newScale > originalScale) {
            newScale = Mathf.Max(newScale - (_reduceSpeed * Time.deltaTime),originalScale);
            transform.localScale = new Vector3(newScale,transform.localScale.y,newScale);
            yield return null;
        }
    }
}
