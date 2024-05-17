using UnityEngine;

public class Turret {
    protected Weapon _weapon;
    protected TurretBase _base;

    protected IRotationMode rotationMode;

    public void SetWeapon(Weapon weapon) {
        _weapon = weapon;
    }

    public void SetTurretBase(TurretBase turretBase) {
        _base = turretBase;
    }

    public void SetRotationMode(IRotationMode rotationMode) {
        this.rotationMode = rotationMode;
    }

    public virtual void Fire() {
        if(_weapon!=null) { 
            _weapon.Fire();
        }
    }

    public virtual void Rotate(Transform playerTrans = null) {
        if(rotationMode != null) {
            rotationMode.Rotate(playerTrans);
        }
    }

    public virtual void SetMove(EnemyEntity enemy, Transform targetTrans) {
        if (_base!=null) {
            _base.Move(enemy,targetTrans); 
        }
    }
}
