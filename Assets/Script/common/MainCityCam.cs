using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCityCam : MonoBehaviour
{
    public Transform target; // ��ת��
    public float speed = 2.0f; // ��ת�ٶ�

    // Update is called once per frame
    void Update()
    {
        // Χ��Y����ת
        transform.RotateAround(target.position, Vector3.up, speed*Time.deltaTime);
    }
}
