using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public LoadingWnd loadingWnd;
    public bool gameStart = false;
    public bool gamePause = false;
    public BattleWnd battleWnd;
    public HomeWnd homeWnd;
    public BattleMgr battleMgr;
    public SoundPlayer bgPlayer;
    public SoundPlayer effectAudioPlayer;

    public GameSettings gameSettings;
    private ResSvc resSvc;
    public int curWaveIndex { private set;  get; }

    // public LoadingWnd loadingWnd;
    public PlayerData playerData { private set; get; }

    public static GameRoot Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    //仅测试用
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
        homeWnd.gameObject.SetActive(true);
        PlayBgAudio(UIManager.Instance.setPanel.GetBgAudioOn());
    }
    public void ContinueBattle() {//玩家重生继续游戏
        battleMgr.ContinueBattle();
    }

    public void PlayBgAudio(bool isOn) {
        if (isOn) {
            bgPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.BG1);
            bgPlayer.PlaySound(true);
        } else {
            if (bgPlayer.audioSource.isPlaying) {
                bgPlayer.audioSource.Pause();
            }
        }
    }

    public void StartBattle(int wave) {
        if (playerData.current_wave < wave) {
            return;
        }
        curWaveIndex = wave;
        homeWnd.gameObject.SetActive(false);
        GameObject go = new GameObject {
            name = "BattleRoot"
        };
        go.transform.SetParent(transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.battleWnd = battleWnd;
        battleMgr.Init(curWaveIndex);
        battleWnd.Init();
        playerData.energy--;
    }

    public void GameOver() {
        UIManager.Instance.GameOver();
    }
    public void GamePause() {
        UIManager.Instance.GameOver();
    }
    public void GameWin() {
        UIManager.Instance.GameOver();
    }
    public void LevelSettlement(int coin) {//关卡结算
        playerData.coin += coin;
        playerData.current_wave = curWaveIndex + 1;
    }
    private void Init() {
        resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        gameSettings = resSvc.LoadConf();
        playerData = resSvc.GetPlayerData("11");
        UIManager.Instance.SetBgAuidoOn(gameSettings.bgAudio);
        UIManager.Instance.ShowJoyStick(gameSettings.showJoyStick);
        EnterMainCity();
        //AudioSvc audio = GetComponent<AudioSvc>();
        //audio.InitSvc();

        //TimerSvc timer = GetComponent<TimerSvc>();
        //timer.InitSvc();

        //LoginSys login = GetComponent<LoginSys>();
        //login.InitSys();


        //BattleSys battle = GetComponent<BattleSys>();
        //battle.InitSys();
    }

    public void EnterMainCity() {
        resSvc.AsyncLoadScene("MainScene", () => {
            homeWnd.gameObject.SetActive(true);
        });
    }

    public void ClickPauseBtn() {
        battleWnd.ClickPauseBtn();
    }
    private void Update() {
        bgPlayer.audioSource.volume = gamePause==true ? 0.2f:1f;
    }
}
