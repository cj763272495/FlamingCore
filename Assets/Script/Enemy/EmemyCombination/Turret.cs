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
        _weapon.Fire();
    }

    public virtual void Rotate(Transform playerTrans = null) {
        rotationMode.Rotate(playerTrans);
        // Default implementation
    }

    public virtual void SetMove(Transform trans, Transform targetTrans) {
        _base.Move(trans, targetTrans); 
    }
}
