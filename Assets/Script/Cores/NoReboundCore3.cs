using UnityEngine;

public class NoReboundCore : PlayerController
{
    protected override void OnCollisionEnter(Collision collision) {
        int collisionLayer = collision.gameObject.layer;
        ContactPoint contactPoint = collision.contacts[0];

        if(destructible && collisionLayer == 7) {//bullet 
            PlayerDead();
        } else if(collisionLayer == 14) {//enemy 
        } else {
            battleMgr.particleMgr.PlayHitWallParticle(contactPoint);
            battleMgr.PlayHitWallClip();
        }

        if(collisionLayer!=14) {
            //计算反射方向
            Vector3 inDirection = (transform.position - lastPos).normalized;
            Vector3 inNormal = contactPoint.normal;
            inNormal.y = 0;
            Vector3 tempDir = Vector3.Reflect(inDirection,inNormal).normalized;
            if(!Physics.Raycast(transform.position,tempDir,0.5f)) {
                _dir = tempDir;
            } else {
                _dir = Vector3.Reflect(tempDir,inNormal).normalized;
            } 
        }
        lastPos = transform.position; //更新上一次位置，用于计算反射方向
    }
}
