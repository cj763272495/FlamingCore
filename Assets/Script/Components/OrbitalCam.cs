using UnityEngine;

public class OrbitalCam:MonoBehaviour {
    public Transform target;
    public float speed = 2.0f;
    public float maxAngle = 30.0f;

    private float totalRotation = 0.0f;
    private float initialRotation;

    void Start() {
        initialRotation = transform.eulerAngles.y;

    }
    void Update() {
        float rotation = speed * Time.deltaTime;
        totalRotation += rotation;
        float clampedRotation = Mathf.PingPong(totalRotation,maxAngle); 

        // ���㱾֡ʵ����ת�ĽǶ�
        float actualRotation = clampedRotation - (transform.eulerAngles.y-initialRotation)-maxAngle/2;
        transform.RotateAround(target.position, Vector3.up,actualRotation);
    }
}