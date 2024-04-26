using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalTurret : Enemy
{
    private readonly float shootTime = 2;
    private float shootTimer;
    public Transform shootPoint;
    public Image countDown;

    private void Start() {
        rotateSpeed = 4;
    }
    private void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(player.transform.position - transform.position),
            rotateSpeed * Time.deltaTime);
        if (shootTimer > shootTime) {
            GameObject go =  ResSvc.Instance.LoadPrefab("Prefab/Enemy/Bullet",true);
            go.transform.localPosition = shootPoint.position;
            go.transform.localScale = Vector3.one;
            go.transform.forward = shootPoint.transform.forward;
            shootTimer = 0;
            countDown.fillAmount = 0;
        }
        shootTimer += Time.deltaTime;
        countDown.fillAmount = shootTimer/shootTime;
    }
}
