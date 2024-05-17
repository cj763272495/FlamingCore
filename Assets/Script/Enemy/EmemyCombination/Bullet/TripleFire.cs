using System.Collections;
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
    private Transform shootPoint;

    public TripleFire(Transform shootPoint) {
        bullet = ResSvc.Instance.LoadPrefab("Prefab/Enemy/Bullet",true);
        PoolManager.Instance.InitPool(bullet,20);
        this.shootPoint = shootPoint;
    }

    public void Fire() {
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
                GameObject go = PoolManager.Instance.GetInstance<GameObject>(bullet);
                go.transform.position = shootPoint.position;
                go.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                go.GetComponent<NormalBullet>().shootDir = shootPoint.forward;
                //go.GetComponent<NormalBullet>().owner = transform;//��ֹ�ӵ���ײ���Լ�
                bulletsFired++;
                bulletTimer = 0;
            }
        }
    }
}
