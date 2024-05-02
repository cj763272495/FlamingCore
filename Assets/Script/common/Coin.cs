using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin:PickUpItem {
    public int CoinValue { get; private set; }
    private Transform playerTransform;

    private void Start() {
        CoinValue = 5;
    }

    private IEnumerator MoveTowardsPlayer() {
        while(playerTransform != null) {
            //使物体移动到玩家位置 
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 1f);
            //当距离小于0.1时销毁
            if(Vector3.Distance(transform.position,playerTransform.position) < 0.1f) {
                AudioManager.Instance.PlaySound(ResSvc.Instance.LoadAudio(Constants.EarnMoneyClip));
                ParticleMgr.Instance.PlayGetCoinParticle(transform.position);
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag("Player")) {
            playerTransform = other.transform;
            StartCoroutine(MoveTowardsPlayer());
        }
    }
}
