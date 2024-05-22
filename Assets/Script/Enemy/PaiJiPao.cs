 using UnityEngine;

public class PaiJiPao : MonoBehaviour
{
    public GameObject paiJiPaoDan;
    public Transform firePoint;
    public float fireRate = 6.0f;
    private float nextFireTime = 0.0f;

    private void Start() {
        PoolManager.Instance.InitPool(paiJiPaoDan,1,BattleSys.Instance.battleMgr.transform);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && Time.time > nextFireTime) {
            PaiJiPaoDan paodan = PoolManager.Instance.GetInstance<GameObject>(paiJiPaoDan).GetComponent<PaiJiPaoDan>();
            paodan.SetPlayer(other.gameObject);
            paodan.transform.position = firePoint.position;
            paodan.transform.up = firePoint.forward;

            nextFireTime = Time.time + fireRate;
        }
    }

}
