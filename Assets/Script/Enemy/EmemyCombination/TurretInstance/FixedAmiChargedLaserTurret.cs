using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FixedAmiChargedLaserTurret: EnemyEntity {

    public GameObject spwanLaser;
    public LineRenderer linerender;
    public GameObject firePoint;

    private Turret turret;

    private FixedBase fixedBase;
    private AmiRotationMode rotationMode;
    private LaserWeapon laserWeapon;
    private ChargedLaser laser;

    private void Start() {
        turret = new Turret();

        fixedBase = new FixedBase();
        turret.SetTurretBase(fixedBase);

        rotationMode = new AmiRotationMode(transform);
        turret.SetRotationMode(rotationMode);

        laserWeapon = new LaserWeapon();
        laser = new ChargedLaser(spwanLaser,linerender,firePoint,this);
        laserWeapon.SetFireMode(laser);
        turret.SetWeapon(laserWeapon);
    }

    public override void Update() {
        base.Update();
        if(player) {
            turret.Fire();
            //turret.Rotate(player.transform);
            //turret.SetMove(transform);
        }
    }
}
