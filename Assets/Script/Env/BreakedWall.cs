using UnityEngine;

public class BreakedWall : MonoBehaviour //������ǽ
{
    public ParticleSystem _particle;

    private void Start() {
        ParticleMgr.Instance.AddCustomParticle(_particle.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            //�������鶯��
            ParticleMgr.Instance.PlayCustomParticle(_particle.gameObject, transform.position); 
            Destroy(gameObject);
        }
    }
}
