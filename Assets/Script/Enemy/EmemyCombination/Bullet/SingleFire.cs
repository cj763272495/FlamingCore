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
        bullet = ResSvc.Instance.LoadPrefab("Prefab/Enemy/Bullet",true);
        PoolManager.Instance.InitPool(bullet,20);
        this.shootPoint = shootPoint;
    }

    public void Fire() {
        if(shootTimer > shootTime) {
            foreach(Transform point in shootPoint) {
                GameObject go = PoolManager.Instance.GetInstance<GameObject>(bullet);
                go.transform.position = point.position;
                go.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                go.GetComponent<NormalBullet>().shootDir = point.forward;
                //go.GetComponent<NormalBullet>().owner = transform;//防止子弹碰撞到自己
            }
            shootTimer = 0;
            countDown.fillAmount = 0;
        }
        shootTimer += Time.deltaTime;
        countDown.fillAmount = shootTimer / shootTime;
    }
}
