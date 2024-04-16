using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class BattleWnd : MonoBehaviour
{
    private bool m_game_pause;
    public Image btn_pause;
    public Sprite[] img_pause_continue;
    public GameObject pause_panel;
    public PlayerController player;
    public FloatingJoystick joystick;
    public Text CoinTxt;

    public Text countdownText; 
    private float countdownTimeLeft; // 剩余倒计时时间

    public Laser laser;
    private BattleMgr battleMgr;

    // Start is called before the first frame update
    public void Init()
    {

        gameObject.SetActive(true);
        //if (joystick.IsDown) {
        //    player.Speed = Constants.SlowDownSpeed;
        //    //m_rotate_speed = Constants.SlowRotateSpeed;
        //    laser.SetEnable(true);
        //} else {
        //    laser.SetEnable(false);
        //}
        //if (Input.GetMouseButtonUp(0)) {
        //    player.Dir = joystick.UpDirection == Vector3.zero ? player.Dir : joystick.UpDirection;
        //    player.Speed = Constants.PlayerNormalSpeed;
        //    //m_rotate_speed = Constants.NormalRotateSpeed;
        //    player.IsMove = true;
        //    player.Pos = player.transform.position;
        //}
        if (GameRoot.Instance.gameSettings.bgAudio) {
            GameRoot.Instance.bgPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.BG2);
            GameRoot.Instance.bgPlayer.PlaySound(true);
        }
    }

    public void ClickPauseBtn() {
        if (GameRoot.Instance.gamePause == false) {
            GameRoot.Instance.gamePause = true; 
            btn_pause.sprite = img_pause_continue[1];
            Time.timeScale = 0f;
            pause_panel.SetActive(true);
        } else {
            pause_panel.SetActive(false);
            btn_pause.sprite = img_pause_continue[0]; 
            countdownText.gameObject.SetActive(true);
            StartCountDown();
        }
    }

    public void EarnMoney() {
        CoinTxt.text = battleMgr.GetCoinNum().ToString();
    }
    private async void StartCountDown() {
        countdownText.text = "3"; 
        // 等待3秒
        await Task.Delay(1000); // 第1秒
        countdownText.text = "2";
        await Task.Delay(1000); // 第2秒
        countdownText.text = "1";
        await Task.Delay(1000); // 第3秒
        ResumeGame();
    }
    private void ResumeGame() { 
        GameRoot.Instance.gamePause = false;
        countdownText.gameObject.SetActive(false);
        // 这里执行恢复逻辑
        joystick.IsDown = true;
    }

    private IEnumerator FadeCoroutine(float targetAlpha, float duration) {
        float startTime = Time.time;
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, targetAlpha * t);
            yield return null;
        }
        countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, targetAlpha);
    }


}
