using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ContinuousLaser : IFireMode {

    protected float _len = 20;
    protected List<Transform> _firePoints;
    protected GameObject _spawnedLaser;
    protected List<LineRenderer> _lineRenderers;
    protected LayerMask _layer;

    public ContinuousLaser(GameObject spawnedLaser,List<LineRenderer> lineRenderers,
        List<Transform> firePoints,LayerMask layer, float len=20) {
        _len = len;
        _firePoints = firePoints;
        _spawnedLaser = spawnedLaser;
        _lineRenderers = lineRenderers;
        _layer = layer;
        EnableLaser();
    }

    public virtual void Fire() {
        for(int i = 0; i < _firePoints.Count; i++) {
            Vector3 direction = _firePoints[i].forward;
            Vector3 origin = _firePoints[i].position;
            _lineRenderers[i].positionCount = 2;
            _lineRenderers[i].SetPosition(0,origin);
                RaycastHit hit;
                if(Physics.Raycast(origin, direction, out hit, _len, _layer)) {
                    _lineRenderers[i].SetPosition(1, hit.point); 
                    GameObject hitGo = hit.collider.gameObject;
                    if(hitGo.tag == "Player") {
                        hitGo.GetComponent<PlayerController>().PlayerDead();
                    } else if(hitGo.tag == "DestoryableEnemy") {
                        hitGo.gameObject.SetActive(false);
                        //ParticleMgr.Instance.PlayEnemyDeadParticle(hit.point);
                    }
                    break;
                } else {
                    _lineRenderers[i].SetPosition(1, origin + direction * _len);
                    break;
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
