using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public bool gameStart = false;
    public HomeWnd homeWnd;
    public LoadingWnd loadingWnd;

    public BattleMgr battleMgr;
    public SoundPlayer bgPlayer;
    public SoundPlayer effectAudioPlayer;

    public GameSettings gameSettings;
    private ResSvc resSvc;
    public int CurWaveIndex { private set;  get; }
     
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

        BattleSys battle = GetComponent<BattleSys>();
        battle.InitSys();
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
        if (PlayerData.energy<0) {
            Debug.Log("能量不足");
            return;
        }
        if(wave >PlayerData.max_unLock_wave) {
            //关卡未解锁
            return;
        }
        homeWnd.gameObject.SetActive(false);
        BattleSys.Instance.StartBattle(wave);
    }

    public void LevelSettlement(int coin) {//关卡结算
        PlayerData.coin += coin;
        PlayerData.max_unLock_wave = CurWaveIndex + 1;
        resSvc.SavePlayerData(playerID, PlayerData);
    }

    public void EnterMainCity() {
        resSvc.AsyncLoadScene("MainScene", () => {
            homeWnd.UpdateHomeWndCoinAndEnergy();
            homeWnd.gameObject.SetActive(true);
        });
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
