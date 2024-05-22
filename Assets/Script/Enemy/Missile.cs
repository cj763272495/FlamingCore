using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile:MonoBehaviour {
    public ParticleSystem tailGasParticle;
    public ParticleSystem boomParticle;

    public float MaxSpeed = 12;
    public float curSpeed = 0;
    public float acceleration = 0.5f; // 加速度
    public float avoidDistance = 3f; // 避开墙壁的距离

    public PlayerController player;
    public float rotationSpeed = 100;

    public bool startChase = false;
    public LayerMask bulletLayerMask;

    public void StartChase(PlayerController player) {
        startChase = true;
        this.player = player;
        tailGasParticle.Play();
    }

    void Update() {
        if(startChase && player) {
            Vector3 targetDirection = (player.transform.position - transform.position).normalized;

            if(curSpeed < MaxSpeed) {
                curSpeed += acceleration * Time.deltaTime;
            } else {
                Vector3 currentDirection = transform.forward;
                float angle = Vector3.Angle(currentDirection,targetDirection);
                if(!Physics.Raycast(transform.position,targetDirection,avoidDistance,bulletLayerMask) || angle > 90) {
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,targetDirection,rotationSpeed * Time.deltaTime,0));
                }
            }
        }
        transform.position += transform.forward * curSpeed * Time.deltaTime;
    }



    private void OnCollisionEnter(Collision collision) {
        GameObject go = collision.gameObject;
        if(go == player || go.layer == 10) {
            if(go == player) {
                player.PlayerDead();
            }
            tailGasParticle.Stop();
            boomParticle.Play();
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }
}
