using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PaiJiPaoDan : MonoBehaviour
{
    private GameObject _player;
    public ParticleSystem explosionParticle;
    public ParticleSystem tailGasParticle;

    public float speed = 5f; // ���յ��ٶ�
    public float riseTime = 5.0f; // ���յ�ʱ��
    private float riseTimer = 0.0f; // ���յļ�ʱ��

    public Canvas AmiCanvas;
    public Image countDown;
    private Vector3 targetPos ;
    private Vector3 beginDropDownPos;

    private void Start() {
        ParticleMgr.Instance.AddCustomParticle(explosionParticle.gameObject,2);
    }
     
    public void Shot(GameObject player) {
        riseTimer = 0.0f;
        targetPos = Vector3.zero;
        beginDropDownPos = Vector3.zero;
        AmiCanvas.gameObject.SetActive(false);
        _player = player;
        tailGasParticle.Play();
    }

    void Update() {
        if(!_player) {
            return;
        }
        if(riseTimer < riseTime) {
            transform.position += transform.forward * speed * Time.deltaTime;
            riseTimer += Time.deltaTime;
        } else { 
            if(targetPos==Vector3.zero) {
                AmiCanvas.gameObject.SetActive(true); 
                transform.position = new Vector3(_player.transform.position.x,
                    transform.position.y,_player.transform.position.z);
                beginDropDownPos = transform.position;
                targetPos = _player.transform.position;
                transform.forward = -transform.forward;
            }
            countDown.fillAmount =(transform.position.y - beginDropDownPos.y) / (targetPos.y - beginDropDownPos.y);
            Vector3 offset = transform.forward * speed * Time.deltaTime;
            transform.position += offset;
            AmiCanvas.transform.position = targetPos;
        }
    }

    private async void OnCollisionEnter(Collision collision) {
        tailGasParticle.Stop();
        AmiCanvas.gameObject.SetActive(false);
        explosionParticle.Play();
        await ToolClass.CallAfterDelay(0.5f,Explode);
        ParticleMgr.Instance.PlayCustomParticle(explosionParticle.gameObject,transform.position);
    }
     

    void Explode() {
        // ��ȡ�����ڱ�ը��Χ�ڵ�����
        Collider[] colliders = Physics.OverlapSphere(transform.position, countDown.rectTransform.sizeDelta.x/2); 
        foreach(Collider collider in colliders) { 
            if(collider.gameObject.layer == 1<< 7 ) {
                Destroy(collider.gameObject);
            }else if(collider.gameObject.tag == "Player") {
                collider.gameObject.GetComponent<PlayerController>().PlayerDead();
            }
        } 
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}
