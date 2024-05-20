using UnityEngine;

public class Roll : MonoBehaviour
{ 
    public Transform rotateAroundTrans;
    public float rotationSpeed = 10.0f; // ��rotateAroundTrans��ת���ٶ�
    public float selfRotationSpeed = 10.0f; // ������x����ת���ٶ�
     
    void Update()
    {
        // ��rotateAroundTrans��ת
        transform.RotateAround(rotateAroundTrans.position,Vector3.up,rotationSpeed * Time.deltaTime);

        // ������x����ת
        transform.Rotate(selfRotationSpeed * Time.deltaTime,0,0);
    }
}
