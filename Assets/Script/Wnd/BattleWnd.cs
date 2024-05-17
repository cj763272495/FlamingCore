using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.VisualScripting;
using DG.Tweening;

public class BattleWnd:MonoBehaviour {
    public Image btn_pause;
    public Sprite[] img_pause_continue;
    public PausePanel pause_panel;
    public WinPanel win_panel;
    public DeadPanel dead_panel;
    public FailPanel fail_panel; 
    public TransitionLevelPanel transitionLevelPanel;
    public Text hp_txt;
    public Text countdownText;

    public GameObject energyShow;
    public GameObject coinShow;
    public GameObject hpShow;
    public GuideLine guideLine;
    public FloatingJoystick joystick;

    public BattleMgr battleMgr;

    public void Init() {
        gameObject.SetActive(true);
        win_panel.gameObject.SetActive(false);
        dead_panel.gameObject.SetActive(false);
        fail_panel.gameObject.SetActive(false);
        pause_panel.gameObject.SetActive(false);
        transitionLevelPanel.gameObject.SetActive(false);
        battleMgr.OnStartBattleChanged += HandleStartBattleChanged;
    }
 
    private void HandleStartBattleChanged(bool startBattle) {
        btn_pause.gameObject.SetActive(startBattle);
    }

    public void ClickPauseBtn() {
        pause_panel.gameObject.SetActive(true); 
        battleMgr.PauseBattle(); 
    }
    public Tween StartCountDown3Seconds() {
        countdownText.text = "3";
        countdownText.gameObject.SetActive(true);
        countdownText.color = new Color(countdownText.color.r,countdownText.color.g,countdownText.color.b,1f);
        DG.Tweening.Sequence sequence = DOTween.Sequence().SetUpdate(UpdateType.Normal,true);
        for(int i = 3; i > 0; --i) {
            int j = i;
            sequence.AppendInterval(0.8f)
                    .Append(countdownText.transform.DOScaleY(0,0.1f)) // 将字体的高度变为0
                    .AppendCallback(() => countdownText.text = (j - 1).ToString()) // 更新显示的数字
                    .Append(countdownText.transform.DOScaleY(1,0.1f)); // 将字体的高度变为1
        }
        sequence.OnComplete(() => countdownText.gameObject.SetActive(false));
        return sequence.Play();
    }


    public void ShowHp(bool isShow = true) {
        hpShow.SetActive(isShow);
        energyShow.SetActive(!isShow);
        coinShow.SetActive(!isShow);
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
