using System.Collections; 
using UnityEngine; 

public class PlayerController: Entity {
    protected float _speed;
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
            SetCam();
            SetRotateAndShot();
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
        } else if(collisionLayer == 14) {//enemy
            battleMgr.EliminateEnemy(); 
        } else {
            battleMgr.particleMgr.PlayHitWallParticle(contactPoint);
            battleMgr.PlayHitWallClip();
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
    }

    public void PlayerDead() {
        battleMgr.EndBattle(false,transform.position);
        gameObject.SetActive(false);
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
        //transform.position = Vector3.MoveTowards(transform.position,
            //transform.position + _dir.normalized,_speed * Time.deltaTime);
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

    IEnumerator ReviveIncincibility(){
        gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(3f);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void EnterOverloadMode() {
        _speed = Constants.OverloadSpeed;
        destructible = false;
        lastPos = transform.position;
        CancelInvoke("ExitOverloadMode");
        overLoadField.Play();
        ToolClass.ChangeCameraFov(Camera.main,Constants.OverloadFov,1);
    }

    public void ExitOverloadMode() { 
        _speed = Constants.PlayerSpeed;
        destructible = true;
        overLoadField.Stop();
        CancelInvoke("EnterOverloadMode");
        ToolClass.ChangeCameraFov(Camera.main,battleMgr.DefaultFov,0.2f);
    }

    public void EnterIdleState(bool isActive) {
        gameObject.SetActive(isActive);
        isMove = false;
        lastPos = transform.position;
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
