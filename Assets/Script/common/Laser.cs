using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float m_len;

    public LayerMask collisionLayer; // �������߼��Ĳ�
    public GameObject player;
    private int index = 1;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        m_len = Constants.MaxGuideLineLen;
        lineRenderer.positionCount = 2; // ���ö�������Ϊ2
    }

    public void setDir(Vector3 dir) {
        index = 1;
        lineRenderer.positionCount = 2;
        if (gameObject.activeSelf) {
            lineRenderer.SetPosition(0, player.transform.position); // �������

            Vector3 start = player.transform.position; // �������Ϊ��ǰλ��

            // �ݹ麯�������ڴ����η���
            void BounceRay(Vector3 origin, Vector3 direction, float currentLength) {
                if (dir == Vector3.zero) {
                    dir = player.GetComponent<PlayerController>().Dir;
                }
                RaycastHit hit;
                if (Physics.Raycast(origin, direction, out hit, currentLength, collisionLayer)) {
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
                    // ���û����ײ������LineRenderer�Ķ���λ�õ���󳤶�
                    //Debug.Log(index+ ","+lineRenderer.positionCount+"," + direction.magnitude);
                    //lineRenderer.positionCount++;
                    lineRenderer.SetPosition(index, origin + direction * currentLength);
                }
            }

            // ����㷢������
            BounceRay(start, dir, m_len);
        }
    }
}
