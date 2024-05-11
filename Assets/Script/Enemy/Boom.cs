using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    //private bool _showGuideLine;
    private bool _isCrashed;
    private Vector3 _dir;

    public GuideLine guideLine;
    public float attackRange=3;
    public GameObject rangeShow;
    public Rigidbody rb;


    public CapsuleCollider exploreTrigger;

    private void Start() { 
        guideLine.gameObject.SetActive(false);
        exploreTrigger.enabled = false;
    } 

    public void ShowGudieLine(Vector3 dir ) {
        guideLine.SetLen(attackRange);
        guideLine.SetDir(transform,dir);
        guideLine.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision) {
        int layer = collision.gameObject.layer; 
        if(layer == 8) {
            _isCrashed = true;
            rangeShow.SetActive(false);
            _dir = new Vector3(-collision.contacts[0].normal.x,transform.position.y,-collision.contacts[0].normal.z);
            StartCoroutine(StartMove());
        }
        if(_isCrashed && (layer==6 || layer==10)) {
            //Boom particle 
            exploreTrigger.enabled = true;
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision) {
        guideLine.gameObject.SetActive(false);
    }

    private IEnumerator StartMove() {
        transform.LookAt(_dir);
        while(_isCrashed) { 
            transform.Translate(_dir * Time.deltaTime * 10);
            //model.transform.Rotate(Vector3.right,Time.deltaTime * 2000);
            //rb.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles + Vector3.right * Time.deltaTime * 2000));
            transform.Rotate(Vector3.right * Time.deltaTime * 1800);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 6) {
            //销毁碰撞的物体
            Destroy(other.gameObject);
        }
    }

}
