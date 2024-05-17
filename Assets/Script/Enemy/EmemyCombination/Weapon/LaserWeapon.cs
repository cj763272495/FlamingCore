using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{
    public override void Fire() {
        fireMode.Fire();
    }
}
