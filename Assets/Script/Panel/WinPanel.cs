using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Text curLevel;
    public Text coinTxt;

    private readonly int minSize = 30;
    private readonly int maxSize = 80;
    private readonly float cycleDuration = 2.0f; 
    public float breathRate = 2.0f;
    public Image BreathImage; 
    public Chest chest;

    public void SetWinPanelLevelTxt(string wave) {
        curLevel.text = wave;
    }

    public void OpenWinPanel(int coinNum) {
        coinTxt.text = "+ " + coinNum.ToString();
        gameObject.SetActive(true);  
        chest.OpenChest();
        InvokeRepeating(nameof(Breathe), 0f, breathRate);
    }

    void Breathe() {
        float t = Mathf.Sin(Time.time * Mathf.PI / breathRate);
        float alpha = t * 0.5f + 0.5f;
        BreathImage.color = new Color(1,1,1,alpha);
    }

    public void ClickReturnHomeBtn() {
        LeaveWinPanel();
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickNextWaveBtn() {
        LeaveWinPanel();
        BattleSys.Instance.battleMgr.StratNextLevel();
    }

    public void ClickAgainBtn() {
        StopAllCoroutines();
        CancelInvoke("Breathe");
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.PlayAgain();
        chest.Exit();
    }

    private void LeaveWinPanel() {
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.DestoryBattle();
        StopAllCoroutines();
        CancelInvoke("Breathe");
        chest.Exit();
    }
}
