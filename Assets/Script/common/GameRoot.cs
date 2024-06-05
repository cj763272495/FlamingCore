using System;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }

    private ResSvc resSvc;
    private UIManager uIManager; 
    public SoundPlayer bgPlayer;

    public bool gameStart = false;
    public GameSettings gameSettings;
    public Transform canvasTrans;

    public int CurWaveIndex { private set;  get; }
    private PlayersDataSystem _pds;


    public event Action<int> OnCoinChanged;//传入coin变化的值
    private int _coinCached;
    public int CoinCached {
        get { return _coinCached; }
        set {
            if(_coinCached != value) {
                OnCoinChanged?.Invoke(value - _coinCached);
                _coinCached = Math.Min(value, 9999);
                _pds.SetCoin(_coinCached); 
            }
        }
    }

    public event Action<int> OnEnergyChanged;
    private int _energyCached;
    public int EnergyCached { 
        get { return _energyCached; }
        set { 
            if(_energyCached != value) {
                _energyCached = Math.Min(value,99);
                OnEnergyChanged?.Invoke(_energyCached);
                _pds.SetEnergy(_energyCached); 
            }
        } 
    }

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
        //GameStart();
    }

    public void ClearUIRoot() {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++) {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Init() {
        resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        _pds = GetComponent<PlayersDataSystem>();
        _pds.InitSys();

        uIManager = UIManager.Instance;
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();
        gameSettings = resSvc.LoadConf(); 
        UIManager.Instance.SetBgAuidoOn(gameSettings.bgAudio);
        UIManager.Instance.ShowJoyStick(gameSettings.showJoyStick); 
        bgPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.BGMainCity);
        uIManager.startPanel.gameObject.SetActive(true);
    }

    public void GameStart() {
        ClearUIRoot();
        Init();
        PlayBgAudio(UIManager.Instance.setPanel.GetBgAudioOn());
    }

    public void StartBattle(int wave) {
        if (_energyCached < 0) {
            Debug.Log("能量不足");
            return;
        }
        if(wave >_pds.GetMaxUnLockWave()) {
            //关卡未解锁
            return;
        }
        uIManager.homeWnd.gameObject.SetActive(false);
        BattleSys.Instance.StartBattle(wave);
        EnergyCached--;
        uIManager.ShowImgEnergyDecrease();
    }

    public void LevelSettlement(int coin) {//关卡结算
        CoinCached += coin;
        _pds.SetMaxUnLockWave(CurWaveIndex + 1);
        _pds.SavePlayerData(); 
    }

    public void EnterMainCity() { 
        resSvc.AsyncLoadScene("MainCity", () => {
            uIManager.homeWnd.Init();
            uIManager.ShowPlayerAssets();
            PlayBgAudio(UIManager.Instance.setPanel.GetBgAudioOn());
        });
    }

    public void PlayBgAudio(bool isOn) {
        if (isOn) {
            bgPlayer.PlaySound(true);
        } else {
            if (bgPlayer.audioSource.isPlaying) {
                bgPlayer.audioSource.Pause();
            }
        }
    } 

}
