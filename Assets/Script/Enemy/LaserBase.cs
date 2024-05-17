using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject spawnedlaser;
    public float len = 20;
    public LineRenderer _lineRenderer;

    protected virtual void Start() {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0,Vector3.zero);
        EnableLaser();
    }

    void Update() {
        ShootLaser();
    }
    protected virtual void ShootLaser() {
        RaycastHit hit;
        if(Physics.Raycast(firePoint.transform.position,firePoint.transform.forward,out hit,len)) {
            _lineRenderer.SetPosition(1,new Vector3(0,0,hit.distance));
            if(hit.collider.gameObject.tag == "Player") {
                hit.collider.gameObject.GetComponent<PlayerController>().PlayerDead();
                //Debug.Log("Hit the player!");
            }
        } else {
            _lineRenderer.SetPosition(1,new Vector3(0,0,len));
        }
    }

    void EnableLaser() {
        spawnedlaser.SetActive(true);
    }

    void DisableLaser() {
        spawnedlaser.SetActive(false);
    }
}
