using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdsorptionWall : MonoBehaviour
{ 
    PlayerController _player;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            _player = collision.gameObject.GetComponent<PlayerController>();
            _player.EnterIdleState(true);
            FloatingJoystick joystick = BattleSys.Instance.battleMgr.joystick; 
            joystick.OnPointerUpAction += OnPointerUp;
        }
    }

    public void OnPointerUp() {
        _player.ExitIdleState();
        ToolClass.CallAfterDelay(0.5f, () => {
            BattleSys.Instance.battleMgr.joystick.OnPointerUpAction -= OnPointerUp;
        });
    }
}
