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

    public void ClickRetunLogin() {
        ResSvc.Instance.AsyncLoadScene("StartScene",() => {
            GameRoot.Instance.ClearUIRoot();
            gameObject.SetActive(false);
            UIManager.Instance.regLogInWnd.Init();
            UIManager.Instance.homeWnd.mainShow.SetActive(false);
        });
    }
}
