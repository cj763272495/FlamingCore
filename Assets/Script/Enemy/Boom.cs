using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    //private bool _showGuideLine;
    private bool _isCrashed=false;//�ж��Ƿ��Ѿ��������ײ
    private Vector3 _dir;

    public GuideLine guideLine;
    public float attackRange=3;
    public GameObject rangeShow;
    public Rigidbody rb;
    //�����ٶ�
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
        if(_isCrashed && (layer==6 || layer==10)) {//�������˻���ǽ�巢����ը
            //Boom particle 
            exploreTrigger.enabled = true;
            _isCrashed = false;
            //�ȴ���ը������������
            Destroy(gameObject,0.5f);
        }
    }

    private IEnumerator StartMove() {
        while(_isCrashed) {
            rb.MovePosition(transform.position + _dir * 5 * Time.deltaTime);
            //model.transform.Rotate(Vector3.right,Time.deltaTime * 2000);
            //rb.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles + Vector3.right * Time.deltaTime * 2000));
            //transform.Rotate(transform.right * Time.deltaTime * 900,Space.World);
            // ������ת�����ת�Ƕ�
            Vector3 rotationAxis = Vector3.Cross(_dir,Vector3.up);
            float rotationAmount = _speed * 100 * Time.deltaTime; 
            // ��ת����
            transform.Rotate(rotationAxis,-rotationAmount,Space.World);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {//��ը��Χ��⵽����������
        if(other.gameObject.layer == 6) {
            //������ײ������
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
