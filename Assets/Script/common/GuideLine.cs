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

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        m_len = Constants.MaxGuideLineLen;
        lineRenderer.positionCount = 2; // ���ö�������Ϊ2
    }

    public void SetLen(float len) {
        m_len = len;
    }

    public void SetDir(Transform trans, Vector3 dir) {//params�����trans�� ����
        //if(!player) {
        //    return;
        //}
        index = 1;
        lineRenderer.positionCount = 2;
        if (gameObject.activeSelf) {
            // �ݹ麯�������ڴ����η���
            void BounceRay(Vector3 origin, Vector3 direction, float currentLength) {
                //if (dir == Vector3.zero) {
                //    dir = player.Dir;
                //} 
                if (Physics.Raycast(origin, direction, out RaycastHit hit, currentLength, collisionLayer)) {
                    if(hit.collider.gameObject.tag=="Boom") {
                        Boom boom = hit.collider.gameObject.GetComponent<Boom>();
                        boom.ShowGudieLine(-hit.normal);
                    }
                    // �������������ײ�����㷴������
                    Vector3 reflectionDir = Vector3.Reflect(direction, hit.normal);
                    // �����µķ��������߳���
                    float remainLength = currentLength - hit.distance /*- 0.01f*/; // ��ȥһ��С��ֵ�Ա�����뵽��ײ���ڲ�

                    // ����Ƿ���ʣ�����߳���
                    if ( remainLength > 0) {
                        // �ݹ����BounceRay���������ߵ���㡢����ͳ���
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(index, hit.point);
                        index++;
                        BounceRay(hit.point, reflectionDir, remainLength);
                    } else {
                        // ���û��ʣ��ķ�����������߳��Ȳ��㣬����LineRenderer�Ķ���λ��
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1,
                            origin + direction * currentLength);
                    }
                } else {
                    lineRenderer.SetPosition(index, origin + direction * currentLength);
                }
            }

            // ����㷢������ 
            lineRenderer.SetPosition(0,trans.position);
            BounceRay(trans.position, dir, m_len);
        }
    }
}
