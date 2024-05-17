using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletWeapon : Weapon
{
    public override void Fire() {
        fireMode.Fire();
    }
}
