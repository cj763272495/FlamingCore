using UnityEngine;

public class FixedAutoRotationContinuesLaserTurret: EnemyEntity {

    public GameObject spwanLaser;
    public LineRenderer linerender; 

    private FixedBase fixedBase;
    private AutoRotationMode rotationMode;
    private LaserWeapon laserWeapon;
    private ContinuousLaser laser;

    public override void Start() {
        base.Start(); 
        fixedBase = new FixedBase();
        turret.SetTurretBase(fixedBase);

        rotationMode = new AutoRotationMode(transform);
        turret.SetRotationMode(rotationMode);

        if(spwanLaser && linerender) {
            laserWeapon = new LaserWeapon();
            laser = new ContinuousLaser(spwanLaser,linerender,firePoints);
            laserWeapon.SetFireMode(laser);
            turret.SetWeapon(laserWeapon);
        } 
    }

    public override void Update() {
        base.Update(); 
    }
}
