using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float m_len;
    private Vector3 dir;
    private int maxBounces = 5;
    public LayerMask collisionLayer; // �������߼��Ĳ�
    public GameObject player;
    private int index = 1;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        m_len = Constants.MaxGuideLineLen;
        lineRenderer.positionCount = 2; // ���ö�������Ϊ2
    }

    void Update() {
        lineRenderer.SetPosition(0, player.transform.position); // �������

        Vector3 start = player.transform.position; // �������Ϊ��ǰλ��

        // �ݹ麯�������ڴ����η���
        void BounceRay(Vector3 origin, Vector3 direction, float currentLength, int remainingBounces) {
            if (dir==Vector3.zero) {
                dir = player.GetComponent<PlayerController>().Dir;
            }
            // ���߼��
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, currentLength, collisionLayer)) {
                // �������������ײ�����㷴������
                Vector3 reflectionDir = Vector3.Reflect(direction, hit.normal);
                // �����µ����߳���
                float remainLength = m_len - hit.distance - 0.1f; // ��ȥһ��С��ֵ�Ա�����뵽��ײ���ڲ�

                // ����Ƿ���ʣ��ķ�����������߳���
                if (remainingBounces > 0 && remainLength >0) {
                    // �ݹ����BounceRay���������ߵ���㡢����ͳ���
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(index, hit.point);
                    index++;
                    BounceRay(hit.point, reflectionDir, remainLength, remainingBounces - 1);
                } else {
                    // ���û��ʣ��ķ�����������߳��Ȳ��㣬����LineRenderer�Ķ���λ��
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, 
                        origin + direction * currentLength);
                }
            } else {
                // ���û����ײ������LineRenderer�Ķ���λ�õ���󳤶�
                //Debug.Log(index+ ","+lineRenderer.positionCount+"," + direction.magnitude);
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, origin + direction * currentLength);
                index++;
            }
        }

        // ����㷢������
        BounceRay(start, dir, m_len, maxBounces);
        index = 1;
        lineRenderer.positionCount = 2;
    }


    public void SetEnable(bool b) {
        lineRenderer.enabled = b;
    
    }
    //public void SetPoint(Vector3 start,RaycastHit hits) {
    //    laser.SetPosition(0, start);
    //    laser.SetPosition(1, hits.point);
    //}
    //public void SetPoint(Vector3 start, Vector3 end) {
    //    laser.SetPosition(0, start);
    //    laser.SetPosition(1, end);
    //}
    //public void SetPoint(int  index, Vector3 point) {
    //    laser.SetPosition(index, point);
    //}

    //public void setCount(int count) {
    //    laser.positionCount = count;
    //}
    public void setDir(Vector3 dir) {
        this.dir = dir;
    }
}
