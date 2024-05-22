using System.Collections; 
using UnityEngine;

public class TracerCannon : MonoBehaviour
{
    public GameObject missile; // 炮弹预制体
    public float fireInterval = 5.0f; // 发射间隔
    private Coroutine fireCoroutine;
    public Transform firePoint;

    private void Start() { 
        PoolManager.Instance.InitPool(missile,3,BattleSys.Instance.battleMgr.transform);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") { 
            fireCoroutine = StartCoroutine(FireBullets(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player") { 
            if(fireCoroutine != null) {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }
    }

    private IEnumerator FireBullets(GameObject player) {
        while(true) {
            GameObject go = PoolManager.Instance.GetInstance<GameObject>(missile);
            Missile bullet = go.GetComponent<Missile>();
            bullet.transform.position = firePoint.position;
            bullet.transform.forward = firePoint.forward;
            bullet.StartChase(player.GetComponent<PlayerController>()); 
            //todo set player  
            // 等待fireInterval秒
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
