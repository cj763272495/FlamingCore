using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Animator ani; 
    public LineRenderer line;
    public float rayLength = 20.0f; // 射线的长度 
    public float growSpeed = 1000.0f; // 射线增长的速度
    public ParticleSystem startParticle;
    public ParticleSystem endParticle;

    public void StartShow() {
        PlayAni();
    }

    IEnumerator GrowRay() {
        startParticle.Play();
        endParticle.Play();
        float currentLength = 0.0f; 
        line.SetPosition(0, line.transform.position); 
        while(currentLength < rayLength) { 
            currentLength += growSpeed * Time.deltaTime; 
            line.SetPosition(1,transform.position + Vector3.up * currentLength);
            endParticle.transform.position = transform.position + Vector3.up * currentLength;
            yield return null;
        }
    }

    public void PlayAni() {
        ani.Play("checkpoint_2");
        ToolClass.CallAfterDelay(2.7f,OpenEnd);
    }

    public void OpenEnd() {
        StartCoroutine(GrowRay()); 
    }
}
