using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float rotateSpeed = 100;
    public PlayerController player;
    public Rigidbody rb;
    private SoundPlayer soundPlayer;

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            soundPlayer = collision.gameObject.GetComponent<SoundPlayer>();
            soundPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.HitEnenmyClip);
            soundPlayer.PlaySound();
            Destroy(gameObject);
        }
    }

}
