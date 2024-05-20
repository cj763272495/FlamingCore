using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public ParticleSystem tailGasParticle;
    public ParticleSystem boomParticle;

    public float MaxSpeed = 10;
    public float curSpeed = 0;
    public float acceleration = 0.5f; // 加速度
    public float avoidDistance = 1f; // 避开墙壁的距离

    public PlayerController player;

    public bool startChase = false;

    public void StartChase(PlayerController player) {
        startChase = true;
        this.player = player;
    }

    void Update() {
        if(startChase && player) {
            curSpeed = Mathf.Min(curSpeed + acceleration * Time.deltaTime,MaxSpeed);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            if(Physics.Raycast(transform.position,direction,avoidDistance)) {
                direction = Quaternion.Euler(0,90,0) * direction;
            }
            transform.position += direction * curSpeed * Time.deltaTime;
            tailGasParticle.Play();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            player = collision.gameObject.GetComponent<PlayerController>();
            player.PlayerDead();
            tailGasParticle.Stop();
            boomParticle.Play();
        }
    }
}
