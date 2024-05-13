using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float rotateSpeed = 100;
    public GameObject player;
    protected bool canDestroy = true;
    public int destoryCoinValue = 5;
    protected bool canAttack = true;

    public virtual void Update() {
        if (player == null) {
            player = BattleSys.Instance.battleMgr.player.gameObject;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 8 && canDestroy) {
            ParticleMgr.Instance.PlayEnemyDeadParticle(collision.contacts[0], collision.transform);
            AudioManager.Instance.PlaySound(ResSvc.Instance.LoadAudio(Constants.HitEnenmyClip));

            StartCoroutine(DestroyAfterSeconds(1));
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DestroyAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
