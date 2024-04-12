using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCityCam : MonoBehaviour
{
    public Transform target; // 旋转点
    public float speed = 2.0f; // 旋转速度

    // Update is called once per frame
    void Update()
    {
        // 围绕Y轴旋转
        transform.RotateAround(target.position, Vector3.up, speed*Time.deltaTime);
    }
}
