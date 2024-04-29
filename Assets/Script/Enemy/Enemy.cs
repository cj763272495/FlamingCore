using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float rotateSpeed = 100;
    public GameObject player;
    public Rigidbody rb;

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            //soundPlayer = collision.gameObject.GetComponent<SoundPlayer>();
            //soundPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.HitEnenmyClip);
            //soundPlayer.PlaySound();
            ParticleMgr.Instance.PlayEnemyDeadParticle(collision.contacts[0].point, collision.transform);
            Destroy(gameObject);
        }
    }

}
