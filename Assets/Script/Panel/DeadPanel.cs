using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeadPanel : MonoBehaviour
{
    public BattleWnd battleWnd;
    public Image countDown;
    public Text countDownTxt;
    public Text reviveCoinTxt;
    private int m_reviveCost;
    public Button continueBtn;

    public float countdownDuration = 5.0f; // 倒计时持续时间，单位为秒
    public float displayUpdateInterval = 1.0f; // 更新显示的间隔

    private float elapsedTime = 0.0f; // 已经过去的秒数


    private void Start() {
        m_reviveCost = Constants.ReviceCost;
        reviveCoinTxt.text = m_reviveCost.ToString();
    }

    public void CannotContinueByCoin() {
        reviveCoinTxt.color = Color.red;
        continueBtn.enabled = false;
    }

    public void ShowAndStartCountDown() {
        continueBtn.enabled = true;
        gameObject.SetActive(true);
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine() {
        elapsedTime = countdownDuration; 
        // 倒计时开始
        while (elapsedTime > 0) {
            elapsedTime -= Time.deltaTime; 
            // 更新UI或游戏状态显示倒计时
            UpdateCountdownDisplay(elapsedTime);
            yield return null; // 等待下一帧
        }

        // 倒计时结束，可以在这里处理倒计时结束的逻辑
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

    public void ClickContinueBtn() {
        //如果当前剩余金额大于所需，继续进行,玩家原地重生
        if (GameRoot.Instance.PlayerData.coin >= m_reviveCost) {
            GameRoot.Instance.PlayerData.coin -= m_reviveCost;
            BattleSys.Instance.ContinueBattle();
        }
    }

    public void ClickCancelBtn() {
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.DestoryBattle();
        GameRoot.Instance.EnterMainCity();
    }
}
