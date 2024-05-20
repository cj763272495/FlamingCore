using UnityEngine;

public class BreakedWall : MonoBehaviour //ø…∆∆ÀÈ«Ω
{
    public ParticleSystem _particle;

    private void Start() {
        ParticleMgr.Instance.AddCustomParticle(_particle.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            //≤•∑≈∆∆ÀÈ∂Øª≠
            ParticleMgr.Instance.PlayCustomParticle(_particle.gameObject, transform.position); 
            Destroy(gameObject);
        }
    }
}
