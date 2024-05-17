using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimingTurretBase: EnemyEntity {

    public override void Update() {
        if(player) {
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        }
    }
}
