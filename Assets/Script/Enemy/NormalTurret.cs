using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalTurret : EnemyEntity {
    private readonly float shootTime = 2;
    private float shootTimer;
    public Transform shootPoint;
    public Image countDown;

    private void Start() {
        rotateSpeed = 4;
    }


    public override void Update() {
        if(player) {
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir),
                rotateSpeed * Time.deltaTime);
            if(!canAttack) {
                return;
            }
            if(shootTimer > shootTime) {
                GameObject go = ResSvc.Instance.LoadPrefab("Prefab/Enemy/Bullet",true);
                go.transform.localPosition = shootPoint.position;
                go.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                go.GetComponent<NormalBullet>().shootDir = shootPoint.forward;
                go.GetComponent<NormalBullet>().owner = transform;//防止子弹碰撞到自己
                shootTimer = 0;
                countDown.fillAmount = 0;
            }
            shootTimer += Time.deltaTime;
            countDown.fillAmount = shootTimer / shootTime;
        }
    }
}
