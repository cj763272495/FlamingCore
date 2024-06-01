using UnityEngine;

public class TracerCannon : MonoBehaviour
{
    public GameObject missile; // ÅÚµ¯Ô¤ÖÆÌå
    public Transform firePoint;
    public float fireRate = 5.0f;
    private float nextFireTime = 0.0f;

    private void Start() { 
        PoolManager.Instance.InitPool(missile,3,BattleSys.Instance.battleMgr.transform);
    }
    private void OnTriggerStay(Collider other) {
        if(other.tag == "Player" && Time.time > nextFireTime) {
            Missile mis = PoolManager.Instance.GetInstance<GameObject>(missile,
                BattleSys.Instance.battleMgr.transform).GetComponent<Missile>(); 
            mis.transform.position = firePoint.position;
            mis.transform.up = firePoint.forward;
            mis.StartChase(other.gameObject);

            nextFireTime = Time.time + fireRate;
        }
    }
}
