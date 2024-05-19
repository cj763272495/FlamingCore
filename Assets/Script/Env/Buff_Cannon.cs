using System.Collections; 
using UnityEngine;

public class Buff_Cannon:MonoBehaviour {
    public Animator animator;
    private PlayerController playerController;
    public Collider _collider;
    private bool rotateTrt=false;

    public const float OnBuffCannonGuideLineLen = 10;
    private FloatingJoystick joystick;

    public ParticleSystem ps;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            ToolClass.ChangeCameraFov(Camera.main,Constants.OverloadFov,1);
            _collider.enabled = false;
            ToolClass.SetGameObjectXZPos(other.gameObject,transform.position);
            transform.rotation = Quaternion.LookRotation(BattleSys.Instance.battleMgr.joyStickDir);

            animator.SetBool("Show",true);
            playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.EnterIdleState(false);
            joystick = BattleSys.Instance.battleMgr.joystick;
            joystick.OnPointerDownAction += OnPointerDown;
            joystick.OnPointerUpAction += OnPointerUp;
            joystick.enabled = false;
            rotateTrt = true;
            ps.Play();
        }
    }

    public void EnablePlayerCtr() {
        joystick.enabled = true;
    }

    public void OnPointerDown() { 
        if(rotateTrt) {
            rotateTrt = true;
            StartCoroutine(RotateTrt());
            BattleSys.Instance.battleMgr.guideLine.SetLen(OnBuffCannonGuideLineLen);
        }
    }

    private IEnumerator RotateTrt() {  
        while(rotateTrt) { 
            transform.rotation = Quaternion.LookRotation(BattleSys.Instance.battleMgr.joyStickDir);
            yield return null;
        }
    }

    public void OnPointerUp() {
        animator.SetBool("Attack",true);
    }

    public void Attack() {
        playerController.ExitIdleState();
        playerController.EnterOverloadMode();
        ParticleMgr.Instance.PlayOverLoadParticle(playerController);
        joystick.OnPointerUpAction -= OnPointerUp;
        joystick.OnPointerUpAction -= OnPointerDown;
        BattleSys.Instance.battleMgr.guideLine.SetLen(Constants.MaxGuideLineLen); 
        rotateTrt = false;
        ps.Stop();
        StopAllCoroutines(); 
    }

    public void EndHideAnimation() {  
        _collider.enabled = true;
        animator.SetBool("Show",false);
        animator.SetBool("Attack",false);
    }
}
