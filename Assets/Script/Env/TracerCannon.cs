using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerCannon : MonoBehaviour
{
    public GameObject missile; // 炮弹预制体
    public float fireInterval = 5.0f; // 发射间隔
    private Coroutine fireCoroutine; // 发射协程

    private void Start() { 
        PoolManager.Instance.InitPool(missile,3,BattleSys.Instance.battleMgr.transform);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            // 如果是玩家，开始发射炮弹
            fireCoroutine = StartCoroutine(FireBullets(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player") {
            // 如果是玩家，停止发射炮弹
            if(fireCoroutine != null) {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }
    }

    private IEnumerator FireBullets(GameObject player) {
        while(true) {
            // 创建并发射炮弹
            PoolManager.Instance.GetInstance<GameObject>(missile);
            //todo set player
            // 等待fireInterval秒
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
