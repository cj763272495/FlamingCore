using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class BattleWnd:MonoBehaviour {
    public Image btn_pause;
    public Sprite[] img_pause_continue;
    public PausePanel pause_panel;
    public WinPanel win_panel;
    public DeadPanel dead_panel;
    public FailPanel fail_panel; 
    public Text hp_txt;
    public Text countdownText;

    public GameObject energyShow;
    public GameObject hpShow;
    public GuideLine guideLine;
    public FloatingJoystick joystick;

    public BattleMgr battleMgr;

    public void Init() {
        gameObject.SetActive(true);
        win_panel.gameObject.SetActive(false);
        dead_panel.gameObject.SetActive(false);
        pause_panel.gameObject.SetActive(false); 
        battleMgr.OnStartBattleChanged += HandleStartBattleChanged;
    }
 
    private void HandleStartBattleChanged(bool startBattle) {
        btn_pause.gameObject.SetActive(startBattle);
    }

    public void ClickPauseBtn() {
        pause_panel.gameObject.SetActive(true); 
        battleMgr.PauseBattle(); 
    }

    public async void StartCountDown3Seconds() {
        countdownText.gameObject.SetActive(true);
        for(int i = 3; i > 0; i--) {
            countdownText.text = i.ToString();
            await Task.Delay(1000);
        }
        countdownText.gameObject.SetActive(false);

    }

    public void ShowHp(bool isShow = true) {
        hpShow.SetActive(isShow);
        energyShow.SetActive(!isShow); 
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
