
public class FixedAutorotationNormalTurret: EnemyEntity {
    private FixedBase fixedAutorotationBase; 
    private AutoRotationMode autoRotationMode;
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

        autoRotationMode = new AutoRotationMode(transform, rotateSpeed);
        turret.SetRotationMode(autoRotationMode);
    }

    public override void Update() {
        base.Update(); 
    }
}
