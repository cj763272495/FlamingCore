using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {  
    private float speed;
    private float rotateSpeed;
    public bool isMove;
    Vector3 dir;

    public Vector3 Dir {
        get { return dir; }
    }
     
    public Rigidbody rb;
    public Vector3 lastPos;

    public Transform camTrans; 

    public BattleMgr battleMgr;
    public bool destructible=true;

    public void Init() {
        rb = GetComponentInChildren<Rigidbody>();
        rb.maxAngularVelocity = 30;
        lastPos = transform.position;
        camTrans = Camera.main.transform;
        destructible = true;
        speed = Constants.PlayerSpeed;
        rotateSpeed = Constants.RotateSpeed;
        isMove = true;
    }

    void Update() {
        if(battleMgr.StartBattle && isMove) { 
            SetMove();
            SetRotate();
            SetCam();
        }
    }
    
    public void SetDir(Vector3 direnction) {
        dir = direnction;
    }

    private void OnCollisionEnter(Collision collision) {
        int collisionLayer = collision.gameObject.layer;
        ContactPoint contactPoint = collision.contacts[0];

        if(destructible && collisionLayer == 7) {//bullet 
            battleMgr.EndBattle(false,contactPoint.point); 
            Destroy(gameObject);
        } else if(collisionLayer == 6) {//enemy
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
        if(tempDir!=Vector3.zero && tempDir.y==0) {
            dir = Vector3.Reflect(inDirection,inNormal).normalized;
        }
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

    private void SetMove() {
        transform.position = Vector3.MoveTowards(transform.position, 
            transform.position + dir.normalized, speed * Time.deltaTime);
    }
    private void SetRotate() {
        Vector3 rotationAxis = new Vector3(dir.z, 0f, -dir.x).normalized;
        float rotationAmount = speed * rotateSpeed;
        Vector3 angularVelocity = rotationAxis * rotationAmount;
        rb.angularVelocity = angularVelocity;
    }

    public void SetCam() {
        if (camTrans != null) {
            camTrans.position = transform.position - battleMgr.camOriginOffset;
        }
    }

    public void Revive() {
        StartCoroutine(ReviveIncincibility());
    }

    void OnDestroy() {
        StopAllCoroutines();
    }

    IEnumerator ReviveIncincibility(){
        gameObject.layer = LayerMask.NameToLayer("Player");
        yield return new WaitForSeconds(3f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
