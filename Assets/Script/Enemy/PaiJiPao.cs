using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaiJiPao : MonoBehaviour
{
    public GameObject paiJiPaoDan;

    private void Start() {
        PoolManager.Instance.InitPool(paiJiPaoDan,1);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            PaiJiPaoDan paodan =  PoolManager.Instance.GetInstance<GameObject>(paiJiPaoDan).GetComponent<PaiJiPaoDan>();
            paodan.SetPlayer(other.gameObject);
        }
    }

}
