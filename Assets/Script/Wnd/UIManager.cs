using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public HomeWnd homeWnd;
    public PausePanel pausePanel;
    public SetPanel setPanel;
    public static UIManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
     
    public void OpenPasueWnd(bool open = true) {
        pausePanel.gameObject.SetActive(open);
    }

    public void GameOver() {
        pausePanel.gameObject.SetActive(true);
    }

    public void SetBgAuidoOn(bool isOn) {
        setPanel.SetBgAudio(isOn);
    }
    public void ShowJoyStick(bool isOn) {
        setPanel.SetJoyStick(isOn);
    }
}
