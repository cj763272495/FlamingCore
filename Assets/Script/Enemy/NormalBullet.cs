using System.Drawing;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{ 
    private float bulletSpeed = 10;
    public float remainTime = 3;
    public Vector3 shootDir;
    public Transform owner;
    private float timer = 0;
    private bool startTimer;

    private void OnEnable() {
        timer = 0;
        startTimer = true;
    }
    void Update() {
        transform.Translate(bulletSpeed * Time.deltaTime * shootDir);
        if(startTimer) {
            timer += Time.deltaTime;
        }

        if (timer >= remainTime) {
            ReturnBullet();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.transform != owner) { 
            ParticleMgr.Instance.PlayBulletDestoryParticle(collision.contacts[0].point);
            ReturnBullet();
        }
    }

    public void SetBulletSpeed(float sp) {
        bulletSpeed = sp;
    }
    public void SetBulletShotDir(Vector3 dir) {
        shootDir = dir;
    }

    public void ReturnBullet() {
        PoolManager.Instance.ReturnToPool(this);
        startTimer = false;
        timer = 0;
    }
}
