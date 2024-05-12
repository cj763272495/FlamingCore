using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class GuideLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float m_len;

    public LayerMask collisionLayer; // �������߼��Ĳ�
    public PlayerController player;
    private int index = 1;
    private GameObject curPointObj;

    void Awake(){
        lineRenderer = GetComponent<LineRenderer>();
        m_len = Constants.MaxGuideLineLen;
        lineRenderer.positionCount = 2; // ���ö�������Ϊ2
    }

    public void SetLen(float len) {
        m_len = len;
    }

    public void SetDir(Transform trans,Vector3 dir) {
        index = 1;
        lineRenderer.positionCount = 2;
        if(gameObject.activeSelf) {
            lineRenderer.SetPosition(0,trans.position);
            BounceRay(trans.position,dir,m_len);
        } else {
            HideBoom();
        }
    }

    private void BounceRay(Vector3 origin,Vector3 direction,float remainingLength,bool hitBoom = false) {
        if(Physics.Raycast(origin,direction,out RaycastHit hit,remainingLength,collisionLayer)) {
            Vector3 reflectionDir = Vector3.Reflect(direction,hit.normal);
            float newRemainingLength = remainingLength - hit.distance;
            if(hit.collider.gameObject.tag == "Boom") {
                HandleBoom(hit);
                hitBoom = true;
            }
            if(newRemainingLength > 0) {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(index, hit.point);
                index++;
                BounceRay(hit.point,reflectionDir,newRemainingLength,hitBoom);
            } else {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1,hit.point);
            }
        } else {
            if(!hitBoom) {
                HideBoom();
            }
            lineRenderer.SetPosition(index,origin + direction * remainingLength);
        }
    }


    private void HandleBoom(RaycastHit hit) {
        if(curPointObj && curPointObj != hit.collider.gameObject) {
            curPointObj.GetComponent<Boom>().guideLine.gameObject.SetActive(false);
        }
        curPointObj = hit.collider.gameObject;
        Boom boom = hit.collider.gameObject.GetComponent<Boom>();
        boom.ShowGudieLine(-hit.normal);
    }
    private void HideBoom() {
        if(curPointObj) {
            curPointObj.GetComponent<Boom>().guideLine.gameObject.SetActive(false);
            curPointObj = null;
        }
    }
}
