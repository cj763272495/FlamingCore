using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private float rotateSpeed=400;
    public AudioSource audioSource; 
    void Update() {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }


}
