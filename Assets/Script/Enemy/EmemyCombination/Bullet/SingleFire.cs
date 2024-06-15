using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleFire : IFireMode
{
    private readonly float shootTime = 3;
    private float shootTimer;
    public GameObject bullet;
    public Image countDown;
    private List<Transform> shootPoint;

    public SingleFire(List<Transform> shootPoint) {
        bullet = Resources.Load<GameObject>("Prefab/Enemy/Bullet");
        PoolManager.Instance.InitPool(bullet,20,BattleSys.Instance.battleMgr.transform);
        this.shootPoint = shootPoint;
    }

    public void Fire() {
        if(shootPoint == null || shootPoint.Count == 0)
            return;
        if(shootTimer > shootTime) {
            foreach(Transform point in shootPoint) {
                GameObject go = PoolManager.Instance.GetInstance<GameObject>(bullet,BattleSys.Instance.battleMgr.transform);
                go.transform.position = point.position;
                go.transform.localScale = Vector3.one;
                go.GetComponent<NormalBullet>().SetBulletShot(point.forward);
                go.GetComponent<NormalBullet>().owner = point.parent.parent;
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
