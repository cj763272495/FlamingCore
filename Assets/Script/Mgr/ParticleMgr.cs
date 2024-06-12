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
        battleMgr = battle;        // ��ײǽ�ڵ�������Ч 
        hitWallParticle = InitParticlePool("Prefab/Particles/Cores/HitWallParticle");
        //��ײ���˵�������Ч
        enemyDeadParticle = InitParticlePool("Prefab/Particles/EnemyDeadParticle");
        //���������������Ч
        deadParticle = InitParticlePool("Prefab/Particles/Cores/DeadParticle",1);
        //�ӵ����ٵ�������Ч
        bulletDestoryParticle = InitParticlePool("Prefab/Particles/BulletDestoryParticle",10);
        //��ȡ�����Ч 
        getCoinParticle = InitParticlePool("Prefab/Particles/GetCoinParticle",5);
        //����ģʽ��Ч
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
        StartCoroutine(ReturnPool(go));//������ɷ��ض����
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
        // ʹ��Э��ʵ��������Ч������ң�������ɺ�ֹͣЭ��
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
