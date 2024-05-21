using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    //��������
    //private void Start() {
    //    ClearUIRoot();
    //    Init();
    //    GameStart();
    //}

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
    }

    public void GameStart() {
        ClearUIRoot();
        Init();
        PlayBgAudio(UIManager.Instance.setPanel.GetBgAudioOn());
    }

    public void StartBattle(int wave) {
        if (_pds.PlayerData.energy<0) {
            Debug.Log("��������");
            return;
        }
        if(wave >_pds.PlayerData.max_unLock_wave) {
            //�ؿ�δ����
            return;
        }
        uIManager.homeWnd.gameObject.SetActive(false);
        BattleSys.Instance.StartBattle(wave);
        uIManager.ShowImgEnergyDecrease();
    }

    public void LevelSettlement(int coin) {//�ؿ�����
        _pds.PlayerData.coin += coin;
        _pds.PlayerData.max_unLock_wave = CurWaveIndex + 1;
        _pds.SavePlayerData(); 
    }

    public void EnterMainCity() {
        resSvc.AsyncLoadScene("MainCity", () => {
            uIManager.homeWnd.Init();
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
