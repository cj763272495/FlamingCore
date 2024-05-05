using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float rotateSpeed = 100;
    public GameObject player;
    protected bool canDestroy = true;
    public int destoryCoinValue = 5;

    protected virtual void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player") && canDestroy) {
            ParticleMgr.Instance.PlayEnemyDeadParticle(collision.contacts[0], collision.transform);
            AudioManager.Instance.PlaySound(ResSvc.Instance.LoadAudio(Constants.HitEnenmyClip));

            StartCoroutine(DestroyAfterSeconds(1));
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DestroyAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
