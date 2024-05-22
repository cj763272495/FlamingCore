using UnityEngine; 
using System.Collections;

public class ParticleMgr : MonoBehaviour
{
    public static ParticleMgr Instance { get; private set; }
    private GameObject hitWallParticle;
    private GameObject enemyDeadParticle;
    private GameObject deadParticle;
    private GameObject bulletDestoryParticle;
    private GameObject getCoinParticle; 
    private GameObject overLoadParticle;

    public BattleMgr battleMgr;

    private void Awake() {
        Instance = this;
    }

    public void Init(BattleMgr battle) {
        battleMgr = battle;
    }
    private void Start() {
        // 碰撞墙壁的粒子特效 
        hitWallParticle = InitParticlePool( "Prefab/Particles/Cores/HitWallParticle");
        //碰撞敌人的粒子特效
        enemyDeadParticle = InitParticlePool("Prefab/Particles/EnemyDeadParticle");
        //玩家死亡的粒子特效
        deadParticle = InitParticlePool("Prefab/Particles/Cores/DeadParticle",1);
        //子弹销毁的粒子特效
        bulletDestoryParticle = InitParticlePool("Prefab/Particles/BulletDestoryParticle",10);
        //获取金币特效 
        getCoinParticle = InitParticlePool("Prefab/Particles/GetCoinParticle",5);
        //超载模式特效
        overLoadParticle = InitParticlePool("Prefab/Particles/Cores/OverLoadParticle",1);
    }

    public GameObject InitParticlePool(string path,int num = 3) {
        GameObject particle = ResSvc.Instance.LoadPrefab(path);
        if(particle != null) {
            PoolManager.Instance.InitPool(particle,num,battleMgr.transform);
        } else {
            Debug.LogError("InitParticlePool failed, particle is null, path: " + path);
        }
        return particle;
    }

    public void AddCustomParticle(GameObject particle, int num=3) {
        PoolManager.Instance.InitPool(particle, num, battleMgr.transform);
    }

    public void PlayCustomParticle(GameObject particle, Vector3 position) {
        GameObject go = PoolManager.Instance.GetInstance<GameObject>(particle);
        go.transform.position =position;
        go.transform.parent = battleMgr.transform;
        go.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void PlayHitWallParticle(ContactPoint contact) {
        GameObject go =  PoolManager.Instance.GetInstance<GameObject>(hitWallParticle);
        Vector3 point = contact.point;
        Vector3 normal = contact.normal;
        go.transform.position = point;
        go.transform.parent = battleMgr.transform;
        Quaternion rotation = Quaternion.LookRotation(normal);
        go.GetComponent<ParticleSystem>().transform.rotation = rotation;
        go.GetComponent<ParticleSystem>().Play();
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
        EnemyEntity enemy = contact.otherCollider.GetComponent<EnemyEntity>();
        if(enemy) {
            enemyDeadCoin.coinValue = enemy.destoryCoinValue;
        }
    }

    public void PlayBulletDestoryParticle(ContactPoint contact) {
        GameObject go = PoolManager.Instance.GetInstance<GameObject>(bulletDestoryParticle);
        if(go) {
            Vector3 point = contact.point;
            Vector3 normal = contact.normal;
            go.transform.position = point;
            go.transform.parent = battleMgr.transform;

            Quaternion rotation = Quaternion.LookRotation(normal);
            go.GetComponentInChildren<ParticleSystem>().transform.rotation = rotation;
            go.GetComponentInChildren<ParticleSystem>().Play();

        }
    }

    public void PlayGetCoinParticle(Vector3 point) {
        GameObject go = PoolManager.Instance.GetInstance<GameObject>(getCoinParticle);
        go.transform.position = point;
        go.transform.parent = battleMgr.transform;
        go.GetComponent<ParticleSystem>().Play();
    }

    public void PlayDeadParticle(Vector3 point) {
        GameObject go = PoolManager.Instance.GetInstance<GameObject>(deadParticle);
        go.transform.position = point;
        go.transform.parent = battleMgr.transform;
        go.GetComponent<ParticleSystem>().Play();
    }
    public void PlayOverLoadParticle( PlayerController player) {
        GameObject go = PoolManager.Instance.GetInstance<GameObject>(overLoadParticle);
        go.transform.position = player.transform.position;
        go.transform.parent = battleMgr.transform;
        go.transform.forward = player.Dir;
        go.GetComponentInChildren<ParticleSystem>().Play();
        // 使用协程实现粒子特效跟随玩家，播放完成后停止协程
        StartCoroutine(OverLoadParticleFollowPlayer(go, player));
    }
    private IEnumerator OverLoadParticleFollowPlayer(GameObject particle, PlayerController player) {
        while(particle != null && particle.GetComponentInChildren<ParticleSystem>().isPlaying) {
            particle.transform.position = player.transform.position; 
            particle.transform.forward = player.Dir;
            yield return null;
        }
        if(player.destructible) {
            particle.GetComponentInChildren<ParticleSystem>().Stop(); 
        }
    }

    private void PlayParticle(GameObject particle, Vector3 position) {
        GameObject go = PoolManager.Instance.GetInstance<GameObject>(particle);
        go.transform.position = position;
        go.transform.parent = battleMgr.transform;
        go.GetComponentInChildren<ParticleSystem>().Play();
    }
}
