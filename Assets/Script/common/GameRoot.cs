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
    public int CurWaveIndex { private set;  get; }

    // public LoadingWnd loadingWnd;
    public PlayerData PlayerData { private set; get; }

    public static GameRoot Instance { get; private set; }

    public string playerID = "12";//测试用玩家数据

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

    private void Init() {
        resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        gameSettings = resSvc.LoadConf();
        PlayerData = resSvc.GetPlayerData(playerID);
        UIManager.Instance.SetBgAuidoOn(gameSettings.bgAudio);
        UIManager.Instance.ShowJoyStick(gameSettings.showJoyStick);
        EnterMainCity();
    }


    public void GameStart() {
        homeWnd.Init();
        PlayBgAudio(UIManager.Instance.setPanel.GetBgAudioOn());
    }

    public void StartBattle(int wave) {
        if (PlayerData.current_wave < wave) {
            return;
        }
        CurWaveIndex = wave;
        homeWnd.gameObject.SetActive(false);
        GameObject go = new() {
            name = "BattleRoot"
        };
        go.transform.SetParent(transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.battleWnd = battleWnd;
        battleMgr.Init(CurWaveIndex);
        battleWnd.Init();
        PlayerData.energy--;
    }

    public void ContinueBattle() {//玩家重生继续游戏
        battleMgr.ContinueBattle();
    }

    public void GamePause() {
        UIManager.Instance.GameOver();
    }

    public void GameOver() {
        UIManager.Instance.GameOver();
    }

    public void GameWin() {
        UIManager.Instance.GameOver();
    }

    public void LevelSettlement(int coin) {//关卡结算
        PlayerData.coin += coin;
        PlayerData.current_wave = CurWaveIndex + 1;
        resSvc.SavePlayerData(playerID, PlayerData);
    }

    public void EnterMainCity() {
        resSvc.AsyncLoadScene("MainScene", () => {
            homeWnd.UpdateHomeWndCoinAndEnergy();
            homeWnd.gameObject.SetActive(true);
        });
    }

    public void ClickPauseBtn() {
        battleWnd.ClickPauseBtn();
    }

    private void Update() {
        bgPlayer.audioSource.volume = gamePause==true ? 0.2f:1f;
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

}
