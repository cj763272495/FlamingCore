using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleMgr : MonoBehaviour
{
    public static ParticleMgr Instance { get; private set; }
    public GameObject hitWallParticle;
    public GameObject enemyDeadParticle;
    public GameObject deadParticle;
    public GameObject bulletDestoryParticle;
    public BattleMgr battleMgr;

    private void Awake() {
        Instance = this;
    }

    public void Init() {
        // 碰撞墙壁的粒子特效
        hitWallParticle = ResSvc.Instance.LoadPrefab("Prefab/Particles/HitWallParticle");
        PoolManager.Instance.InitPool(hitWallParticle, 3, battleMgr.transform);

        //碰撞敌人的粒子特效
        enemyDeadParticle = ResSvc.Instance.LoadPrefab("Prefab/Particles/EnemyDeadParticle");
        PoolManager.Instance.InitPool(enemyDeadParticle, 3, battleMgr.transform);

        //玩家死亡的粒子特效
        deadParticle = ResSvc.Instance.LoadPrefab("Prefab/Particles/DeadParticle");
        PoolManager.Instance.InitPool(deadParticle, 1, battleMgr.transform);

        //子弹销毁的粒子特效
        bulletDestoryParticle = ResSvc.Instance.LoadPrefab("Prefab/Particles/BulletDestoryParticle");
        PoolManager.Instance.InitPool(bulletDestoryParticle, 5, battleMgr.transform);
    }

    public void PlayHitWallParticle(ContactPoint contact) {
        GameObject go =  PoolManager.Instance.
            GetInstance<GameObject>(hitWallParticle);
        Vector3 point = contact.point;
        Vector3 normal = contact.normal;
        go.transform.position = point;
        go.transform.parent = battleMgr.transform;
        // 旋转go使go的z轴和碰撞点point的法线平行
        Quaternion rotation = Quaternion.LookRotation(normal);
        go.GetComponentInChildren<ParticleSystem>().transform.rotation = rotation;
        //go.transform.forward = point;
        go.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void PlayEnemyDeadParticle(ContactPoint contact, Transform player) {
        GameObject go = PoolManager.Instance.GetInstance<GameObject>(enemyDeadParticle);
        go.transform.position = contact.point;
        go.transform.parent = battleMgr.transform;
        go.GetComponent<ParticleSystem>().Play();

        ParticleSystem[] particleSystems = go.GetComponentsInChildren<ParticleSystem>();
        ParticleSystem lastParticleSystem = particleSystems[particleSystems.Length - 1];

        EnemyDeadCoin enemyDeadCoin = lastParticleSystem.gameObject.GetComponent<EnemyDeadCoin>();
        enemyDeadCoin.isPlaying = true;
        enemyDeadCoin.player = player.gameObject;
        Enemy enemy = contact.otherCollider.GetComponent<Enemy>();
        if(enemy) {
            enemyDeadCoin.coinValue = enemy.destoryCoinValue;
        }
    }



    public void PlayBulletDestoryParticle(ContactPoint contact) {
        GameObject go = PoolManager.Instance.
    GetInstance<GameObject>(bulletDestoryParticle);
        Vector3 point = contact.point;
        Vector3 normal = contact.normal;
        go.transform.position = point;
        go.transform.parent = battleMgr.transform;
        // 旋转go使go的z轴和碰撞点point的法线平行
        Quaternion rotation = Quaternion.LookRotation(normal);
        go.GetComponentInChildren<ParticleSystem>().transform.rotation = rotation;
        //go.transform.forward = point;
        go.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void PlayDeadParticle(Vector3 point) {
        GameObject go = PoolManager.Instance.
    GetInstance<GameObject>(deadParticle);
        go.transform.position = point;
        go.transform.parent = battleMgr.transform;
        go.GetComponent<ParticleSystem>().Play();
    }
}
