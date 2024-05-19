using System;
using UnityEngine;

public class ShieldCore : PlayerController
{
    public ParticleSystem sheildParticle;
    //public bool hasSheild = false;
    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        int collisionLayer = collision.gameObject.layer;

        //�ɴݻٲ���ײ���ӵ�
        //���ɴݻٻ���ײ����������
        if(!destructible || collisionLayer != 7) {//bullet
            GetField(); 
            battleMgr.joystick.OnPointerUpAction += OnPointerUpAction;
        } else if(destructible && collisionLayer==7) {
            ToolClass.CallAfterDelay(0.5f,() => { // ����0.5������ٻ���
                DisappearField();
            }); 
        }
    }

    public void OnPointerUpAction() { 
        DisappearField();
        battleMgr.joystick.OnPointerUpAction -= OnPointerUpAction;
    }

    public void GetField() { 
        sheildParticle.Play();
        destructible = false;
    }

    public void DisappearField() { 
        destructible = true;
        sheildParticle.Stop();
    }
}