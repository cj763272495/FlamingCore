using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticWall : MonoBehaviour //µ¯Á¦Ç½
{
    PlayerController _player;
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            _player = collision.gameObject.GetComponent<PlayerController>();
            _player.EnterOverloadMode(); 
        }
    }
}
