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

    public float countdownDuration = 5.0f; // ����ʱ����ʱ�䣬��λΪ��
    public float displayUpdateInterval = 1.0f; // ������ʾ�ļ��

    private float elapsedTime = 0.0f; // �Ѿ���ȥ������


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
        // ����ʱ��ʼ
        while (elapsedTime > 0) {
            elapsedTime -= Time.deltaTime; 
            // ����UI����Ϸ״̬��ʾ����ʱ
            UpdateCountdownDisplay(elapsedTime);
            yield return null; // �ȴ���һ֡
        }

        // ����ʱ���������������ﴦ����ʱ�������߼�
        FinishCountdown();
    }

    void UpdateCountdownDisplay(float timeLeft) {
        countDownTxt.text = Mathf.CeilToInt(timeLeft).ToString();
        countDown.fillAmount = timeLeft / countdownDuration;
    }

    void FinishCountdown() {
        // ����ʱ����
        ClickCancelBtn();
    }

    public void ClickContinueBtn() {
        //�����ǰʣ����������裬��������,���ԭ������
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
