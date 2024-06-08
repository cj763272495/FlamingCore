using UnityEngine;

public class NormalBullet : MonoBehaviour
{ 
    public float bulletSpeed = 10;
    public float remainTime = 3;
    public Vector3 shootDir;
    public Transform owner;
    private float timer = 0;
    private bool startTimer; 
    
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
            gameObject.SetActive(false);
            ReturnBullet();
        }
    }

    public void SetBulletSpeed(float sp) {
        bulletSpeed = sp;
    }
    public void SetBulletShot(Vector3 dir) {
        timer = 0;
        startTimer = true;
        shootDir = dir;
    }

    public void ReturnBullet() {
        PoolManager.Instance.ReturnToPool(this);
        startTimer = false;
        timer = 0;
    }
}
