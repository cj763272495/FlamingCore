using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Text hpTxt;
    private void OnEnable() {
        hpTxt.text = "x" + BattleSys.Instance.battleMgr.GetHP();
    }

    public void ClickReturn() {//����������
        BattleSys.Instance.battleMgr.DestoryBattle();
        gameObject.SetActive(false);
        GameRoot.Instance.EnterMainCity();
    }
    public void ClickRetry() {//���¿�ʼ
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.PlayAgain(BattleSys.Instance.battleMgr.CurLevelID);
    } 
    
    public void ClickBack() {//������Ϸ
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.ResumeBattle();
    }
}
