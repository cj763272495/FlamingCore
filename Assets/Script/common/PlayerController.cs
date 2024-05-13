using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {  
    protected float _speed;
    private float _rotateSpeed;
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

    public virtual void Init() {
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
            SetRotate();
        }
    }

    protected virtual void Update() {
        //if(battleMgr.StartBattle && isMove) {
        //    SetMove();
        //    SetCam();
        //    SetRotate();
        //}
    }

    public void SetDir(Vector3 direnction) {
        _dir = direnction;
    }

    public virtual void OnPointerDown() {
        
    }

    public virtual void OnPointerUp() {
        ExitOverloadMode();
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        int collisionLayer = collision.gameObject.layer;
        ContactPoint contactPoint = collision.contacts[0];

        if(destructible && collisionLayer == 7) {//bullet 
            battleMgr.EndBattle(false,contactPoint.point); 
            Destroy(gameObject);
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
        //if(tempDir!=Vector3.zero && tempDir.y==0) {
            if(!Physics.Raycast(transform.position,tempDir,0.5f)) {
                _dir = tempDir;
            } else {
                // 如果射线检测到物体，那么调整反射方向
                _dir = Vector3.Reflect(tempDir,inNormal).normalized;
            }
        //}
        lastPos = transform.position;//更新上一次位置，用于计算反射方向
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
        _rb.MovePosition(transform.position + _dir * _speed * Time.fixedDeltaTime);


    }
    private void SetRotate() {
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
        Invoke("ExitOverloadMode",Constants.overloadDuration);//2秒后退出过载模式
    }

    public void ExitOverloadMode() { 
        _speed = Constants.PlayerSpeed;
        destructible = true;
    }

    public void EnterIdleState() {
        gameObject.SetActive(false);
        isMove = false;
        lastPos = transform.position;
    }
    public void ExitIdleState() {
        isMove = true;
        gameObject.SetActive(true);
    }
}
