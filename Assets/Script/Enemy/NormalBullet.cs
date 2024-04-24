using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{

    private readonly float BulletSpeed = 15;
    private readonly float remainTime = 3;
    private float timer = 0;

    void Update() {
        transform.Translate(BulletSpeed * Time.deltaTime * transform.right);
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
