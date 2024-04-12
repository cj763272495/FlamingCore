using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Enemy
{
    private void Update() {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime); 
    }
}
