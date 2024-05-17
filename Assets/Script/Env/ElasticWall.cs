using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticWall : MonoBehaviour
{
    PlayerController _player;
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            _player = collision.gameObject.GetComponent<PlayerController>();
            _player.EnterOverloadMode(); 
        }
    }

    //public void OnPointerUp() {
    //    _player.ExitIdleState();
    //    ToolClass.CallAfterDelay(0.5f,() => {
    //        BattleSys.Instance.battleMgr.joystick.OnPointerUpAction -= OnPointerUp;
    //    });
    //}
}
