using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{

    private float BulletSpeed = 15;
    private float remainTime = 3;
    private float timer = 0;

    void Update() {
        transform.Translate(transform.right * BulletSpeed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= remainTime) {
            Destroy(gameObject);
            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
