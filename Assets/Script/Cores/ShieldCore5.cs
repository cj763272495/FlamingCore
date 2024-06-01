using UnityEngine;

public class ShieldCore : PlayerController
{
    public ParticleSystem sheildParticle; 
    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        int collisionLayer = collision.gameObject.layer;

        //可摧毁并且撞到子弹
        //不可摧毁或者撞到其他东西
        if(!destructible || collisionLayer != 7) {//bullet
            GetField(); 
            battleMgr.joystick.OnPointerUpAction += OnPointerUpAction;
        } else if(destructible && collisionLayer==7) {
            ToolClass.CallAfterDelay(0.5f,() => { // 被击0.5秒后销毁护盾
                DisappearField();
            });
        }
    }

    public void OnPointerUpAction() {
        DisappearField();
        battleMgr.joystick.OnPointerUpAction -= OnPointerUpAction;
    }

    public void GetField() {
        sheildParticle.gameObject.SetActive(true);
        sheildParticle.Play();
        destructible = false;
    }

    public void DisappearField() {
        sheildParticle.gameObject.SetActive(false);
        destructible = true;
        sheildParticle.Stop();
    }
}
