using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerCannon : MonoBehaviour
{
    public GameObject missile; // �ڵ�Ԥ����
    public float fireInterval = 5.0f; // ������
    private Coroutine fireCoroutine; // ����Э��

    private void Start() { 
        PoolManager.Instance.InitPool(missile,3,BattleSys.Instance.battleMgr.transform);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            // �������ң���ʼ�����ڵ�
            fireCoroutine = StartCoroutine(FireBullets(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player") {
            // �������ң�ֹͣ�����ڵ�
            if(fireCoroutine != null) {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }
    }

    private IEnumerator FireBullets(GameObject player) {
        while(true) {
            // �����������ڵ�
            PoolManager.Instance.GetInstance<GameObject>(missile);
            //todo set player
            // �ȴ�fireInterval��
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
