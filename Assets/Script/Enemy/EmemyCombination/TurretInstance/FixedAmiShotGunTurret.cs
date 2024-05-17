
public class FixedAmiShotGunTurret :EnemyEntity  {
    private FixedBase fixedBase;
    private NormalBulletWeapon bulletWeapon;
    private ShotgunFire tripleFire;
    private AmiRotationMode rotationMode; 

    public int bulletNum = 3;
    public float spreadAngle = 45f;

    public override void Start() { 
        base.Start(); 
        fixedBase = new FixedBase();
        turret.SetTurretBase(fixedBase);

        bulletWeapon = new NormalBulletWeapon();
        tripleFire = new ShotgunFire(firePoints, bulletNum, spreadAngle);
        tripleFire.countDown = countDown;
        bulletWeapon.SetFireMode(tripleFire); 
        turret.SetWeapon(bulletWeapon);

        rotationMode = new AmiRotationMode(transform); 
        turret.SetRotationMode(rotationMode);
    }
    

    public override void Update() {
        base.Update(); 
    }
}
