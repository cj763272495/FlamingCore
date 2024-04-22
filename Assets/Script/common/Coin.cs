using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PickUpItem {
    public int coinValue { get; private set; }

    private void Start() {
        coinValue = 5;
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            other.gameObject.GetComponent<SoundPlayer>().clipSource
                = Resources.Load<AudioClip>(Constants.EarnMoneyClip);
            other.gameObject.GetComponent<SoundPlayer>().PlaySound();
            Destroy(gameObject);
        }
    }
}
