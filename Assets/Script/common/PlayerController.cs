using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    public FloatingJoystick joystick;

    private float m_speed = 0;
    public float Speed {
        get { return m_speed; }
        set { m_speed = value; }
    }

    private float m_rotate_speed = 0;

    private bool m_is_move = false;
    public bool IsMove {
        get { return m_is_move; }
        set { m_is_move = value; }
    }

    Vector3 dir = Vector3.zero;

    public Vector3 Dir {
        get { return dir; }
        set { dir = value; }
    }

    public Laser laser;
    public Rigidbody rb;

    private Vector3 pos;
    public Vector3 Pos {
        get { return pos; }
         
        set { pos = value; }
    }
    public Transform camTrans;
    private Vector3 camOffset;
    public BattleMgr battleMgr;

    private SoundPlayer soundPlayer;
    public SoundPlayer effectAudioPlayer;

    private float lastCollisionTime = 0f;
    private float collisionThresholdTime = 0.01f; // 时间阈值，单位秒

    public void Init() {
        laser = GameObject.FindGameObjectWithTag("GuideLine").GetComponent<Laser>();
        laser.gameObject.SetActive(false);
        laser.player = gameObject;
        rb = GetComponentInChildren<Rigidbody>();
        rb.maxAngularVelocity = 30;
        pos = transform.position;
        soundPlayer = GetComponent<SoundPlayer>();
        joystick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<FloatingJoystick>();
        joystick.gameObject.SetActive(true);
        joystick.SetIsShow(GameRoot.Instance.gameSettings.showJoyStick);
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }

    void Update() {
        if (GameRoot.Instance.gamePause) {
            return; 
        }
        if (joystick.IsDown) {
            if (!GameRoot.Instance.gameStart) {
                GameRoot.Instance.gameStart = true;
            }
            Time.timeScale = 0.2f;
            m_speed = Constants.PlayerNormalSpeed;
            m_rotate_speed = Constants.NormalRotateSpeed;
            laser.gameObject.SetActive(true);
        } else {
            laser.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        MakeGuideLine(); 
        if (m_is_move && battleMgr.startBattle) { 
            SetMove();
            SetRotate();
            SetCam();
        }
        if (Input.GetMouseButtonUp(0)){ 
            if (joystick.UpDirection != Vector3.zero) {
                Quaternion rotation = Quaternion.Euler(0, -45, 0);
                dir = rotation * joystick.UpDirection;
            }
            m_speed = Constants.PlayerNormalSpeed;
            m_rotate_speed = Constants.NormalRotateSpeed; 
            m_is_move = true;
            pos = transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (Time.time - lastCollisionTime < collisionThresholdTime) { 
            return;
        }
        lastCollisionTime = Time.time;

        if (battleMgr.startBattle && collision.gameObject.layer == 7) {//bullet 
            battleMgr.EndBattle(false);
            effectAudioPlayer.clipSource = Resources.Load<AudioClip>(Constants.DeadClip);
            effectAudioPlayer.PlaySound();
            Destroy(gameObject);
        }else if (collision.gameObject.layer == 6) {//enemy
            battleMgr.EliminateEnemy();
        }else {
            soundPlayer.clipSource = Resources.Load<AudioClip>(Constants.HitWallClip);
            soundPlayer.PlaySound();
        }
        Vector3 inDirection = (transform.position - pos).normalized;
        Vector3 inNormal = collision.contacts[0].normal; 
        if (dir == Vector3.zero) {
            Debug.Log(collision.collider.tag);
            dir = -dir;
        } else {
            dir = Vector3.Reflect(inDirection, inNormal);
        }
        if (dir.y != 0) {
            dir.y = 0;
        }
    }
    private void OnCollisionExit(Collision collision) {
        pos = transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) { //pickupitem
            if (other.transform.tag == "coin") {
                battleMgr.EarnCoin(other.gameObject.GetComponent<Coin>().coinValue);
            }
        }
    }

    private void SetMove() {
        transform.position = Vector3.MoveTowards(transform.position, 
            transform.position + dir.normalized, m_speed * Time.deltaTime);
    }
    private void SetRotate() {
        Vector3 rotationAxis = new Vector3(dir.z, 0f, -dir.x).normalized;
        float rotationAmount = m_speed * m_rotate_speed;
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
        laser.setDir(direction);
    }
}
