using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlayer: PlayerController { 
    private List<GameObject> enemiesInRange = new List<GameObject>();
    public float fireRate = 0.5f; // 子弹发射的频率，单位是秒
    private bool canShoot = true;
    public GameObject Bullet;

    public GameObject[] shootPoints;
    public float rotateSpeed = 100;

    public SpriteRenderer arrow;
    public SpriteRenderer countCircle;
    public float moveSpeed = 6;

    public override void Init(BattleMgr battle,StateMgr state) {
        battleMgr = battle;
        stateMgr = state;
        camTrans = Camera.main.transform;
        destructible = true;
        _speed = moveSpeed;
        isMove = false;
        PoolManager.Instance.InitPool(Bullet, 10);
        Born();
        arrow.gameObject.SetActive(false);
        countCircle.gameObject.SetActive(true);
    } 

    protected override void FixedUpdate() {
        if(battleMgr.StartBattle && isMove) {
            SetMove();
            SetCam();
            if(enemiesInRange.Count > 0) {
                SetRotate();
                Shot();
            }
        }
    }

    protected override void SetRotate() { 
        Vector3 direction = enemiesInRange[0].transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation,toRotation,Time.deltaTime * rotateSpeed); 
    }

    public override void OnPointerDown() {
        arrow.gameObject.SetActive(true);
    }

    public override void OnDrag() {
        arrow.gameObject.SetActive(true);
        Vector2 dir = (Vector3.forward * battleMgr.joystick.Vertical + Vector3.right * battleMgr.joystick.Horizontal).normalized;
        SetDir(dir);
        arrow.transform.right = new Vector3(-dir.x,0,-dir.y);
        arrow.transform.forward = Vector3.up;
        arrow.transform.position = transform.position + new Vector3(dir.x,0,dir.y) * 2.5f;
        Take();
    }

    public override void OnPointerUp() {
        isMove = false;
        Idle();
        arrow.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 14) {  
            enemiesInRange.Add(other.gameObject); 
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 14) {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Shot() {
        if(canShoot) {
            foreach(GameObject shootPoint in shootPoints) {
                GameObject bullet = PoolManager.Instance.GetInstance<GameObject>(Bullet);
                bullet.transform.forward = shootPoint.transform.forward;
            }
            StartCoroutine(ShootDelay());
        }
    } 

    private IEnumerator ShootDelay() {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    protected override void OnCollisionEnter(Collision collision) {
        int collisionLayer = collision.gameObject.layer;
        ContactPoint contactPoint = collision.contacts[0];

        if(destructible && collisionLayer == 7) {//bullet 
            PlayerDead();
        }// else if(collisionLayer == 14) {//enemy
        //    battleMgr.EliminateEnemy();
        //}  
    }
}
