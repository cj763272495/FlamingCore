using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadCoin : MonoBehaviour
{
    public ParticleSystem lastParticleSystem;
    public GameObject player;
    public bool isPlaying = false;
    public int coinValue;

    private void OnTriggerEnter(Collider other) {
        //if(other.comparetag("player")) {
        //    soundplayer = collision.gameobject.getcomponent<soundplayer>();
        //    soundplayer.clipsource = ressvc.instance.loadaudio(constants.hitenenmyclip);
        //    soundplayer.playsound();
        //    particlemgr.instance.playenemydeadparticle(other.transform.position,other.transform);
        //    destroy(gameobject);
        //}
    }

    private void Update() {
        if(player && isPlaying) {
            StartChase();
        }
    }

    public void StartChase() { 
        StartCoroutine(AttractParticlesToPlayerAfterDelay(lastParticleSystem, 0.4f));//�ӳ�һ��ʱ������ӷ������
    }

    private IEnumerator AttractParticlesToPlayerAfterDelay(ParticleSystem particleSystem,float delay) {
        yield return new WaitForSeconds(delay);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        int numParticles = particleSystem.GetParticles(particles);

        for(int i = 0; i < numParticles; i++) {
            Vector3 direction = player.transform.position - particles[i].position;
            float speed = 15f +500 * Time.deltaTime; 
            particles[i].velocity = direction.normalized * speed;

            if(Vector3.Distance(particles[i].position, player.transform.position) < 1f) {
                particles[i].remainingLifetime = 0;
                AudioClip clip = ResSvc.Instance.LoadAudio(Constants.EarnMoneyClip,true);
                AudioManager.Instance.PlaySound(clip);
                //֪ͨbattleMgr���ӽ��
                BattleSys.Instance.battleMgr.EarnCoin(coinValue);
            }
        }

        particleSystem.SetParticles(particles,numParticles);
    }
}
