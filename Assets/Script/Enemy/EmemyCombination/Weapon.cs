using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon 
{
    protected IFireMode fireMode;

    public void SetFireMode(IFireMode fireMode) {
        this.fireMode = fireMode;
    }

    public abstract void Fire();
}
