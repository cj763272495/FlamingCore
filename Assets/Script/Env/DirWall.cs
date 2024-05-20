using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.SetDir(transform.forward);
        }
    }
}
