using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PickUpItem {
    public int CoinValue { get; private set; }

    private void Start() {
        CoinValue = 5;
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            other.gameObject.GetComponent<SoundPlayer>().clipSource
                = Resources.Load<AudioClip>(Constants.EarnMoneyClip);
            other.gameObject.GetComponent<SoundPlayer>().PlaySound();
            Destroy(gameObject);
        }
    }
}