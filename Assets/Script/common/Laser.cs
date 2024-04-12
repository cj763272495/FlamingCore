using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float m_len;
    private Vector3 dir;
    private int maxBounces = 5;
    public LayerMask collisionLayer; // 用于射线检测的层
    public GameObject player;
    private int index = 1;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        m_len = Constants.MaxGuideLineLen;
        lineRenderer.positionCount = 2; // 设置顶点数量为2
    }

    void Update() {
        lineRenderer.SetPosition(0, player.transform.position); // 设置起点

        Vector3 start = player.transform.position; // 射线起点为当前位置

        // 递归函数，用于处理多次反射
        void BounceRay(Vector3 origin, Vector3 direction, float currentLength, int remainingBounces) {
            if (dir==Vector3.zero) {
                dir = player.GetComponent<PlayerController>().Dir;
            }
            // 射线检测
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, currentLength, collisionLayer)) {
                // 如果射线遇到碰撞，计算反射向量
                Vector3 reflectionDir = Vector3.Reflect(direction, hit.normal);
                // 计算新的射线长度
                float remainLength = m_len - hit.distance - 0.1f; // 减去一个小的值以避免插入到碰撞体内部

                // 检查是否还有剩余的反射次数和射线长度
                if (remainingBounces > 0 && remainLength >0) {
                    // 递归调用BounceRay，更新射线的起点、方向和长度
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(index, hit.point);
                    index++;
                    BounceRay(hit.point, reflectionDir, remainLength, remainingBounces - 1);
                } else {
                    // 如果没有剩余的反射次数或射线长度不足，更新LineRenderer的顶点位置
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, 
                        origin + direction * currentLength);
                }
            } else {
                // 如果没有碰撞，更新LineRenderer的顶点位置到最大长度
                //Debug.Log(index+ ","+lineRenderer.positionCount+"," + direction.magnitude);
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, origin + direction * currentLength);
                index++;
            }
        }

        // 从起点发射射线
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
