using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedLaser :  ContinuousLaser, IFireMode {
    public float chargeTime = 3f;
    public readonly float minWidth = 0.01f;
    public readonly float maxWidth = 0.15f;

    public float effectiveWidth = 0.1f;
    public float effectTime = 0.5f;

    bool isCharging = true;

    public ChargedLaser(GameObject spawnedLaser,List<LineRenderer> lineRenderers,
        List<Transform> firePoint, MonoBehaviour monoBehaviour, LayerMask layer, float len = 20) :
            base(spawnedLaser,lineRenderers,firePoint, layer, len) {
        _len = len;
        _firePoints = firePoint;
        _spawnedLaser = spawnedLaser;
        _lineRenderers = lineRenderers;
        _layer = layer;

        EnableLaser();
        monoBehaviour.StartCoroutine(ChargeAndShoot());
    }

    public override void Fire() {
        foreach(Transform firePoint in _firePoints) {
            RaycastHit hit;
            if(Physics.Raycast(firePoint.transform.position,firePoint.transform.forward,out hit,_len,_layer)) {
                _lineRenderers[0].SetPosition(1,new Vector3(0,0,hit.distance));
                if(hit.collider.gameObject.tag == "Player" && _lineRenderers[0].startWidth >= effectiveWidth) {
                    hit.collider.gameObject.GetComponent<PlayerController>().PlayerDead();
                }
            } else {
                _lineRenderers[0].SetPosition(1,new Vector3(0,0,_len));
            }
        } 
    }

    IEnumerator ChargeAndShoot() {
        while(true) {
            if(isCharging) {
                // 激光正在充电，宽度保持在 minWidth
                _lineRenderers[0].startWidth = minWidth;
                _lineRenderers[0].endWidth = minWidth;
                yield return new WaitForSeconds(chargeTime);
                isCharging = false;
            } else {
                // 激光开始生效，宽度从 minWidth 线性插值到 maxWidth
                for(float t = 0; t < effectTime; t += Time.deltaTime) {
                    float chargeAmount = t / effectTime;
                    float width = Mathf.Lerp(0,maxWidth,chargeAmount);
                    _lineRenderers[0].startWidth = width;
                    _lineRenderers[0].endWidth = width;
                    yield return null;
                }
                for(float t = 0; t < effectTime * 2; t += Time.deltaTime) {
                    float chargeAmount = t / (effectTime * 2);
                    float width = Mathf.Lerp(maxWidth,0,chargeAmount * 2);
                    _lineRenderers[0].startWidth = width;
                    _lineRenderers[0].endWidth = width;
                    yield return null;
                }
                isCharging = true;
            }
        }
    }
}
