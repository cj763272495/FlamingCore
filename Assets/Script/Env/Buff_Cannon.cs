using System.Collections; 
using UnityEngine;

public class Buff_Cannon : MonoBehaviour
{
    public Animator animator;
    private PlayerController playerController;
    public Collider _collider;
    private bool rotateTrt;
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            _collider.enabled = false;
            ToolClass.SetGameObjectPosXZ(other.gameObject, transform.position);
            animator.SetBool("Show",true);
            playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.EnterIdleState();
            BattleSys.Instance.battleMgr.joystick.OnPointerDownAction += OnPointerDown;
            BattleSys.Instance.battleMgr.joystick.OnPointerUpAction += OnPointerUp;
        }
    }


    public void OnPointerDown() {
        rotateTrt = true;
        StartCoroutine(RotateTrt());
    }

    private IEnumerator RotateTrt() {  
        while(rotateTrt) { 
            transform.rotation = Quaternion.LookRotation(-BattleSys.Instance.battleMgr.joyStickDir);
            yield return null;
        }
    }

    public void OnPointerUp() {
        animator.SetBool("Attack",true);
        rotateTrt = false;
    }


    public void Attack() {
        playerController.ExitIdleState();
        playerController.EnterOverloadMode();
        BattleSys.Instance.battleMgr.joystick.OnPointerUpAction -= OnPointerUp;
        BattleSys.Instance.battleMgr.joystick.OnPointerUpAction -= OnPointerDown;
    }


    public void EndHideAnimation() {  
        _collider.enabled = true;
        animator.SetBool("Show",false);
        animator.SetBool("Attack",false);
    }
}
