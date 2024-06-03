using UnityEngine;

public class Trap4 : MonoBehaviour{
    public float rotationSpeedFactor = 10f;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision) {
        Vector3 collisionPoint = collision.contacts[0].point;
        Vector3 pivotPoint = transform.position;
        Vector3 relativePosition = collisionPoint - pivotPoint;
        Vector3 rotationAxis = transform.up;

        //扭矩 = 力 * 力臂
        float torqueMagnitude = collision.gameObject.GetComponent<PlayerController>().Speed * relativePosition.magnitude * rotationSpeedFactor;

        // 碰撞点在支点左边则逆时针旋转，右边则顺时针旋转
        float sign = Vector3.Dot(relativePosition,transform.right) > 0 ? 1f : -1f;
        rb.AddTorque(rotationAxis * sign * torqueMagnitude);
    }


}
