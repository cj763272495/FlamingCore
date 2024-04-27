using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleMgr : MonoBehaviour
{
    public static ParticleMgr Instance { get; private set; }
    public GameObject hitWallParticle;
    public BattleMgr battleMgr;

    private void Awake() {
        Instance = this;
    }

    public void Init() {
        hitWallParticle = ResSvc.Instance.LoadPrefab("Prefab/HitWallParticle");
        PoolManager.Instance.InitPool(hitWallParticle, 2, battleMgr.transform);
    }

    public void PlayHitWallParticle(Vector3 point) {
        GameObject go =  PoolManager.Instance.
            GetInstance<GameObject>(hitWallParticle);
        go.transform.position = point;
        go.transform.parent = battleMgr.transform;
        go.GetComponentInChildren<ParticleSystem>().Play();
    }
}
