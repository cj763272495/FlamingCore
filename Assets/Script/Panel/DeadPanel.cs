using System.Collections; 
using UnityEngine;
using UnityEngine.UI;
public class DeadPanel : MonoBehaviour
{
    public BattleWnd battleWnd;
    public Image countDown;
    public Text countDownTxt;
    public Text reviveCoinTxt;
    private int m_reviveCost;
    public Button hpContinueBtn;
    public Button coinContinueBtn;

    public float countdownDuration = 5.0f; // 倒计时持续时间，单位为秒
    public float displayUpdateInterval = 1.0f; // 更新显示的间隔

    private float elapsedTime = 0.0f; // 已经过去的秒数

    private PlayersDataSystem pds;


    private void Start() {
        m_reviveCost = Constants.ReviceCost;
        reviveCoinTxt.text = m_reviveCost.ToString();
        pds = PlayersDataSystem.Instance;
    }

    public void ShowContinueBtn(bool hasHP) {
        hpContinueBtn.gameObject.SetActive(hasHP);
        coinContinueBtn.gameObject.SetActive(!hasHP);
    }

    public void SetContinueByCoinBtn(bool enoughCoin) {
        reviveCoinTxt.color =enoughCoin? Color.white : Color.red;
        hpContinueBtn.enabled = enoughCoin;
    }

    public void ShowAndStartCountDown() {
        hpContinueBtn.enabled = true;
        gameObject.SetActive(true);
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine() {
        elapsedTime = countdownDuration; 
        while (elapsedTime > 0) {
            elapsedTime -= Time.deltaTime; 
            UpdateCountdownDisplay(elapsedTime);
            yield return null;
        }
        FinishCountdown();
    }

    void UpdateCountdownDisplay(float timeLeft) {
        countDownTxt.text = Mathf.CeilToInt(timeLeft).ToString();
        countDown.fillAmount = timeLeft / countdownDuration;
    }

    void FinishCountdown() {
        // 倒计时结束
        ClickCancelBtn();
    }

    public void ClickCoinContinueBtn() {
        //如果当前剩余金额大于所需，继续进行,玩家原地重生
        if(pds.PlayerData.coin >= m_reviveCost) {
            pds.PlayerData.coin -= m_reviveCost;
            BattleSys.Instance.ReviveAndContinueBattle();
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
    }

    public void ClickHpContinueBtn() {
        //如果当前剩余金额大于所需，继续进行,玩家原地重生
        //if (pds.PlayerData.coin >= m_reviveCost) {
        //    pds.PlayerData.coin -= m_reviveCost;
        //    BattleSys.Instance.ReviveAndContinueBattle();
        //    gameObject.SetActive(false);
        //    StopAllCoroutines();
        //}
        BattleSys.Instance.ReviveAndContinueBattle();
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    public void ClickCancelBtn() {
        UIManager.Instance.battleWnd.fail_panel.gameObject.SetActive(true);
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.LevelSettlement();
        //gameObject.SetActive(false);
        //BattleSys.Instance.battleMgr.DestoryBattle();
        //GameRoot.Instance.EnterMainCity();
    }
}
