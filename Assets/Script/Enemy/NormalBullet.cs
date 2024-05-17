using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{

    private readonly float BulletSpeed = 10;
    private readonly float remainTime = 3;
    public Vector3 shootDir;
    public Transform owner;
    private float timer = 0;

    void Update() {
        transform.Translate(BulletSpeed * Time.deltaTime * shootDir);
        timer += Time.deltaTime;
        if (timer >= remainTime) {
            gameObject.SetActive(false);
            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.transform != owner) {
            ParticleMgr.Instance.PlayBulletDestoryParticle(collision.contacts[0]);
            gameObject.SetActive(false);
        }
    }
}
