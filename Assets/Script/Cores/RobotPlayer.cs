using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlayer: PlayerController { 
    private List<GameObject> enemiesInRange = new List<GameObject>();
    public float fireRate = 0.1f; // 子弹发射的频率，单位是秒
    public float bulletSpeed = 50; // 子弹发射的频率，单位是秒
    private bool canShoot = true;
    public GameObject Bullet;

    public GameObject[] shootPoints;
    public float rotateSpeed = 100;

    public GameObject arrow;
    public SpriteRenderer countCircle;
    public float moveSpeed = 6;

    public override void Init(BattleMgr battle,StateMgr state) {
        battleMgr = battle;
        stateMgr = state;
        camTrans = Camera.main.transform;
        destructible = true;
        _speed = moveSpeed;
        isMove = false;
        PoolManager.Instance.InitPool(Bullet, 10,battleMgr.transform);
        Born();
        arrow.gameObject.SetActive(false);
        countCircle.gameObject.SetActive(true);
    } 

    protected override void FixedUpdate() {
        if(battleMgr.StartBattle && isMove) {
            SetMove();
            SetCam();
            if(enemiesInRange.Count > 0) {
                SetRotateAndShot();
            }
        }
    }

    protected override void SetRotateAndShot() {
        // 找到距离最近的敌人
        GameObject closestEnemy = enemiesInRange[0];
        if(enemiesInRange.Count>1) {
            float minDistance = Vector3.Distance(transform.position,closestEnemy.transform.position);
            foreach(GameObject enemy in enemiesInRange) {
                float distance = Vector3.Distance(transform.position,enemy.transform.position);
                if(distance < minDistance) {
                    closestEnemy = enemy;
                    minDistance = distance;
                }
            }
        }
        Vector3 direction = closestEnemy.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction); 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        // 检查物体是否面向敌人
        if(Vector3.Dot(transform.forward,direction.normalized) > 0.9f) {
            // 物体面向敌人，允许射击
            Shot();
        } 
    }

    public override void OnPointerDown() {
        arrow.gameObject.SetActive(true);
    }

    public override void OnDrag() {
        arrow.gameObject.SetActive(true);
        Vector3 dir = (Vector3.forward * battleMgr.joystick.Vertical + Vector3.right * battleMgr.joystick.Horizontal).normalized;
        SetDir(dir); 
        arrow.transform.LookAt(arrow.transform.position + dir); 
        arrow.transform.position = transform.position + dir * 2.5f;
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
                GameObject go = PoolManager.Instance.GetInstance<GameObject>(Bullet);
                if(go != null) {
                    NormalBullet bullet = go.GetComponent<NormalBullet>();
                    bullet.owner = transform;
                    bullet.transform.position = shootPoint.transform.position;
                    bullet.SetBulletSpeed(bulletSpeed);
                    Vector3 shotDir = shootPoint.transform.forward;
                    bullet.SetBulletShotDir(shotDir);
                } else {
                    Debug.LogError("get Bullet failed");
                    return;
                }
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
