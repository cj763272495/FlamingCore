using UnityEngine;
using UnityEngine.UI;
public class PaiJiPaoDan : MonoBehaviour
{
    private GameObject _player;
    public ParticleSystem explosionParticle;

    public float speed = 5f; // ���յ��ٶ�
    public float riseTime = 5.0f; // ���յ�ʱ��
    private float riseTimer = 0.0f; // ���յļ�ʱ��

    public Canvas AmiCanvas;
    public Image countDown;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 beginDropDownPos = Vector3.zero; 

    private void OnEnable() {
        riseTimer = 0.0f;
        targetPos = Vector3.zero;
        beginDropDownPos = Vector3.zero;
        AmiCanvas.gameObject.SetActive(false);
    }

    public void SetPlayer(GameObject player) {
        _player = player;
    }

    void Update() {
        if(riseTimer < riseTime) {
            transform.position += Vector3.up * speed * Time.deltaTime;
            riseTimer += Time.deltaTime;
        } else { 
            if(targetPos==Vector3.zero) {
                AmiCanvas.gameObject.SetActive(true); 
                transform.position = new Vector3(_player.transform.position.x,
                    transform.position.y,_player.transform.position.z);
                beginDropDownPos = transform.position;
                targetPos = _player.transform.position;
                AmiCanvas.transform.position = targetPos;
            }
            countDown.fillAmount =(targetPos.y - transform.position.y) / (targetPos.y - beginDropDownPos.y);
            Vector3 offset = Vector3.up * speed * Time.deltaTime;
            transform.position -= offset;
            AmiCanvas.transform.position += offset;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        AmiCanvas.gameObject.SetActive(false);
        explosionParticle.Play();
        PoolManager.Instance.ReturnToPool(gameObject);
        ToolClass.CallAfterDelay(0.5f,Explode);
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
    }
}
