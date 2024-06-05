using System.Collections; 
using System.Linq;
using UnityEngine;

public class Boom : MonoBehaviour
{ 
    private bool _isCrashed=false;//判断是否已经被玩家碰撞
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
            //炸弹开始闪光
            StartCoroutine(Flashing());
            StartCoroutine(StartMove());
        }
        if(_isCrashed && (layer==6 || layer==14 || layer==10)) {//碰到敌人或者墙体发生爆炸
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
        float baseIntensity = 1.0f; // 基础发光强度
        float flashFrequency = 2.0f; // 闪烁频率
        float flashAmplitude = 0.5f; // 闪烁振幅

        while(true) {
            float emissionIntensity = baseIntensity + Mathf.Sin(Time.time * flashFrequency) * flashAmplitude;
            mat.SetColor("_EmissionColor", Color.yellow * emissionIntensity);
            yield return null;
        }
    }


    void DestroyObjectsInRadius() {
        var objects = Physics.OverlapSphere(transform.position,attackRange).Select(collider => collider.gameObject);
        foreach(var obj in objects) {
            if(obj.layer == 14) {//摧毁可被摧毁的敌人
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
