using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool startRotate = false;
    public ParticleSystem ps;
    public float rotateSpeed=100;
    public GameObject Quad; 
    public ParticleSystem coinPS;
    public Transform coinTargetPos;
    public Camera chestCam;
    public AudioClip getCoinClip;

    private void Start() {
        gameObject.SetActive(false);
    }

    private void Update() {
        if(startRotate) {
            transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
        }
    }
    public void OpenChest() {
        Quad.SetActive(false);
        startRotate = true;
        gameObject.SetActive(true);
        animator.SetBool("Open", true);
    }

    public void PlayParticle() {
        UIManager.Instance.winPanel.Show1stCoinTxt();
        var emission = coinPS.emission;
        emission.rateOverTime =  Mathf.Max(BattleSys.Instance.battleMgr.GetCoin(),40);
        ps.Play();
        coinPS.Play();
        StartCoroutine(CheckParticleDistance());
        StartCoroutine(StopParticleMovementAfterTime(0.6f));
    }

    public void Exit() {
        startRotate = false;
        ps.Stop();
        gameObject.SetActive(false);
    }

    private ParticleSystem.Particle[] particles;
    int numParticles;
    private IEnumerator StopParticleMovementAfterTime(float time) {
        yield return new WaitForSeconds(time+0.6f);//等待运动时间+发射时间后停止
         
        particles = new ParticleSystem.Particle[coinPS.particleCount];
        numParticles = coinPS.GetParticles(particles);

        for(int i = 0; i < numParticles; i++) {
            particles[i].velocity = Vector3.zero;
        }

        coinPS.SetParticles(particles,numParticles);
        yield return new WaitForSeconds(1);

        particles = new ParticleSystem.Particle[coinPS.particleCount];
        numParticles = coinPS.GetParticles(particles);
        for(int i = 0; i < numParticles; i++) {
            Vector3 direction = (coinTargetPos.position - particles[i].position).normalized;
            particles[i].velocity = direction * Random.Range(5,7);
        }

        coinPS.SetParticles(particles,numParticles); 
    }

    private IEnumerator CheckParticleDistance() {
        while(coinPS.isPlaying) {
            particles = new ParticleSystem.Particle[coinPS.particleCount];
            numParticles = coinPS.GetParticles(particles);
            for(int i = 0; i < numParticles; i++) {
                float distance = Vector3.Distance(coinTargetPos.position,particles[i].position);
                if(distance < 0.1f) {
                    particles[i].remainingLifetime = 0;
                    ParticleMgr.Instance.PlayGetCoinParticle(particles[i].position);
                    AudioManager.Instance.PlaySound(getCoinClip);
                }
            }
            coinPS.SetParticles(particles,numParticles); 
            yield return null;
        }
        UIManager.Instance.winPanel.SecndShowCoinTxt();
    }
}
