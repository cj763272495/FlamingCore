using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : EnemyEntity
{
    public override void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up); 
    }
}
