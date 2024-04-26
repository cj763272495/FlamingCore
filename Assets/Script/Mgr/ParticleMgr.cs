using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleMgr : MonoBehaviour
{
    public static ParticleMgr Instance { get; private set; }
    public ParticleSystem hitWallParticle;
    public BattleMgr battleMgr;

    private void Awake() {
        Instance = this;
    }

    public void Init() {
        hitWallParticle = ResSvc.Instance.LoadPrefab("Prefab/HitWallParticle").GetComponent<ParticleSystem>();
        PoolManager.Instance.InitPool(hitWallParticle, 2, battleMgr.transform);
    }

    public void PlayHitWallParticle(Vector3 point) {
        ParticleSystem ps =  PoolManager.Instance.
            GetInstance<ParticleSystem>(hitWallParticle);
        ps.gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        ps.transform.position = point;
        ps.transform.parent = battleMgr.transform;
        ps.Play();
    }
}
