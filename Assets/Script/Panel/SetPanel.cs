using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : MonoBehaviour
{

    public Toggle bgAuidoTg; 
    public Toggle joyStickTg;


    private void Start() {
        bgAuidoTg.onValueChanged.AddListener(OnBgAuidoTgValueChanged);
        joyStickTg.onValueChanged.AddListener(OnJoyStickTgValueChanged);
    }
    private void OnBgAuidoTgValueChanged(bool isOn) {
        GameRoot.Instance.PlayBgAudio(isOn);
        ResSvc.Instance.ChangeBgAudioConf(isOn);
    }
    private void OnJoyStickTgValueChanged(bool isShow) {
        GameRoot.Instance.gameSettings.showJoyStick = isShow;
        ResSvc.Instance.ChangeShowJoyStickConf(isShow);
    }

    public bool GetBgAudioOn() {
        return bgAuidoTg.isOn;
    }

    public bool GetJoyStickOn() {
        return joyStickTg.isOn;
    }

    public void SetBgAudio(bool on) {
        bgAuidoTg.isOn=on;
    }

    public void SetJoyStick(bool on) {
        joyStickTg.isOn = on;
    }

    public void OpenSetPanel() {
        gameObject.SetActive(true);
    }

    public void CloseSetPanel() {
        gameObject.SetActive(true);
    }

    public void ClickRetunLogin() {
        GameRoot.Instance.ClearUIRoot();
        ResSvc.Instance.AsyncLoadScene("StartScene",() => {
            gameObject.SetActive(false);
            UIManager.Instance.regLogInWnd.Init();
        });
    }
}
