using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Text hpTxt;
    private void OnEnable() {
        hpTxt.text = "x" + BattleSys.Instance.battleMgr.GetHP();
    }

    public void ClickReturn() {//返回主界面
        BattleSys.Instance.battleMgr.DestoryBattle();
        gameObject.SetActive(false);
        GameRoot.Instance.EnterMainCity();
    }
    public void ClickRetry() {//重新开始
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.PlayAgain(BattleSys.Instance.battleMgr.CurLevelID);
    } 
    
    public void ClickBack() {//继续游戏
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.ResumeBattle();
    }
}
