using UnityEngine;

public class FixedAmiChargedLaserTurret: EnemyEntity {

    public GameObject spwanLaser;
    public LineRenderer linerender; 

    private FixedBase fixedBase;
    private AmiRotationMode rotationMode;
    private LaserWeapon laserWeapon;
    private ChargedLaser laser;

    public override void Start() {
        base.Start(); 
        fixedBase = new FixedBase();
        turret.SetTurretBase(fixedBase);

        rotationMode = new AmiRotationMode(transform);
        turret.SetRotationMode(rotationMode);

        laserWeapon = new LaserWeapon();
        laser = new ChargedLaser(spwanLaser,linerender,firePoints,this);
        laserWeapon.SetFireMode(laser);
        turret.SetWeapon(laserWeapon);
    }

    public override void Update() {
        base.Update(); 
    }
}
