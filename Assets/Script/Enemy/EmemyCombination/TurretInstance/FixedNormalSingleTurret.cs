using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedAutorotationNormalTripleTurret : EnemyEntity {
    private FixedBase fixedAutorotationBase;

    private AutoRotationMode autoRotationMode;

    private NormalBulletWeapon bulletWeapon;
    private TripleFire tripleFire;

    private Turret turret;

    public Transform shootPoint; 
    public Image countDown;

    void Start() {
        turret = new Turret();
         
        fixedAutorotationBase = new FixedBase();
        turret.SetTurretBase(fixedAutorotationBase);

        bulletWeapon = new NormalBulletWeapon();
        tripleFire = new TripleFire(shootPoint);
        tripleFire.countDown = countDown;
        bulletWeapon.SetFireMode(tripleFire);
        turret.SetWeapon(bulletWeapon);


        autoRotationMode = new AutoRotationMode(transform);
        turret.SetRotationMode(autoRotationMode);
    }
    

    public override void Update() {
        base.Update();
        if(player) {
            turret.SetMove(transform,player.transform);
            turret.Fire();
            turret.Rotate();
        }
    }
}
