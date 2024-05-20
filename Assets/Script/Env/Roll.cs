using UnityEngine;

public class Roll : MonoBehaviour
{ 
    public Transform rotateAroundTrans;
    public float rotationSpeed = 10.0f; // 绕rotateAroundTrans旋转的速度
    public float selfRotationSpeed = 10.0f; // 自身绕x轴旋转的速度
     
    void Update()
    {
        // 绕rotateAroundTrans旋转
        transform.RotateAround(rotateAroundTrans.position,Vector3.up,rotationSpeed * Time.deltaTime);

        // 自身绕x轴旋转
        transform.Rotate(selfRotationSpeed * Time.deltaTime,0,0);
    }
}
