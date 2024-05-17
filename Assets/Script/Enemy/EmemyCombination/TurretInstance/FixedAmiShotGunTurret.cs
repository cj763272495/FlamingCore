using UnityEngine;
using UnityEngine.UI;

public class FixedAmiShotGunTurret :EnemyEntity  {
    private FixedBase fixedBase;
    private NormalBulletWeapon bulletWeapon;
    private ShotgunFire tripleFire;
    private AmiRotationMode rotationMode;

    public Turret turret;

    public Transform shootPoint;
    public Image countDown;

    public int bulletNum = 3;
    public float spreadAngle = 45f;

    void Start() { 
        turret = new Turret();

        fixedBase = new FixedBase();
        turret.SetTurretBase(fixedBase);

        bulletWeapon = new NormalBulletWeapon();
        tripleFire = new ShotgunFire(shootPoint, bulletNum, spreadAngle);
        tripleFire.countDown = countDown;
        bulletWeapon.SetFireMode(tripleFire); 
        turret.SetWeapon(bulletWeapon);

        rotationMode = new AmiRotationMode(transform); 
        turret.SetRotationMode(rotationMode);
    }
    

    public override void Update() {
        base.Update();
        if(player) {
            turret.SetMove(transform,player.transform);
            turret.Fire();
            turret.Rotate(player.transform);
        }
    }
}
