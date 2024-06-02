using System.Collections.Generic;
using UnityEngine;

public class FixedAmiContinuesLaserTurret: EnemyEntity {

    public LayerMask layer;
    public GameObject spwanLaser;
    public List<LineRenderer> linerenders;

    private FixedBase fixedBase;
    private AmiRotationMode rotationMode;
    private LaserWeapon laserWeapon;
    private ContinuousLaser laser;

    public override void Start() {
        base.Start(); 
        fixedBase = new FixedBase();
        turret.SetTurretBase(fixedBase);

        rotationMode = new AmiRotationMode(transform);
        turret.SetRotationMode(rotationMode);

        laserWeapon = new LaserWeapon();
        laser = new ContinuousLaser(spwanLaser,linerenders,firePoints,layer);
        laserWeapon.SetFireMode(laser);
        turret.SetWeapon(laserWeapon);
    }

    public override void Update() {
        base.Update(); 
    }
}
