using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBase:TurretBase { 
    public float moveSpeed = 0.5f;  
    private Animator ani;

    public TrackingBase(Animator ani) {
    }
    
    public override void Move(Transform trans,Transform targetTrans) {
        Vector3 direction = targetTrans.position - trans.position;
        direction.Normalize(); 
        Vector3 newPosition = trans.position + direction * moveSpeed * Time.deltaTime;
        trans.position = newPosition;
        if(ani) {
            ani.SetBool("Take",true);
        }
    }

}
