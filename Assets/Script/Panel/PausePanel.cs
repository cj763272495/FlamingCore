using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PausePanel : MonoBehaviour
{ 
    public void ClickReturn() {//����������
        GameRoot.Instance.EnterMainCity();
        LeaveScene();
    }
    public void ClickRetry() {//���¿�ʼ
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.PlayAgain();
    }

    public void LeaveScene() { //�뿪�ؿ�����
        BattleSys.Instance.battleMgr.DestoryBattle();
        gameObject.SetActive(false);
        GameRoot.Instance.EnterMainCity(); 
    }
    
    public void ClickBack() {//������Ϸ
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.ResumeBattle();
    }
}
