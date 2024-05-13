using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin:PickUpItem {
    public int CoinValue { get; private set; }
    private Transform playerTransform;
    private AudioClip earnMoneyClip;

    private void Start() {
        CoinValue = 5;
        earnMoneyClip = ResSvc.Instance.LoadAudio(Constants.EarnMoneyClip);
    }

    protected override void Update() {
        base.Update();
        if(playerTransform != null) {
            transform.position = Vector3.MoveTowards(transform.position,playerTransform.position,40f*Time.deltaTime);
            if(Vector3.Distance(transform.position,playerTransform.position) < 0.1f) {
                AudioManager.Instance.PlaySound(earnMoneyClip);
                ParticleMgr.Instance.PlayGetCoinParticle(transform.position);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag("Player")) {
            playerTransform = other.transform; 
        }
    }
}
