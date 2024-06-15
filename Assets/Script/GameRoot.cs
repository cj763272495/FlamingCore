using DG.Tweening;
using System;
using UnityEngine;

public class GameRoot : MonoBehaviour{
    public static GameRoot Instance { get; private set; }
    public SoundPlayer bgPlayer;
    public bool gameStart = false;
    public GameSettings gameSettings;
    public Transform canvasTrans;
    public int CurWaveIndex { private set;  get; }

    private ResSvc resSvc;
    private UIManager uIManager;
    private PlayersDataSystem _pds;
    private int _wave;

    /// <summary>
    /// 金币变化事件
    /// </summary>
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

    /// <summary>
    /// 用户能量变化事件
    /// </summary>
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
        DOTween.Init();
        Init();
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

        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();

        uIManager = UIManager.Instance;
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();
        gameSettings = resSvc.LoadConf();
        uIManager.SetBgAuidoOn(gameSettings.bgAudio);
        uIManager.ShowJoyStick(gameSettings.showJoyStick); 
        bgPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.BGMainCity);

        login.EnterLogin();
    }

    public void GameStart() {
        ClearUIRoot();
        Init();
        PlayBgAudio(uIManager.setPanel.GetBgAudioOn());
    }

    public void StartBattle(int wave) {
        if (_energyCached <= 0) {
            uIManager.ShowUserMsg("能量不足");
            return;
        }
        if(wave >_pds.GetMaxUnLockWave()) {
            return;
        }
        _wave = wave;
        uIManager.homeWnd.bottomBtnBroupAni.DOPlayBackwards();
        uIManager.ShowPlayerAssets(false); 
    }

    public void LevelSettlement() {//关卡结算 
        _pds.SetMaxUnLockWave(CurWaveIndex + 1);
        _pds.SavePlayerData(); 
    }

    public void EnterMainCity(Action loaded=null) {
        resSvc.AsyncLoadScene("MainCity", () => {
            if(loaded != null) {
                loaded();
            }
            Time.timeScale = 1;
            uIManager.homeWnd.Init();
            PlayBgAudio(uIManager.setPanel.GetBgAudioOn());
            uIManager.ShowPlayerAssets();
        });
    }

    public void ExitMainCity() {
        uIManager.homeWnd.gameObject.SetActive(false);
        BattleSys.Instance.StartBattle(_wave);
        EnergyCached--;
        uIManager.ShowImgEnergyDecrease();
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
