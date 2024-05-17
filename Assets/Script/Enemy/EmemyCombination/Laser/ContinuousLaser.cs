using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousLaser : IFireMode {

    protected float _len = 20;
    protected GameObject _firePoint;
    protected GameObject _spawnedLaser;
    protected LineRenderer _lineRenderer;

    public ContinuousLaser(GameObject spawnedLaser,LineRenderer lineRenderer, 
        GameObject firePoint, float len=20) {
        _len = len;
        _firePoint = firePoint;
        _spawnedLaser = spawnedLaser;
        _lineRenderer = lineRenderer;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0,Vector3.zero);
        EnableLaser();
    }

    public virtual void Fire() {
        RaycastHit hit;
        if(Physics.Raycast(_firePoint.transform.position,_firePoint.transform.forward,out hit,_len)) {
            _lineRenderer.SetPosition(1,new Vector3(0,0,hit.distance));
            if(hit.collider.gameObject.tag == "Player") {
                hit.collider.gameObject.GetComponent<PlayerController>().PlayerDead();
            }
        } else {
            _lineRenderer.SetPosition(1,new Vector3(0,0,_len));
        }
    }

    public void EnableLaser() {
        _spawnedLaser.SetActive(true);
    }

    public void DisableLaser() {
        _spawnedLaser.SetActive(false);
    }
}
