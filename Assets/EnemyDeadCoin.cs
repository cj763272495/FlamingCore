using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadCoin : MonoBehaviour
{
    public ParticleSystem lastParticleSystem;
    private PlayerController player;
    public bool isPlaying = false;
    private float maxTurnAngle = 30f;

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
        player = FindObjectOfType<PlayerController>() ?? player;
        if(player && isPlaying) {
            StartChase();
        }
    }

    public void StartChase() { 
        StartCoroutine(AttractParticlesToPlayerAfterDelay(lastParticleSystem, 0.4f));
    }

    private IEnumerator AttractParticlesToPlayerAfterDelay(ParticleSystem particleSystem,float delay) {
        yield return new WaitForSeconds(delay);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        int numParticles = particleSystem.GetParticles(particles);

        for(int i = 0; i < numParticles; i++) {
            Vector3 direction = player.gameObject.transform.position - particles[i].position;
            float speed = 15f +500 * Time.deltaTime; // Increase speed over time
            particles[i].velocity = direction.normalized * speed;

            // Check if the particle is close to the player
            if(Vector3.Distance(particles[i].position,player.gameObject.transform.position) < 1f) {
                // If the particle is close to the player, set its remaining lifetime to 0
                particles[i].remainingLifetime = 0;

                //²¥·ÅÊ°È¡ÒôÐ§
            }
        }

        particleSystem.SetParticles(particles,numParticles);
    }
}
