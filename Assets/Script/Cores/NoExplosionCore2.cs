using UnityEngine;

public class NoExplosionCore : PlayerController
{
    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
    }
}
