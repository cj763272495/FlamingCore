using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunFire : IFireMode {
    private readonly float shootTime = 3;
    private float shootTimer;
    public GameObject bullet;
    public Image countDown;
    private List<Transform> shootPoints;
    
    private int bulletCount = 3;  // 默认同时发射3发
    private float _spreadAngle = 45f;  // 散射角度

    public ShotgunFire(List<Transform> shootPoint, int bulletNum,float spreadAngle) {
        bullet = ResSvc.Instance.LoadPrefab("Prefab/Enemy/Bullet",true);
        PoolManager.Instance.InitPool(bullet,20,BattleSys.Instance.battleMgr.transform);
        shootPoints = shootPoint;
        bulletCount = bulletNum;
        _spreadAngle = spreadAngle;
    }

    public void Fire() {
        if(shootPoints==null || shootPoints.Count==0) {
            return;
        }
        if(shootTimer > shootTime) {
            for(int i = 0; i < bulletCount; i++) {
                foreach(Transform point in shootPoints) {
                    GameObject go = PoolManager.Instance.GetInstance<GameObject>(bullet,BattleSys.Instance.battleMgr.transform);
                    go.transform.position = point.position;
                    go.transform.localScale = Vector3.one;
                    float offsetAngle = (i - bulletCount / 2f) / (bulletCount - 1) * _spreadAngle;
                    go.GetComponent<NormalBullet>().SetBulletShot(Quaternion.Euler(0,offsetAngle,0) * point.forward);
                    go.GetComponent<NormalBullet>().owner = point.parent.parent;
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
