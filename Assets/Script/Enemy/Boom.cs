using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    //private bool _showGuideLine;
    private bool _isCrashed=false;//判断是否已经被玩家碰撞
    private Vector3 _dir;

    public GuideLine guideLine;
    public float attackRange=3;
    public GameObject rangeShow;
    public Rigidbody rb;
    //滚动速度
    public float _speed = 5; 


    public CapsuleCollider exploreTrigger;

    private void Start() { 
        guideLine.gameObject.SetActive(false);
        exploreTrigger.enabled = false;
    } 

    public void ShowGudieLine(Vector3 dir ) {
        guideLine.SetLen(attackRange);
        guideLine.SetDir(transform,dir);
        guideLine.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision) {
        int layer = collision.gameObject.layer; 
        if(layer == 8) {
            _isCrashed = true;
            rangeShow.SetActive(false);
            _dir = new Vector3(collision.contacts[0].normal.x,transform.position.y,collision.contacts[0].normal.z);
            transform.LookAt(_dir);
            StartCoroutine(StartMove());
        }
        if(_isCrashed && (layer==6 || layer==10)) {//碰到敌人或者墙体发生爆炸
            //Boom particle 
            exploreTrigger.enabled = true;
            _isCrashed = false;
            //等待爆炸结束销毁自身
            Destroy(gameObject,0.5f);
        }
    }

    private IEnumerator StartMove() {
        while(_isCrashed) {
            rb.MovePosition(transform.position + _dir * 5 * Time.deltaTime);
            //model.transform.Rotate(Vector3.right,Time.deltaTime * 2000);
            //rb.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles + Vector3.right * Time.deltaTime * 2000));
            //transform.Rotate(transform.right * Time.deltaTime * 900,Space.World);
            // 计算旋转轴和旋转角度
            Vector3 rotationAxis = Vector3.Cross(_dir,Vector3.up);
            float rotationAmount = _speed * 100 * Time.deltaTime; 
            // 旋转物体
            transform.Rotate(rotationAxis,-rotationAmount,Space.World);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {//爆炸范围检测到东西就销毁
        if(other.gameObject.layer == 6) {
            //销毁碰撞的物体
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
