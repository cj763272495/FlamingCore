using System.Collections; 
using UnityEngine;

public class TracerCannon : MonoBehaviour
{
    public GameObject missile; // �ڵ�Ԥ����
    public float fireInterval = 5.0f; // ������
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
            // �ȴ�fireInterval��
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
