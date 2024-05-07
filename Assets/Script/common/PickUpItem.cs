using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private readonly float rotateSpeed=400;
    public AudioSource audioSource; 
    protected virtual void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
    } 
}
