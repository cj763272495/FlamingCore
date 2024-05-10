using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public HomeWnd homeWnd;
    public PausePanel pausePanel;
    public DeadPanel deadPanel;
    public WinPanel winPanel;
    public SetPanel setPanel;
    public StartPanel startPanel;
    public RegLoginWnd regLogInWnd;
    public LoadingWnd loadingWnd;
    public static UIManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void SetBgAuidoOn(bool isOn) {
        setPanel.SetBgAudio(isOn);
    }
    public void ShowJoyStick(bool isOn) {
        setPanel.SetJoyStick(isOn);
    }
}
