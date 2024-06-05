using System.Collections; 
using System.Linq;
using UnityEngine;

public class Boom : MonoBehaviour
{ 
    private bool _isCrashed=false;//�ж��Ƿ��Ѿ��������ײ
    private Vector3 _dir;

    public GuideLine guideLine;
    private float attackRange = 3.5f;
    public GameObject rangeShow;
    public Rigidbody rb; 
    private float _speed = 5;
    public ParticleSystem exploreParticle;

    public Material DangerRangeShowMat;
    public Material ModelOrgMat;

    private void Start() { 
        guideLine.gameObject.SetActive(false);
        ParticleMgr.Instance.AddCustomParticle(exploreParticle.gameObject);
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
            rangeShow.GetComponent<Renderer>().material = DangerRangeShowMat;
            _dir = new Vector3(collision.contacts[0].normal.x,transform.position.y,collision.contacts[0].normal.z);
            transform.LookAt(_dir);
            //ը����ʼ����
            StartCoroutine(Flashing());
            StartCoroutine(StartMove());
        }
        if(_isCrashed && (layer==6 || layer==14 || layer==10)) {//�������˻���ǽ�巢����ը
            StopAllCoroutines();
            rangeShow.SetActive(false);
            ParticleMgr.Instance.PlayCustomParticle(exploreParticle.gameObject, transform.position);
            _isCrashed = false;
            DestroyObjectsInRadius();
            Destroy(gameObject);
        }
    }

    IEnumerator Flashing() {
        Material mat = Instantiate(ModelOrgMat);
        ModelOrgMat = mat;
        float baseIntensity = 1.0f; // ��������ǿ��
        float flashFrequency = 2.0f; // ��˸Ƶ��
        float flashAmplitude = 0.5f; // ��˸���

        while(true) {
            float emissionIntensity = baseIntensity + Mathf.Sin(Time.time * flashFrequency) * flashAmplitude;
            mat.SetColor("_EmissionColor", Color.yellow * emissionIntensity);
            yield return null;
        }
    }


    void DestroyObjectsInRadius() {
        var objects = Physics.OverlapSphere(transform.position,attackRange).Select(collider => collider.gameObject);
        foreach(var obj in objects) {
            if(obj.layer == 14) {//�ݻٿɱ��ݻٵĵ���
                Destroy(obj);
            } else if(obj.layer == 8) {
                obj.GetComponent<PlayerController>().PlayerDead();
            }
        }
    }

    private IEnumerator StartMove() {
        float initialY = rangeShow.transform.position.y;
        while(_isCrashed) {
            rb.MovePosition(transform.position + _dir * 5 * Time.deltaTime);
            Vector3 rotationAxis = Vector3.Cross(_dir,Vector3.up);
            float rotationAmount = _speed * 100 * Time.deltaTime;
            transform.Rotate(rotationAxis,-rotationAmount,Space.World);
            rangeShow.transform.rotation = Quaternion.identity;
            rangeShow.transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
            yield return null;
        }
    }
}
