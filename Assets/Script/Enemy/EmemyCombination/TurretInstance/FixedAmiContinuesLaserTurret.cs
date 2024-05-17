using UnityEngine;

public class FixedAmiContinuesLaserTurret: EnemyEntity {

    public GameObject spwanLaser;
    public LineRenderer linerender; 

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
        laser = new ContinuousLaser(spwanLaser,linerender,firePoints);
        laserWeapon.SetFireMode(laser);
        turret.SetWeapon(laserWeapon);
    }

    public override void Update() {
        base.Update(); 
    }
}
