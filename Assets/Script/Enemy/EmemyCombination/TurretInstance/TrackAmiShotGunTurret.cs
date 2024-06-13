
public class TrackAmiShotGunTurret: EnemyEntity  {
    private TrackingBase trackingBase;
    private NormalBulletWeapon bulletWeapon;
    private ShotgunFire tripleFire;
    public AmiRotationMode rotationMode; 

    public int bulletNum = 3;
    public float spreadAngle = 45f; 

    public override async void Start() { 
        base.Start();
        trackingBase = new TrackingBase();
        turret.SetTurretBase(trackingBase);

        bulletWeapon = new NormalBulletWeapon();
        tripleFire = new ShotgunFire(firePoints, bulletNum, spreadAngle);
        tripleFire.countDown = countDown;
        bulletWeapon.SetFireMode(tripleFire); 
        turret.SetWeapon(bulletWeapon);

        rotationMode = new AmiRotationMode(transform); 
        turret.SetRotationMode(rotationMode);
        Born();
        await ToolClass.CallAfterDelay(1.06f, Idle);
    }
    

    public override void Update() {
        base.Update();

    }
}
