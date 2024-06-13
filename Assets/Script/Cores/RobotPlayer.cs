using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;

public class RobotPlayer: PlayerController { 
    private List<GameObject> enemiesInRange = new List<GameObject>();
    public float fireRate = 0.2f; // 子弹发射的频率，单位是秒
    public float bulletSpeed = 50; // 子弹发射的频率，单位是秒
    private bool canShoot = true;
    public GameObject Bullet;

    public GameObject[] shootPoints;
    public float rotateSpeed = 100;

    public GameObject arrow;
    public float moveSpeed = 6;
    public AudioClip shotAudioClip;

    public int magazineCapacity = 8;
    public float curBulletsNum; 
    private Coroutine ReloadCoroutine;
    public Image bulletCountCircle;

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
        if(bulletCountCircle) {
            bulletCountCircle.gameObject.SetActive(true);
        }
        curBulletsNum = magazineCapacity; 
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
        if (closestEnemy) { 
            float minDistance = Vector3.Distance(transform.position,closestEnemy.transform.position);
            foreach(GameObject enemy in enemiesInRange) {
                float distance = Vector3.Distance(transform.position,enemy.transform.position);
                if(distance < minDistance) {
                    closestEnemy = enemy;
                    minDistance = distance;
                }
            } 
            Vector3 direction = closestEnemy.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,rotateSpeed * Time.deltaTime); 
            if(Vector3.Dot(transform.forward,direction.normalized) > 0.9f) {
                // 物体面向敌人，开始射击
                Shot();
            }
        }
    }

    public override void OnPointerDown() {
        arrow.gameObject.SetActive(true);
    }

    public override void OnDrag() { 
        arrow.gameObject.SetActive(true);
        Vector3 dir = Vector3.forward * battleMgr.joystick.Vertical + Vector3.right * battleMgr.joystick.Horizontal;
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
            other.gameObject.GetComponent<EnemyEntity>().OnEnemyDestroyed += OnEnemyKilled;
        }
    }

    private void OnEnemyKilled(GameObject enemy) {
        if(!enemiesInRange.Remove(enemy)) {
            ToolClass.PrintLog("Remove enemy failed");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 14) {
            other.gameObject.GetComponent<EnemyEntity>().OnEnemyDestroyed -= OnEnemyKilled;
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Shot() {
        if(canShoot && curBulletsNum > 0) {
            foreach(GameObject shootPoint in shootPoints) {
                GameObject go = PoolManager.Instance.GetInstance<GameObject>(Bullet,BattleSys.Instance.battleMgr.transform);
                if(go != null) {
                    NormalBullet bullet = go.GetComponent<NormalBullet>();
                    bullet.owner = transform;
                    bullet.transform.position = shootPoint.transform.position;
                    bullet.SetBulletSpeed(bulletSpeed);
                    Vector3 shotDir = shootPoint.transform.forward;
                    bullet.SetBulletShot(shotDir);
                    AudioManager.Instance.PlaySound(shotAudioClip);
                    curBulletsNum--;
                    if(bulletCountCircle) {
                        bulletCountCircle.fillAmount = curBulletsNum / magazineCapacity;
                    }
                    if(curBulletsNum==0) {
                        ReloadCoroutine = StartCoroutine(ReloadAmmo());
                    }
                } else {
                    ToolClass.PrintLog("get Bullet failed");
                    return;
                }
            }
            StartCoroutine(ShootDelay());
        }
    }

    private IEnumerator ReloadAmmo() {
        while(curBulletsNum < magazineCapacity) {
            // 等待一段时间后增加子弹
            yield return new WaitForSeconds(1f);
            curBulletsNum++;
            if(bulletCountCircle) {
                bulletCountCircle.fillAmount = curBulletsNum / magazineCapacity;
            }
            if(curBulletsNum >= magazineCapacity) { 
                StopCoroutine(ReloadCoroutine); // 停止协程
            }
        }

    }

    private IEnumerator ShootDelay() {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    protected override void OnCollisionEnter(Collision collision) {
        int collisionLayer = collision.gameObject.layer; 

        if(destructible && collisionLayer == 7) {//bullet 
            PlayerDead();
        }
    }
}
