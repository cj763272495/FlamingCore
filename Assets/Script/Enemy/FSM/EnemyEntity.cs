using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEntity:Entity {
    protected PlayerController _player;
    public bool canAttack = true;
    private StateMgr stateMgr;
    protected Turret turret;

    public float rotateSpeed = 100;
    public List<Transform> firePoints;
    public Image countDown;
    public bool canDestroy = true;
    public int destoryCoinValue = 5;

    public virtual void Start() {
        turret = new Turret();
        stateMgr = BattleSys.Instance.battleMgr.stateMgr;
        EventManager.OnPlayerLoaded += HandlePlayerLoaded;
    }

    public virtual void Update() {
        if(_player) {
            if(curAniState != AniState.Born) {
                turret.SetMove(this,_player.transform);
                turret.Rotate(_player.transform); 
            }
        }
        if(canAttack) {
            turret.Fire();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if(canDestroy) {
            if(collision.gameObject.layer == 8 || collision.gameObject.layer == 17) {
                ParticleMgr.Instance.PlayEnemyDeadParticle(collision.contacts[0],collision.transform);
                AudioManager.Instance.PlaySound(ResSvc.Instance.LoadAudio(Constants.HitEnenmyClip));
                gameObject.SetActive(false);
                Destroy(gameObject,1f);
                BattleSys.Instance.battleMgr.EliminateEnemy();
            } 
        }
    }

    public void Born() { 
        stateMgr.ChangeStatus(this,AniState.Born,null);
    }
    public void Take() {
        stateMgr.ChangeStatus(this,AniState.Take,null);
    }
    public void Idle() {
        stateMgr.ChangeStatus(this,AniState.Idle,null);
    }

    public void Attack() {
        stateMgr.ChangeStatus(this,AniState.Attack,null);
    }
     
    void OnDisable() { 
        EventManager.OnPlayerLoaded -= HandlePlayerLoaded;
    }

    void HandlePlayerLoaded(PlayerController player) {
        _player = player;
    } 
}
