using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripleTurret : Enemy
{
    private readonly float shootTime = 3;
    private float shootTimer;
    public Transform shootPoint;
    public Image countDown;

    private void Start() {
        rotateSpeed = 4;
    }
    public void OnPlayerLoaded() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update() {
        if(player) {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(player.transform.position - transform.position),
                rotateSpeed * Time.deltaTime);
            if(!canAttack) {
                return;
            }
            if(shootTimer > shootTime) {
                ShootTripleBullets();
                shootTimer = 0;
                countDown.fillAmount = 0;
            }
            shootTimer += Time.deltaTime;
            countDown.fillAmount = shootTimer / shootTime;
        }
    }

    private void ShootTripleBullets() {
        StartCoroutine(ShootBulletsCoroutine());
    }

    private IEnumerator ShootBulletsCoroutine() {
        for(int i = 0; i < 3; i++) {
            GameObject go = ResSvc.Instance.LoadPrefab("Prefab/Enemy/Bullet",true);
            go.transform.localPosition = shootPoint.position;
            go.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            go.GetComponent<NormalBullet>().shootDir = shootPoint.forward;
            go.GetComponent<NormalBullet>().owner = transform;

            yield return new WaitForSeconds(0.1f); // Delay between each bullet
        }
    }
}
