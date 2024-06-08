using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TripleFire: IFireMode {
    private readonly float shootTime = 3;
    private float shootTimer; 
    private float bulletTimer;
    private readonly float bulletInterval = 0.2f; 
    public Image countDown;
    public GameObject bullet; 
    private int bulletsFired=0; 
    private List<Transform> shootPoints;

    public TripleFire(List<Transform> shootPoints) {
        bullet = ResSvc.Instance.LoadPrefab("Prefab/Enemy/Bullet",true);
        PoolManager.Instance.InitPool(bullet,20,BattleSys.Instance.battleMgr.transform);
        this.shootPoints = shootPoints;
    }

    public void Fire() {
        if(shootPoints == null || shootPoints.Count == 0)
            return;
        if(bulletsFired >= 3) {
            shootTimer += Time.deltaTime;
            if(shootTimer > shootTime) {
                bulletsFired = 0;
                shootTimer = 0;
                if(countDown) {
                    countDown.fillAmount = 0;
                }
                return;
            }
            if(countDown) {
                countDown.fillAmount = shootTimer / shootTime;
            }
        } else {
            bulletTimer += Time.deltaTime;
            if(bulletTimer > bulletInterval) {
                foreach(Transform shootPoint in shootPoints) {
                    GameObject go = PoolManager.Instance.GetInstance<GameObject>(bullet,BattleSys.Instance.battleMgr.transform);
                    go.transform.position = shootPoint.position;
                    go.transform.localScale = Vector3.one;
                    go.GetComponent<NormalBullet>().SetBulletShot(shootPoint.forward);
                    go.GetComponent<NormalBullet>().owner = shootPoint.parent.parent;
                    bulletsFired++;
                    bulletTimer = 0; 
                }
            }
        }
    }
}
