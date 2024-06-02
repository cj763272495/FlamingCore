using System.Collections.Generic;
using UnityEngine;

public class FixedAutoRotationContinuesLaserTurret: EnemyEntity {

    public GameObject spwanLaser;
    public List<LineRenderer> linerenders;
    public float laserLen=20f;
    public LayerMask layer;

    private FixedBase fixedBase;
    private AutoRotationMode rotationMode;
    private LaserWeapon laserWeapon;
    private ContinuousLaser laser;

    public override void Start() {
        base.Start(); 
        fixedBase = new FixedBase();
        turret.SetTurretBase(fixedBase);

        rotationMode = new AutoRotationMode(transform,rotateSpeed);
        turret.SetRotationMode(rotationMode);

        if(spwanLaser && linerenders.Count>0) {
            laserWeapon = new LaserWeapon();
            laser = new ContinuousLaser(spwanLaser,linerenders,firePoints,layer,laserLen);
            laserWeapon.SetFireMode(laser);
            turret.SetWeapon(laserWeapon);
        } 
    }

    public override void Update() {
        base.Update(); 
    }
}
