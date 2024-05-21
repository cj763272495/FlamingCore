using System.Collections;
using UnityEngine;

public class ShopPlayer:PlayerController {
    private IEnumerator coroutine;
    public ParticleSystem hitWallParticle;
    public Material[] materials;
    public GameObject[] trails;
    public GameObject ShopShow;

    public override void Init() {
        gameObject.SetActive(true);
        ShopShow.SetActive(true);
        lastPos = transform.position;
        _speed = Constants.PlayerSpeed;
        _rotateSpeed = Constants.RotateSpeed;
        coroutine = CallSetDir();
        StartCoroutine(coroutine);
        trails[0].SetActive(true);
    }

    public void LeavePanel() {
        gameObject.SetActive(false);
        ShopShow.SetActive(false);
        //gameObject.SetActive(false);
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
        SetRotate();
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

    public void ChangeMaterial(GameObject go, int coreIndex) {
        if(coreIndex > materials.Length-1) {
            Debug.LogError("coreIndex out of range");
            return;
        }
        Renderer renderer = go.GetComponentInChildren<Renderer>();
        if(renderer != null && renderer.materials.Length > 0) {
            Material[] materials = renderer.materials;
            materials[0] = materials[coreIndex];
            renderer.materials = materials;
        }
    }

    public void ChangeTrail(int trailIndex) {
        if(trailIndex > trails.Length-1) {
            Debug.LogError("trailIndex out of range");
            return;
        }
        switch(trailIndex) {
            case 0:
                trails[0].SetActive(false);
                break;
            case 1:
                trails[1].SetActive(true);
                break;
            case 2:
                trails[2].SetActive(true);
                break;
            case 3:
                trails[3].SetActive(true);
                break;
            case 4:
                trails[4].SetActive(true);
                break;
            default:
                break;
        }
    }

}
