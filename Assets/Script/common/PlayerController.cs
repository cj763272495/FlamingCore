using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    public FloatingJoystick joystick;

    private float speed = 0;
    private float rotateSpeed = 0;
    private bool isMove = false;
    Vector3 dir = Vector3.zero;

    public Vector3 Dir {
        get { return dir; }
    }

    public Laser guideLine;
    public Rigidbody rb;
    private Vector3 pos;

    public Transform camTrans;
    private Vector3 camOffset;
    private Vector3 camOriginOffset;

    public BattleMgr battleMgr;
    public bool destructible=true;

    //public float shakeAmount = 0.01f;
    //public float shakeDuration = 0.01f;
    //private Coroutine shakeCoroutine;

    public void Init() {
        guideLine.gameObject.SetActive(false);
        guideLine.player = gameObject;
        joystick.gameObject.SetActive(true);
        joystick.SetIsShow(GameRoot.Instance.gameSettings.showJoyStick);
        joystick.OnPointerDownAction = OnPointerDown;
        joystick.OnPointerUpAction = OnPointerUp;

        rb = GetComponentInChildren<Rigidbody>();
        rb.maxAngularVelocity = 30;
        pos = transform.position;
        camTrans = Camera.main.transform;
        camOriginOffset = transform.position - camTrans.position;
        camOffset = camOriginOffset;
        destructible = true;
    }

    void Update() {
        MakeGuideLine();
        if (isMove && battleMgr.StartBattle) { 
            SetMove();
            SetRotate();
            SetCam();
        }
    }

    public void OnPointerDown() {
        if(!battleMgr.StartBattle) {
            battleMgr.SetBattleStateStart();
        }
        Time.timeScale = 0.1f;
        speed = Constants.PlayerNormalSpeed;
        rotateSpeed = Constants.NormalRotateSpeed;
        guideLine.gameObject.SetActive(true);
    }

    public void OnPointerUp() {
        // 在这里处理鼠标或触摸输入
        guideLine.gameObject.SetActive(false);
        if(battleMgr.StartBattle) {
            Time.timeScale = 1;
        }

        if(joystick.UpDirection != Vector3.zero) {
            Quaternion rotation = Quaternion.Euler(0,-45,0);
            dir = (rotation * joystick.UpDirection).normalized;
        }
        speed = Constants.PlayerNormalSpeed;
        rotateSpeed = Constants.NormalRotateSpeed;
        isMove = true;
        pos = transform.position;
    }



    private void OnCollisionEnter(Collision collision) {
        int collisionLayer = collision.gameObject.layer;
        ContactPoint contactPoint = collision.contacts[0];

        if(destructible && collisionLayer == 7) {//bullet 
            battleMgr.EndBattle(false);
            ParticleMgr.Instance.PlayDeadParticle(contactPoint.point);
            AudioManager.Instance.PlaySound(ResSvc.Instance.LoadAudio(Constants.DeadClip));
            Destroy(gameObject);
        } else if(collisionLayer == 6) {//enemy
            battleMgr.EliminateEnemy();
        } else {
            battleMgr.particleMgr.PlayHitWallParticle(contactPoint);
            AudioClip clip = ResSvc.Instance.LoadAudio(Constants.HitWallClip,true);
            AudioManager.Instance.PlaySound(clip);
        }
        //TriggerShakeCam();
        Vector3 inDirection = (transform.position - pos).normalized;
        Vector3 inNormal = contactPoint.normal;
        inNormal.y = 0;
        Vector3 tempDir = Vector3.Reflect(inDirection,inNormal).normalized;
        if(tempDir!=Vector3.zero && tempDir.y==0) {
            dir = Vector3.Reflect(inDirection,inNormal).normalized;
        }
        pos = transform.position;
    }


    //#region 相机抖动
    //public void TriggerShakeCam() {
    //    if(shakeCoroutine != null) { 
    //        StopShake();
    //    } 
    //    shakeCoroutine = StartCoroutine(ShakeCamera());
    //}

    //IEnumerator ShakeCamera() {
    //    float timer = 0.0f;
    //    while(timer < shakeDuration) {
    //        Vector3 randomShakeOffset = Random.insideUnitSphere * shakeAmount;
    //        camOffset += randomShakeOffset * 0.5f;
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //    shakeCoroutine = null;
    //    camOffset = camOriginOffset;
    //}

    //void StopShake() {
    //    if(shakeCoroutine != null) {
    //        StopCoroutine(shakeCoroutine);
    //        shakeCoroutine = null;
    //    }
    //}
    //#endregion

    //private void OnCollisionExit(Collision collision) {
    //    pos = transform.position;
    //}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) { //pickupitem
            if (other.transform.CompareTag("coin")) {
                battleMgr.EarnCoin(other.gameObject.GetComponent<Coin>().CoinValue);
            }
        }
    }

    private void SetMove() {
        transform.position = Vector3.MoveTowards(transform.position, 
            transform.position + dir.normalized, speed * Time.deltaTime);
    }
    private void SetRotate() {
        Vector3 rotationAxis = new Vector3(dir.z, 0f, -dir.x).normalized;
        float rotationAmount = speed * rotateSpeed;
        Vector3 angularVelocity = rotationAxis * rotationAmount;
        rb.angularVelocity = angularVelocity;
    }

    public void SetCam() {
        if (camTrans != null) {
            camTrans.position = transform.position - camOffset;
        }
    }

    private void MakeGuideLine() {
        Vector3 direction = (Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal).normalized;
        Quaternion rotation = Quaternion.Euler(0, -45, 0);
        // 旋转向量
        direction = rotation * direction;
        if (direction == Vector3.zero) {
            direction = dir;
        }
        guideLine.SetDir(direction);
    }

    void OnDestroy() {
        StopAllCoroutines();
    }

    public void Revive() {
        StartCoroutine(ReviveIncincibility());
    }

    IEnumerator ReviveIncincibility(){
        gameObject.layer = LayerMask.NameToLayer("Player");
        yield return new WaitForSeconds(3f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
