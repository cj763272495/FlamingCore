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
            Vector3 direction = firePoint.forward;
            Vector3 origin = firePoint.position;
            while(true) {
                RaycastHit hit;
                if(Physics.Raycast(origin,direction,out hit,_len)) {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1,hit.point);
                    if(hit.collider.gameObject.tag == "Player") {
                        hit.collider.gameObject.GetComponent<PlayerController>().PlayerDead();
                        break;
                    } else if(hit.collider.gameObject.tag == "Mirror") {
                        // 如果射线碰到了镜子，计算反射方向
                        Vector3 incomingVec = Vector3.Normalize(hit.point - origin);
                        direction = Vector3.Reflect(incomingVec,hit.normal);
                        origin = hit.point;
                    } else if(hit.collider.gameObject.tag == "Wall") {
                        // 如果射线碰到了墙体，停止进一步的射线投射
                        break;
                    }
                } else {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1,origin + direction * _len);
                    break;
                }
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
