using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public LoadingWnd loadingWnd;
    public bool gameStart = false;
    public bool gamePause = false;
    public GameObject HomeWnd;
    public BattleWnd battleWnd;
    public BattleMgr battleMgr;
    public SoundPlayer bgPlayer;

    // public LoadingWnd loadingWnd;

    public static GameRoot Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    private void Start() {
        ClearUIRoot();
        Init();
        GameStart();
    }

    private void ClearUIRoot() {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++) {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void GameStart() {
        HomeWnd.gameObject.SetActive(true);
        battleMgr = GetComponent<BattleMgr>();
        bgPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.BG1);
        bgPlayer.PlaySound(true);
    }

    public void GameOver() {
        UIManager.Instance.GameOver();
    }

    private void Init() {
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        //AudioSvc audio = GetComponent<AudioSvc>();
        //audio.InitSvc();

        //TimerSvc timer = GetComponent<TimerSvc>();
        //timer.InitSvc();

        //LoginSys login = GetComponent<LoginSys>();
        //login.InitSys();
 
 
        //BattleSys battle = GetComponent<BattleSys>();
        //battle.InitSys();
    }

    public void ClickPauseBtn() {
        battleWnd.ClickPauseBtn();
    }
    private void Update() {
        bgPlayer.audioSource.volume = gamePause==true ? 0.2f:1f;
    }
}
