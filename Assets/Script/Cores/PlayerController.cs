using System.Collections; 
using UnityEngine;
using DG.Tweening;

public class PlayerController:Entity {
    protected float _speed;
    public float Speed { get { return _speed; } }
    protected float _rotateSpeed;
    public bool isMove;
    protected Vector3 _dir;

    public Vector3 Dir {
        get { return _dir; }
    }
     
    public Rigidbody _rb;
    public Vector3 lastPos;

    public Transform camTrans; 

    public BattleMgr battleMgr;
    public bool destructible=true;
    public ParticleSystem overLoadField;
    public StateMgr stateMgr;

    public virtual void Init(BattleMgr battle=null,StateMgr state=null) {
        battleMgr = battle;
        stateMgr = state;
        lastPos = transform.position;
        camTrans = Camera.main.transform;
        destructible = true;
        _speed = Constants.PlayerSpeed;
        _rotateSpeed = Constants.RotateSpeed;
        isMove = true;
    }

    protected virtual void FixedUpdate() {
        if(battleMgr.StartBattle && isMove) {
            SetMove(); 
            SetRotateAndShot();
        }
    }

    protected virtual void LateUpdate() {
        if(battleMgr.StartBattle && isMove) { 
            SetCam();
        }
    }

    protected virtual void Update() {  }

    public void SetDir(Vector3 direnction) {
        lastPos = transform.position;
        _dir = direnction;
    }

    public virtual void OnPointerDown() { }

    public virtual void OnDrag() {}

    public virtual void OnPointerUp() {
        ExitOverloadMode();
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        int collisionLayer = collision.gameObject.layer;
        ContactPoint contactPoint = collision.contacts[0];
        if(destructible && collisionLayer == 7) {//bullet 
            PlayerDead();
        } else if(collisionLayer == 14) {

        } else {
            battleMgr.particleMgr.PlayHitWallParticle(contactPoint);
            battleMgr.PlayHitWallClip();
        }

        if(_speed == Constants.OverloadSpeed && collisionLayer ==14 || _speed == Constants.OverloadSpeed && collisionLayer==7) {
            return;
        }
        //计算反射方向
        Vector3 inDirection = (transform.position - lastPos).normalized;
        Vector3 inNormal = contactPoint.normal;
        inNormal.y = 0;
        Vector3 tempDir = Vector3.Reflect(inDirection,inNormal).normalized;
        if(!Physics.Raycast(transform.position,tempDir,0.5f)) {
            _dir = tempDir;
        } else {
            _dir = Vector3.Reflect(tempDir,inNormal).normalized;
        }
        lastPos = transform.position; //更新上一次位置，用于计算反射方向

        Vector3 originalPosition = camTrans.localPosition;

        // 抖动相机
        Camera.main.DOShakePosition(0.1f,0.1f).OnComplete(() => {
            // 抖动完成后重置位置
            camTrans.transform.localPosition = originalPosition;
        });
    }

    public void PlayerDead() {
        if(!destructible) {
            return;
        }
        battleMgr.EndBattle(false,transform.position);
        gameObject.SetActive(false);
        ParticleMgr.Instance.PlayDeadParticle(transform.position);
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider other) {
        if(!battleMgr.StartBattle) {
            return;
        }
        if (other.gameObject.layer == 9) { //pickupitem
            if (other.transform.CompareTag("coin")) {
                battleMgr.EarnCoin(other.gameObject.GetComponent<Coin>().CoinValue);
            }
        }
    }

    protected virtual void SetMove() { 
        _rb.MovePosition(transform.position + _dir * _speed * Time.deltaTime); 
    }
    protected virtual void SetRotateAndShot() {
        Vector3 rotationAxis = Vector3.Cross(_dir,Vector3.up);
        float rotationAmount = _speed * _rotateSpeed * Time.deltaTime;
        transform.Rotate(-rotationAxis,rotationAmount,Space.World);
    }


    public void SetCam() {
        if (camTrans != null) {
            camTrans.position = transform.position + battleMgr.defaultCamOffset;
        }
    }

    public void Revive() {
        StartCoroutine(ReviveIncincibility());
    }

    void OnDestroy() {
        StopAllCoroutines();
    }

    IEnumerator ReviveIncincibility() {
        GetShield();
        gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(2f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        DestoryShield();
    }

    public void EnterOverloadMode() {
        _speed = Constants.OverloadSpeed;
        lastPos = transform.position;
        CancelInvoke("ExitOverloadMode");
        GetShield();
        ToolClass.ChangeCameraFov(Camera.main,Constants.OverloadFov,1);
    }

    public void ExitOverloadMode() {
        if(_speed!=Constants.OverloadSpeed) {
            return;
        }
        _speed = Constants.PlayerSpeed;
        DestoryShield();
        CancelInvoke("EnterOverloadMode");
        ToolClass.ChangeCameraFov(Camera.main,battleMgr.DefaultFov,0.2f);
    }

    public void GetShield() { 
        destructible = false;
        overLoadField.Play();
    }
    public void DestoryShield() {
        overLoadField.Stop(); 
        destructible = true;
    }

    public void EnterIdleState(bool isActive) {
        gameObject.SetActive(isActive);
        isMove = false;
        lastPos = transform.position;
        _rb.velocity = Vector3.zero;
    }
    public void ExitIdleState() {
        isMove = true;
        gameObject.SetActive(true);
    }

    public void Born() {
        stateMgr.ChangeStatus(this,AniState.Born,null);
    }
    public void Take() {
        stateMgr.ChangeStatus(this,AniState.Take,null);
    }
    public void Idle() {
        stateMgr.ChangeStatus(this,AniState.Idle,null);
    } 
}
