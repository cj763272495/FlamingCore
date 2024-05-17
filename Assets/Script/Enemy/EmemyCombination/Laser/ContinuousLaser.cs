using System.Collections.Generic;
using UnityEngine;

public class ContinuousLaser : IFireMode {

    protected float _len = 20;
    protected List<Transform> _firePoints;
    protected GameObject _spawnedLaser;
    protected LineRenderer _lineRenderer;

    public ContinuousLaser(GameObject spawnedLaser,LineRenderer lineRenderer, 
        List<Transform> firePoints, float len=20) {
        _len = len;
        _firePoints = firePoints;
        _spawnedLaser = spawnedLaser;
        _lineRenderer = lineRenderer;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0,Vector3.zero);
        EnableLaser();
    }

    public virtual void Fire() {
        foreach(Transform firePoint in _firePoints) {
            RaycastHit hit;
            if(Physics.Raycast(firePoint.position,firePoint.forward,out hit,_len)) {
                _lineRenderer.SetPosition(1,new Vector3(0,0,hit.distance));
                if(hit.collider.gameObject.tag == "Player") {
                    hit.collider.gameObject.GetComponent<PlayerController>().PlayerDead();
                }
            } else {
                _lineRenderer.SetPosition(1,new Vector3(0,0,_len));
            }
        } 
    }

    public void EnableLaser() {
        _spawnedLaser.SetActive(true);
    }

    public void DisableLaser() {
        _spawnedLaser.SetActive(false);
    }
}
