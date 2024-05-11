using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class GuideLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float m_len;

    public LayerMask collisionLayer; // 用于射线检测的层
    public PlayerController player;
    private int index = 1;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        m_len = Constants.MaxGuideLineLen;
        lineRenderer.positionCount = 2; // 设置顶点数量为2
    }

    public void SetLen(float len) {
        m_len = len;
    }

    public void SetDir(Transform trans, Vector3 dir) {//params：起点trans， 方向
        //if(!player) {
        //    return;
        //}
        index = 1;
        lineRenderer.positionCount = 2;
        if (gameObject.activeSelf) {
            // 递归函数，用于处理多次反射
            void BounceRay(Vector3 origin, Vector3 direction, float currentLength) {
                //if (dir == Vector3.zero) {
                //    dir = player.Dir;
                //} 
                if (Physics.Raycast(origin, direction, out RaycastHit hit, currentLength, collisionLayer)) {
                    if(hit.collider.gameObject.tag=="Boom") {
                        Boom boom = hit.collider.gameObject.GetComponent<Boom>();
                        boom.ShowGudieLine(-hit.normal);
                    }
                    // 如果射线遇到碰撞，计算反射向量
                    Vector3 reflectionDir = Vector3.Reflect(direction, hit.normal);
                    // 计算新的反射后的射线长度
                    float remainLength = currentLength - hit.distance /*- 0.01f*/; // 减去一个小的值以避免插入到碰撞体内部

                    // 检查是否还有剩余射线长度
                    if ( remainLength > 0) {
                        // 递归调用BounceRay，更新射线的起点、方向和长度
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(index, hit.point);
                        index++;
                        BounceRay(hit.point, reflectionDir, remainLength);
                    } else {
                        // 如果没有剩余的反射次数或射线长度不足，更新LineRenderer的顶点位置
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1,
                            origin + direction * currentLength);
                    }
                } else {
                    lineRenderer.SetPosition(index, origin + direction * currentLength);
                }
            }

            // 从起点发射射线 
            lineRenderer.SetPosition(0,trans.position);
            BounceRay(trans.position, dir, m_len);
        }
    }
}
