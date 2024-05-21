using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashBall: PlayerController {

    // 闪现最大距离
    private float _flashDistance = 4f;
    public Image _targetPosImg;
    public Transform canvasTrans;
    public LayerMask imgCollisionLayer; // 用于射线检测的层
    public GameObject dashParticle;
     
    private float _targetPosImgRadiu;
    private bool _showTargetPos = false;

    public override void Init(BattleMgr battle ,StateMgr state) {
        base.Init(battle,state);
        _targetPosImg.gameObject.SetActive(false);
        _targetPosImg.transform.position = transform.position;
        ParticleMgr.Instance.AddCustomParticle(dashParticle);
        _targetPosImgRadiu = _targetPosImg.rectTransform.sizeDelta.x / 2;
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

        if(Physics.Raycast(ray,out RaycastHit hit,_flashDistance,imgCollisionLayer)) {
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