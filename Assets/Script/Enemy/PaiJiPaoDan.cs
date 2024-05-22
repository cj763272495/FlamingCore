using UnityEngine;
using UnityEngine.UI;
public class PaiJiPaoDan : MonoBehaviour
{
    private GameObject _player;
    public ParticleSystem explosionParticle;

    public float speed = 10.0f; // 升空的速度
    public float riseTime = 2.0f; // 升空的时间
    private float riseTimer = 0.0f; // 升空的计时器

    public Image exploreRange;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 beginDropDownPos = Vector3.zero;


    private void OnEnable() {
        riseTimer = 0.0f;
        targetPos = Vector3.zero;
        beginDropDownPos = Vector3.zero; 
    }

    public void SetPlayer(GameObject player) {
        _player = player;
    }

    void Update() {
        if(riseTimer < riseTime) {
            transform.position += Vector3.up * speed * Time.deltaTime;
            riseTimer += Time.deltaTime;
        } else { 
            if(targetPos!=Vector3.zero) {
                exploreRange.gameObject.SetActive(true);
                transform.position = new Vector3(_player.transform.position.x,
                    transform.position.y,_player.transform.position.z);
                beginDropDownPos = transform.position;
                targetPos = _player.transform.position;
            }
            exploreRange.fillAmount =Mathf.Abs(transform.position.y - beginDropDownPos.y / (targetPos.y - beginDropDownPos.y));
            transform.position -= Vector3.up * speed * Time.deltaTime; 
        }
    }

    private void OnCollisionEnter(Collision collision) {
        exploreRange.gameObject.SetActive(false);
        explosionParticle.Play();
        Explode();
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    void Explode() {
        // 获取所有在爆炸范围内的物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, exploreRange.rectTransform.sizeDelta.x/2); 
        foreach(Collider collider in colliders) { 
            if(collider.gameObject.layer == 1<< 7 ) {
                Destroy(collider.gameObject);
            }else if(collider.gameObject.tag == "Player") {
                collider.gameObject.GetComponent<PlayerController>().PlayerDead();
            }
        }
    }
}
