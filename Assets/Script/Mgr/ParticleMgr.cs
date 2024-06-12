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
        if(Instance != null) {
            Debug.LogError("Another instance of ParticleMgr already exists!");
            return;
        }
        Instance = this;
    }

    public void Init(BattleMgr battle) {
        battleMgr = battle;        // 碰撞墙壁的粒子特效 
        hitWallParticle = InitParticlePool("Prefab/Particles/Cores/HitWallParticle");
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
        particle.transform.parent = battleMgr.transform;
        if(particle != null) {
            PoolManager.Instance.InitPool(particle, num, battleMgr.transform);
        } else {
            Debug.LogError("InitParticlePool failed, particle is null, path: " + path);
        }
        return particle;
    }

    private GameObject PlayParticle(GameObject particle,Vector3 position) {
        GameObject go = PoolManager.Instance.GetInstance<GameObject>(particle);
        go.transform.position = position;
        go.transform.parent = battleMgr.transform;
        go.GetComponentInChildren<ParticleSystem>().Play();
        StartCoroutine(ReturnPool(go));//播放完成返回对象池
        return go;
    }

    IEnumerator ReturnPool(GameObject particle) { 
        yield return new WaitWhile(() => particle.GetComponentInChildren<ParticleSystem>().isPlaying); 
        PoolManager.Instance.ReturnToPool(particle);
    }

    public void AddCustomParticle(GameObject particle, int num=3) {
        PoolManager.Instance.InitPool(particle, num, battleMgr.transform);
    }
    
    public void PlayCustomParticle(GameObject particle, Vector3 position) {
        PlayParticle(particle, position);
    }

    public void PlayHitWallParticle(ContactPoint contact) {  
        GameObject go = PlayParticle(hitWallParticle,contact.point); 
        Vector3 normal = contact.normal;
        Quaternion rotation = Quaternion.LookRotation(normal);
        go.GetComponent<ParticleSystem>().transform.rotation = rotation; 
    }

    public void PlayEnemyDeadParticle(ContactPoint contact, Transform player) {
        GameObject go = PlayParticle(enemyDeadParticle,contact.point);
        ParticleSystem[] particleSystems = go.GetComponentsInChildren<ParticleSystem>();
        ParticleSystem lastParticleSystem = particleSystems[particleSystems.Length - 1];
        EnemyDeadCoin enemyDeadCoin = lastParticleSystem.gameObject.GetComponent<EnemyDeadCoin>();
        enemyDeadCoin.isPlaying = true;
        enemyDeadCoin.player = player.gameObject; 
        if(contact.otherCollider.GetComponent<EnemyEntity>() is EnemyEntity enemy) {
            enemyDeadCoin.coinValue = enemy.destoryCoinValue;
        }
    }
    public void PlayEnemyDeadParticle(Transform trans, Transform player) {
        GameObject go = PlayParticle(enemyDeadParticle, trans.position);
        ParticleSystem[] particleSystems = go.GetComponentsInChildren<ParticleSystem>();
        ParticleSystem lastParticleSystem = particleSystems[particleSystems.Length - 1];
        EnemyDeadCoin enemyDeadCoin = lastParticleSystem.gameObject.GetComponent<EnemyDeadCoin>();
        enemyDeadCoin.isPlaying = true;
        enemyDeadCoin.player = player.gameObject;
        if(trans.GetComponent<EnemyEntity>() is EnemyEntity enemy) {
            enemyDeadCoin.coinValue = enemy.destoryCoinValue;
        }
    }

    public void PlayBulletDestoryParticle(Vector3 contact) {
        PlayParticle(bulletDestoryParticle, contact);
    }

    public void PlayGetCoinParticle(Vector3 point) {
        PlayParticle(getCoinParticle, point);
    }

    public void PlayDeadParticle(Vector3 point) {
        PlayParticle(deadParticle, point);
    }
    public void PlayOverLoadParticle( PlayerController player) {
        GameObject go =  PlayParticle(overLoadParticle, player.transform.position);
        // 使用协程实现粒子特效跟随玩家，播放完成后停止协程
        StartCoroutine(OverLoadParticleFollowPlayer(go, player));
    }
    private IEnumerator OverLoadParticleFollowPlayer(GameObject particle, PlayerController player) {
        while(particle && particle.GetComponentInChildren<ParticleSystem>().isPlaying) {
            particle.transform.position = player.transform.position; 
            particle.transform.forward = player.Dir;
            yield return null;
        }
        if(player.destructible) {
            particle.GetComponentInChildren<ParticleSystem>().Stop(); 
        }
    }


}
