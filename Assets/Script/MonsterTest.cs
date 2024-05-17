using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : EnemyEntity
{ 
    public GameObject firePoint;

    public GameObject spawnedlaser;
    public float len=20;
    public LineRenderer lineRenderer;

    private void Start() {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0,Vector3.zero);
        
        EnableLaser();
    }

    public override void  Update() {
        ShootLaser();
    }
    void ShootLaser() {
        RaycastHit hit;
        if(Physics.Raycast(firePoint.transform.position,firePoint.transform.forward,out hit,len)) {
            lineRenderer.SetPosition(1,new Vector3(0,0,hit.distance));
        } else {
            lineRenderer.SetPosition(1,new Vector3(0,0,len));
        }
    }

    void EnableLaser() {
        spawnedlaser.SetActive(true);
    }

    void DisableLaser() {
        spawnedlaser.SetActive(false);
    }   
}
