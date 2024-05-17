using UnityEngine;
using UnityEngine.UI;

public class TrackAmiShotGunTurret: EnemyEntity  {
    private TrackingBase trackingBase;
    private NormalBulletWeapon bulletWeapon;
    private ShotgunFire tripleFire;
    private AmiRotationMode rotationMode;

    public Turret turret;

    public Transform shootPoint;
    public Image countDown;

    public int bulletNum = 3;
    public float spreadAngle = 45f;

    public Animator ani;

    void Start() { 
        turret = new Turret();

        trackingBase = new TrackingBase(ani);
        turret.SetTurretBase(trackingBase);

        bulletWeapon = new NormalBulletWeapon();
        tripleFire = new ShotgunFire(shootPoint, bulletNum, spreadAngle);
        tripleFire.countDown = countDown;
        bulletWeapon.SetFireMode(tripleFire); 
        turret.SetWeapon(bulletWeapon);

        rotationMode = new AmiRotationMode(transform); 
        turret.SetRotationMode(rotationMode);

        ani.SetBool("Birth", true);
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
