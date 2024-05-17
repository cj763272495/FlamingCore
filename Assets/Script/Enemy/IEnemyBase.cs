using System;
using UnityEngine;

public interface IEnemyBase {
    void Update();
    void OnCollisionEnter(Collision collision);
}
 