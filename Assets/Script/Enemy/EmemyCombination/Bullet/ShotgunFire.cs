using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunFire : IFireMode {
    private readonly float shootTime = 3;
    private float shootTimer;
    public GameObject bullet;
    public Image countDown;
    private List<Transform> shootPoint;
    
    private int bulletCount = 3;  // 默认同时发射3发
    private float _spreadAngle = 45f;  // 散射角度

    public ShotgunFire(List<Transform> shootPoint, int bulletNum,float spreadAngle) {
        bullet = ResSvc.Instance.LoadPrefab("Prefab/Enemy/Bullet",true);
        PoolManager.Instance.InitPool(bullet,20,BattleSys.Instance.battleMgr.transform);
        this.shootPoint = shootPoint;
        bulletCount = bulletNum;
        _spreadAngle = spreadAngle;
    }

    public void Fire() {
        if(shootPoint==null) {
            return;
        }
        if(shootTimer > shootTime) {
            for(int i = 0; i < bulletCount; i++) {
                foreach(Transform point in shootPoint) {
                    GameObject go = PoolManager.Instance.GetInstance<GameObject>(bullet);
                    go.transform.position = point.position;
                    go.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                    float offsetAngle = (i - bulletCount / 2f) / (bulletCount - 1) * _spreadAngle;
                    go.GetComponent<NormalBullet>().SetBulletShotDir(Quaternion.Euler(0,offsetAngle,0) * point.forward);
                }
            }
            shootTimer = 0;
            if(countDown) {
                countDown.fillAmount = 0;
            }
        }
        shootTimer += Time.deltaTime;
        if(countDown) {
            countDown.fillAmount = shootTimer / shootTime;
        }
    }
} 
