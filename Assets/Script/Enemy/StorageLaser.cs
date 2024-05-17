using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageLaser : Laser
{
    public float chargeTime = 3f;
    public readonly float minWidth=0.01f;
    public readonly float maxWidth=0.15f;

    public float effectiveWidth = 0.1f;
    public float effectTime = 0.5f;

    bool isCharging = true;

    protected override void Start() {
        base.Start(); 
        StartCoroutine(ChargeAndShoot());
    }

    protected override void ShootLaser() {
        RaycastHit hit;
        if(Physics.Raycast(firePoint.transform.position,firePoint.transform.forward,out hit,len)) {
            _lineRenderer.SetPosition(1,new Vector3(0,0,hit.distance));
            if(hit.collider.gameObject.tag == "Player" && _lineRenderer.startWidth >= effectiveWidth) {
                // 激光射中了玩家
                Debug.Log("Hit the player!");
            }
        } else {
            _lineRenderer.SetPosition(1,new Vector3(0,0,len));
        }
    }

    IEnumerator ChargeAndShoot() {
        while(true) {
            if(isCharging) {
                // 激光正在充电，宽度保持在 minWidth
                _lineRenderer.startWidth = minWidth;
                _lineRenderer.endWidth = minWidth;
                yield return new WaitForSeconds(chargeTime);
                isCharging = false;
            } else {
                // 激光开始生效，宽度从 minWidth 线性插值到 maxWidth
                for(float t = 0; t < effectTime; t += Time.deltaTime) {
                    float chargeAmount = t / effectTime;
                    float width = Mathf.Lerp(0,maxWidth,chargeAmount);
                    _lineRenderer.startWidth = width;
                    _lineRenderer.endWidth = width;
                    yield return null;
                }
                for(float t = 0; t < effectTime*2; t += Time.deltaTime) {
                    float chargeAmount = t / (effectTime*2);
                    float width = Mathf.Lerp(maxWidth,0,chargeAmount*2);
                    _lineRenderer.startWidth = width;
                    _lineRenderer.endWidth = width;
                    yield return null;
                }
                isCharging = true;
            }
        }
    }

}
