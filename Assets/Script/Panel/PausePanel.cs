using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PausePanel : MonoBehaviour
{ 
    public void ClickReturn() {//返回主界面
        GameRoot.Instance.EnterMainCity();
        LeaveScene();
    }
    public void ClickRetry() {//重新开始
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.PlayAgain();
    }

    public void LeaveScene() { //离开关卡场景
        BattleSys.Instance.battleMgr.DestoryBattle();
        gameObject.SetActive(false);
        GameRoot.Instance.EnterMainCity(); 
    }
    
    public void ClickBack() {//继续游戏
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.ResumeBattle();
    }
}
