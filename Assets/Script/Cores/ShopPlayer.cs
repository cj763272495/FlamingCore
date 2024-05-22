using System.Collections;
using UnityEngine; 

public class ShopPlayer:PlayerController {
    private IEnumerator coroutine;
    public ParticleSystem hitWallParticle;
    public GameObject[] coreModes;
    public GameObject[] trails; 

    public override void Init(BattleMgr b=null,StateMgr state=null) {
        gameObject.SetActive(true); 
        lastPos = transform.position;
        _speed = Constants.PlayerSpeed;
        _rotateSpeed = Constants.RotateSpeed;
        coroutine = CallSetDir();
        StartCoroutine(coroutine);
        coreModes[0].SetActive(true);
        trails[0].SetActive(true);
    }
    

    private IEnumerator CallSetDir() {
        while(true) {
            Vector3 randomVector = new Vector3(Random.Range(-1f,1f),0,Random.Range(-1f,1f));
            SetDir(randomVector);
            SetScale(1f);
            yield return new WaitForSeconds(Random.Range(3,5));
        }
    }

    private void SetScale(float contineuTime) {
        Time.timeScale = 0.3f;
        ToolClass.CallAfterDelay(contineuTime,() => { Time.timeScale = 1; });
    }

    protected override void FixedUpdate() {
        SetMove();
        SetRotateAndShot();
    }

    protected override void OnCollisionEnter(Collision collision) {
        hitWallParticle.transform.position = collision.contacts[0].point;
        hitWallParticle.Play();
        ContactPoint contactPoint = collision.contacts[0];
        //计算反射方向
        Vector3 inDirection = (transform.position - lastPos).normalized;
        Vector3 inNormal = contactPoint.normal;
        inNormal.y = 0;
        Vector3 tempDir = Vector3.Reflect(inDirection,inNormal).normalized;
        if(!Physics.Raycast(transform.position,tempDir,0.5f)) {
            _dir = tempDir;
        } else {
            _dir = Vector3.Reflect(tempDir,inNormal).normalized;
        }
        lastPos = transform.position; //更新上一次位置，用于计算反射方向
    }

    public void ChangeModes(int index) {
        ChangeProps(index,coreModes); 
    }
    public void ChangeTrails(int index) {
        ChangeProps(index, trails);
    }

    public void ChangeProps(int index ,GameObject[] props) {
        if(index > props.Length-1) {
            Debug.LogError("out of range");
            return;
        }
        foreach(GameObject item in props) {
            item.SetActive(false);
        }
        props[index].SetActive(true);
    }

}
