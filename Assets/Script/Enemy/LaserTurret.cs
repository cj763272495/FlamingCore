using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Enemy
{
    private void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up); 
    }
}
