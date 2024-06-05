using UnityEngine; 

public class GuideLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float m_len;

    public LayerMask collisionLayer; // 用于射线检测的层
    public PlayerController player;
    private int index = 1;
    private GameObject curPointBoom;

    void Awake(){
        lineRenderer = GetComponent<LineRenderer>();
        m_len = Constants.MaxGuideLineLen;
        lineRenderer.positionCount = 2; // 设置顶点数量为2
    }

    public void SetLen(float len) {
        m_len = len;
    }

    public void SetDir(Transform trans,Vector3 dir) {
        index = 1;
        lineRenderer.positionCount = 2;
        Vector3 startPos = trans.position;
        if(gameObject.activeSelf) {
            lineRenderer.SetPosition(0,startPos);
            BounceRay(startPos,dir,m_len);
        } else {
            HideBoomGuideLine();
        }
    }

    private void BounceRay(Vector3 origin,Vector3 direction,float remainingLength,bool hitBoom = false) {
        if(Physics.Raycast(origin,direction,out RaycastHit hit,remainingLength,collisionLayer)) {
            Vector3 reflectionDir = Vector3.Reflect(direction,hit.normal);
            float newRemainingLength = remainingLength - hit.distance;
            if(hit.collider.gameObject.tag == "Boom") {
                HandlePointToBoom(hit);
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
                HideBoomGuideLine();
            }
            lineRenderer.SetPosition(index,origin + direction * remainingLength);
        }
    }


    private void HandlePointToBoom(RaycastHit hit) {
        //if(curPointBoom && curPointBoom != hit.collider.gameObject) {
        //    curPointBoom.GetComponent<Boom>().guideLine.gameObject.SetActive(false);
        //}
        curPointBoom = hit.collider.gameObject;
        Boom boom = hit.collider.gameObject.GetComponent<Boom>();
        boom.ShowGudieLine(-hit.normal);
    }

    private void HideBoomGuideLine() {
        if(curPointBoom) {
            curPointBoom.GetComponent<Boom>().guideLine.gameObject.SetActive(false);
            curPointBoom = null;
        }
    }
}
