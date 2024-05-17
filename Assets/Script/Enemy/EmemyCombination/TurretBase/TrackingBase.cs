using UnityEngine;

public class TrackingBase:TurretBase {
    public float moveSpeed = 0.5f;

    public float _trackingDis;
    public float _attackDis;

    public TrackingBase(float trackingDis=10, float attackDis=5) {
        _trackingDis = trackingDis;
        _attackDis = attackDis;
    }

    public override void Move(EnemyEntity enemy,Transform targetTrans) {
        if(InAtkRange(enemy,targetTrans)) {
            Vector3 direction = targetTrans.position - enemy.transform.position;
            direction.Normalize();
            Vector3 newPosition = enemy.transform.position + direction * moveSpeed * Time.deltaTime;
            // 检查新的位置和目标位置的距离
            if(Vector3.Distance(newPosition,targetTrans.position) >= _attackDis) {
                enemy.transform.position = newPosition;
                enemy.Take();
            } else {
                enemy.Idle();
            }
        } else {
            enemy.Idle();
        }
    }

    private bool InAtkRange(EnemyEntity enemy, Transform targetTrans) { 
        AniState curState = enemy.transform.GetComponent<EnemyEntity>().curAniState; 
        if(curState!=AniState.Born) {
            Vector3 target = targetTrans.position;
            Vector3 self = enemy.transform.position;
            target.y = 0;
            self.y = 0;
            float dis = Vector3.Distance(target,self);
            if(dis <= _trackingDis) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        } 
    }
}
