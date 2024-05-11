using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashBall:PlayerController {

    // 闪现最大距离
    public float _flashDistance = 5f;
    public Image _targetPosImg;
    private float _targetPosImgRadiu = 0.75f + 0.1f;
    private bool _showTargetPos = false;
    public Transform canvasTrans;
    public LayerMask collisionLayer; // 用于射线检测的层
    public GameObject dashParticle;

    public override void Init() {
        base.Init();
        _targetPosImg.gameObject.SetActive(false);
        _targetPosImg.transform.position = transform.position;
        ParticleMgr.Instance.AddCustomParticle(dashParticle);
    }

    override protected void Update() {
        base.Update();
        canvasTrans.rotation = Quaternion.Euler(90,0,0);
    }

    public override void OnPointerDown() { 
        //显示闪现落地的位置
        base.OnPointerDown();
        _showTargetPos = true; 
        _targetPosImg.transform.position = transform.position;
        _targetPosImg.gameObject.SetActive(true);
        StartCoroutine(MakeTargetPosImgCoroutine());
    }

    private IEnumerator MakeTargetPosImgCoroutine() {
        while(_showTargetPos) {
            MakeTargetPosImg();
            yield return null;
        }
    }

    private void MakeTargetPosImg() {
        Vector3 direction = battleMgr.joyStickDir==Vector3.zero? _dir:battleMgr.joyStickDir;
        Ray ray = new Ray(transform.position,direction);
        float magnitude = battleMgr.joystick.Input.magnitude; 

        if(Physics.Raycast(ray,out RaycastHit hit,_flashDistance,collisionLayer)) {
            _targetPosImg.transform.position = hit.point - direction * _targetPosImgRadiu; 
        } else {
            _targetPosImg.transform.position = transform.position + direction * _flashDistance * magnitude;
        }
    }

    public override void OnPointerUp() {
        base.OnPointerUp();
        ParticleMgr.Instance.PlayCustomParticle(dashParticle,transform.position);
        //闪现到落地的位置
        transform.position = new Vector3(_targetPosImg.transform.position.x,transform.position.y,_targetPosImg.transform.position.z);
        _showTargetPos = false;
        _targetPosImg.gameObject.SetActive(false);
    }
} 