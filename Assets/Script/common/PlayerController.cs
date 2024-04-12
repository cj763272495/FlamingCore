using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    public FloatingJoystick joystick;
    public GameObject vmcamera;
    public GameObject CameraObj;
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


    private SoundPlayer soundPlayer;

    public void Start() {
        ResSvc res =  ResSvc.Instance;
        laser.SetEnable(false);
        rb = transform.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 30;
        pos = transform.position;
        soundPlayer = GetComponent<SoundPlayer>();
        GameRoot.Instance.battleWnd.joystick = joystick;
        joystick.gameObject.SetActive(true);
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
            laser.SetEnable(true);
        } else {
            laser.SetEnable(false);
            Time.timeScale = 1;
        }

        MakeGuideLine(); 
        if (m_is_move) { 
            SetMove();
            SetRotate();
            SetCam();
        }
        if (Input.GetMouseButtonUp(0)){ 
            dir = joystick.UpDirection == Vector3.zero? dir : joystick.UpDirection;
            m_speed = Constants.PlayerNormalSpeed;
            m_rotate_speed = Constants.NormalRotateSpeed; 
            m_is_move = true;
            pos = transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 7) {//bullet
            gameObject.SetActive(false); 
            GameRoot.Instance.GameOver();
        }else if (collision.gameObject.layer == 6) {//enemy

        }else if (collision.gameObject.layer == 9) { //pickupitem
            if (collision.transform.tag=="coin") {
                GameRoot.Instance.battleMgr.EarnCoin(collision.gameObject.GetComponent<Coin>().coinValue);
            }
        } else {
            if (joystick.IsDown) {
                soundPlayer.clipSource = Resources.Load<AudioClip>(Constants.HitWallSlowlyClip);
            } else { 
                soundPlayer.clipSource = Resources.Load<AudioClip>(Constants.HitWallClip);
            }
            soundPlayer.PlaySound();
        }
        Vector3 inDirection = (transform.position - pos).normalized;
        Vector3 inNormal = collision.contacts[0].normal;
        dir = Vector3.Reflect(inDirection, inNormal);
        if (dir.y != 0) {
            dir.y = 0;
        }
        pos = transform.position;
    }
     
    private void SetMove() {
        transform.position = Vector3.MoveTowards(transform.position, 
            transform.position + dir, m_speed * Time.deltaTime);
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
        if (direction == Vector3.zero) {
            direction = dir;
        }
        laser.setDir(direction);
    }
}
