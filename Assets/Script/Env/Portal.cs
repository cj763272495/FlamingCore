using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject exitPortal; // ��Ӧ�ĳ��ڴ�����
    
    // ����������봫����ʱ����
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            // �������ң�����ҵ�λ������Ϊ���ڴ����ŵ�λ��
            if(exitPortal) {
                PlayerController player = other.GetComponent<PlayerController>();
                ToolClass.SetGameObjectXZPos(player.gameObject, exitPortal.transform.position);
                player.lastPos = player.transform.position;
                //player.SetCam();
                //player.EnterIdleState(true);
                BattleSys.Instance.battleMgr.EnterBulletTime();
                EnterPortalCD();
                exitPortal.GetComponent<Portal>().EnterPortalCD();
            }
        }
    }
     
    void ShowPortal() {
        gameObject.SetActive(true);
        GetComponent<Collider>().enabled = true;
    }
    public async void EnterPortalCD() {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false; 
       await  ToolClass.CallAfterDelay(0.5f,() => {
            gameObject.SetActive(false);
            Invoke("ShowPortal",0.5f);
        });
    }
}
