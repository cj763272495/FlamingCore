using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCityCam : MonoBehaviour
{
    public Transform target; // ��ת��
    public float speed = 2.0f; // ��ת�ٶ�
     
    void Update()
    { 
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
    }
}
