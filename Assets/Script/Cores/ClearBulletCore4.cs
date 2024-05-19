using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBulletCore : PlayerController
{
    public float clearRadius = 4f;
    public ParticleSystem clearParticle;

    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        //�������Χ�ڵ��ӵ�
        Collider[] colliders = Physics.OverlapSphere(transform.position, clearRadius, 1 << 7);
        clearParticle.transform.position = transform.position;
        clearParticle.Play();
        foreach(Collider item in colliders) {
            Destroy(item.gameObject);
        }
    }
}
