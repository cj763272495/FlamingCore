
public class FixedAmiNormalTurret: EnemyEntity {
    private FixedBase fixedAutorotationBase; 
    private AmiRotationMode amiRotationMode;
    private NormalBulletWeapon bulletWeapon;
    private SingleFire SingleFire;

    public override void Start() { 
        base.Start();
        fixedAutorotationBase = new FixedBase();
        turret.SetTurretBase(fixedAutorotationBase);

        bulletWeapon = new NormalBulletWeapon();
        SingleFire = new SingleFire(firePoints);
        SingleFire.countDown = countDown;
        bulletWeapon.SetFireMode(SingleFire);
        turret.SetWeapon(bulletWeapon);

        amiRotationMode = new AmiRotationMode(transform, rotateSpeed);
        turret.SetRotationMode(amiRotationMode);
    }

    public override void Update() {
        base.Update(); 
    }
}
