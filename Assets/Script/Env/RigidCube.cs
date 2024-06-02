using UnityEngine;

public class RigidCube : MonoBehaviour
{
    public float forceMultiplier = 1.0f;
    public float frictionCoefficient = 0.6f;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if(rb.velocity.magnitude > 0.2f) { 
            Vector3 velocity = rb.velocity;
            Vector3 frictionForce = -velocity.normalized * frictionCoefficient;
            rb.AddForce(frictionForce,ForceMode.Force);
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag=="Player") {
            Vector3 dir = collision.contacts[0].normal;
            rb.AddForce(dir * forceMultiplier,ForceMode.Impulse);
        }
    }
}
