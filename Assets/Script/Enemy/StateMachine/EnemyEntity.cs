using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    protected float rotateSpeed = 100;
    public GameObject player;
    protected bool canDestroy = true;
    public int destoryCoinValue = 5;
    protected bool canAttack = true;

    public virtual void Update() {
        if (player == null&& BattleSys.Instance.battleMgr.player) {
            player = BattleSys.Instance.battleMgr.player.gameObject;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 8 && canDestroy) {
            ParticleMgr.Instance.PlayEnemyDeadParticle(collision.contacts[0], collision.transform);
            AudioManager.Instance.PlaySound(ResSvc.Instance.LoadAudio(Constants.HitEnenmyClip));
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
}
