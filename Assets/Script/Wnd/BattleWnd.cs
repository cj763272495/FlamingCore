using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class BattleWnd : MonoBehaviour
{
    public Image btn_pause;
    public Sprite[] img_pause_continue;
    public PausePanel pause_panel;
    public WinPanel win_panel;
    public DeadPanel dead_panel;
    public FailPanel fail_panel;
    public PlayerController player;
    public FloatingJoystick joystick;
    public Text coin_txt;
    public Text hp_txt;
    public Text countdownText;

    public GameObject coinShow;
    public GameObject energyShow;
    public GameObject hpShow;

    public Laser laser;
    public BattleMgr battleMgr;

    public void Init()
    {
        gameObject.SetActive(true);
        win_panel.gameObject.SetActive(false);
        dead_panel.gameObject.SetActive(false);
        pause_panel.gameObject.SetActive(false);
        if (GameRoot.Instance.gameSettings.bgAudio) {
            GameRoot.Instance.bgPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.BG2);
            GameRoot.Instance.bgPlayer.PlaySound(true);
        }
    }

    public void ClickPauseBtn() {
        if (GameRoot.Instance.gamePause == false) {
            GameRoot.Instance.gamePause = true; 
            btn_pause.sprite = img_pause_continue[1];
            pause_panel.gameObject.SetActive(true);
            battleMgr.PauseBattle();
        } else {
            btn_pause.sprite = img_pause_continue[0];
            pause_panel.gameObject.SetActive(false);  
            countdownText.gameObject.SetActive(true);  
            StartCountDown3Seconds(countdownText);
            battleMgr.ResumeBattle();
        }
    }

    private async void StartCountDown3Seconds(Text countdownText) {
        countdownText.text = "3";
        // µÈ´ý3Ãë
        await Task.Delay(1000);
        countdownText.text = "2";
        await Task.Delay(1000);
        countdownText.text = "1";
        await Task.Delay(1000);
        countdownText.gameObject.SetActive(false);
    }
    public void EarnMoney() {
        coin_txt.text = battleMgr.GetCoinNum().ToString();
    }

    public void ShowHp(bool isShow=true) {
        if (isShow) {
            hpShow.SetActive(true);
            energyShow.SetActive(false);
            coinShow.SetActive(false);
        } else {
            hpShow.SetActive(false);
            energyShow.SetActive(true);
            coinShow.SetActive(true);
        }
    }
    
    //private IEnumerator FadeCoroutine(float targetAlpha, float duration) {
    //    float startTime = Time.time;
    //    while (Time.time - startTime < duration) {
    //        float t = (Time.time - startTime) / duration;
    //        countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, targetAlpha * t);
    //        yield return null;
    //    }
    //    countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, targetAlpha);
    //} 
}
