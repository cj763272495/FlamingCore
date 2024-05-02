using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float rotateSpeed = 100;
    public GameObject player;
    public int destoryCoinValue = 5;

    protected virtual void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            ParticleMgr.Instance.PlayEnemyDeadParticle(collision.contacts[0], collision.transform);
            AudioManager.Instance.PlaySound(ResSvc.Instance.LoadAudio(Constants.HitEnenmyClip));
            Destroy(gameObject);
        }
    }

}
