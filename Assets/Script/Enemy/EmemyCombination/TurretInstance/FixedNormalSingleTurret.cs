
public class FixedAutorotationNormalTripleTurret : EnemyEntity {
    private FixedBase fixedAutorotationBase; 
    private AutoRotationMode autoRotationMode; 
    private NormalBulletWeapon bulletWeapon;
    private TripleFire tripleFire; 

    public override void Start() { 
        base.Start();
        fixedAutorotationBase = new FixedBase();
        turret.SetTurretBase(fixedAutorotationBase);

        bulletWeapon = new NormalBulletWeapon();
        tripleFire = new TripleFire(firePoints);
        tripleFire.countDown = countDown;
        bulletWeapon.SetFireMode(tripleFire);
        turret.SetWeapon(bulletWeapon);
        
        autoRotationMode = new AutoRotationMode(transform);
        turret.SetRotationMode(autoRotationMode);
    }

    public override void Update() {
        base.Update(); 
    }
}
