using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool startRotate = false;
    public ParticleSystem ps;

    private void Start() {
        gameObject.SetActive(false);
    }

    private void Update() {
        if(startRotate) {
            transform.Rotate(150 * Time.deltaTime * Vector3.up);
        }
    }
    public void OpenChest() {
        gameObject.SetActive(true);
        animator.SetBool("Open", true);
        //ps.Play();
    }

    public void PlayParticle() {
        startRotate = true;
        ps.Play(); 
    }

    public void Exit() {
        gameObject.SetActive(false);
        startRotate = false;
        ps.Stop();
    }
}
