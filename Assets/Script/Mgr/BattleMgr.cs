using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class BattleMgr:MonoBehaviour {
    private ResSvc resSvc;
    private GameRoot gameRoot;
    private PlayersDataSystem _pds;
    private BattleSys battleSys;
    private int coin;

    public Camera cam;
    public Vector3 defaultCamOffset;

    public PlayerController player;
    private Vector3 deadPos;
    public BattleWnd battleWnd;
    private int hp;
    private LevelData levelData;
    private int eliminateEnemyNum;

    //test
    public bool startBattle;
    public bool StartBattle {
        get { return startBattle; }
        private set {
            if(startBattle != value) {
                startBattle = value;
                OnStartBattleChanged?.Invoke(startBattle);
            }
        }
    }

    public Vector3 joyStickDir;

    public event Action<bool> OnStartBattleChanged;
    public ParticleMgr particleMgr;

    public FloatingJoystick joystick;
    public GuideLine guideLine;

    //第一大关0-4，第二大关5-9
    public int CurWaveIndex { private set; get; }//大关卡0++
    public int CurLevelID { private set; get; }//小关卡0-4

    private AudioClip hitWallClip;
    private AudioClip deadClip;

    private Coroutine makeGuideLineCoroutine;
    public float DefaultFov { private set; get; }

    public void EarnCoin(int num) {
        coin += num;
    }
    public float GetCoin() {
        return coin;
    }

    public void EliminateEnemy() {
        eliminateEnemyNum++;
        if(eliminateEnemyNum == 1/*levelData.EnemyNum*/) {
            if(CurLevelID==4) {
                //大关卡结束
                player.destructible = false;
                EndBattle(true);
            } else {
                //小关卡结束
                Time.timeScale = 0.2f;
                player.destructible = false;
                battleWnd.transitionLevelPanel.TransitionToNextLevel(CurWaveIndex,CurLevelID, hp).onComplete += () => {
                    startBattle = false;
                    Time.timeScale = 1;
                    UIManager.Instance.ShowBlend();
                    StratNextLevel();
                    battleWnd.transitionLevelPanel.gameObject.SetActive(false);
                };
            }
        }
    }

    private void HandleStartBattleChanged(bool startBattle) {
        gameRoot.bgPlayer.audioSource.volume = StartBattle ? 1 : 0.5f;
        if(player) {
            player.gameObject.SetActive(startBattle);
        }
        if(joystick) {
            joystick.gameObject.SetActive(startBattle);
        }
        battleWnd.ShowHp(startBattle);
    }

    public void Init(int waveid,int levelid=0,Action cb = null) {
        CurWaveIndex = waveid;
        CurLevelID = levelid;
        if(levelid==0) {//大关开始
            gameRoot = GameRoot.Instance;
            _pds = PlayersDataSystem.Instance;
            battleSys = BattleSys.Instance;
            resSvc = ResSvc.Instance;
            guideLine = battleWnd.guideLine;
            joystick = battleWnd.joystick; 
            hp = 3;
            coin = 0; 
            hitWallClip = resSvc.LoadAudio(Constants.HitWallClip,true);
            deadClip = resSvc.LoadAudio(Constants.DeadClip);
        }
        eliminateEnemyNum = 0;
        string waveName = $"Level{waveid * 5 + levelid}";
        battleWnd.hp_txt.text = $"x {hp}";

        if(gameObject.GetComponent<ParticleMgr>() == null) {
            particleMgr = gameObject.AddComponent<ParticleMgr>();
            particleMgr.battleMgr = this;
            particleMgr.Init();
        }

        levelData = resSvc.GetMapCfgData(waveid.ToString());
        if(levelData != null) {
            resSvc.AsyncLoadScene(waveName, ()=>OnSceneLoaded(cb));
        }
    }
    private void OnSceneLoaded(Action cb) {
        guideLine.gameObject.SetActive(false); 
        joystick.gameObject.SetActive(true);
        joystick.SetIsShow(gameRoot.gameSettings.showJoyStick);
        joystick.OnPointerDownAction = OnPointerDown;
        joystick.OnPointerUpAction = OnPointerUp;
        cam = Camera.main;
        SetCameraData(levelData);
        defaultCamOffset = Constants.DefaultCamOffset;

        LoadPlayer(new Vector3(
            levelData.PlayerStartPosition.X,
            levelData.PlayerStartPosition.Y,
            levelData.PlayerStartPosition.Z));
        if(gameRoot.gameSettings.bgAudio) {
            gameRoot.bgPlayer.clipSource = resSvc.LoadAudio(Constants.BGGame);
            gameRoot.bgPlayer.PlaySound(true);
        }

        OnStartBattleChanged += HandleStartBattleChanged;
        StartBattle = true;
        cb?.Invoke();
    }

    public void OnPointerDown() {
        if(!StartBattle) {
            return;
        }
        Time.timeScale = 0.1f;
        guideLine.gameObject.SetActive(true);
        makeGuideLineCoroutine = StartCoroutine(MakeGuideLineCoroutine());
    }
    private IEnumerator MakeGuideLineCoroutine() {
        while(true) {
            MakeGuideLine();
            yield return null;
        }
    }

    public void OnPointerUp() {
        if(!StartBattle) {
            return;
        }
        if(makeGuideLineCoroutine != null) {
            StopCoroutine(makeGuideLineCoroutine);
        }
        // 在这里处理鼠标或触摸输入
        guideLine.gameObject.SetActive(false);
        Time.timeScale = 1;
        if(joystick.UpDirection != Vector3.zero) { 
            player.SetDir(joystick.UpDirection.normalized);
        }
        player.isMove = true;
        player.lastPos = player.transform.position;
    }

    public void PlayHitWallClip() {
        AudioManager.Instance.PlaySound(hitWallClip);
    }

    private void MakeGuideLine() {
        if(!player) {
            return;
        }
        Vector3 direction = (Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal).normalized;
        if(direction == Vector3.zero) {
            direction = player.Dir;
        }
        joyStickDir = direction;
        guideLine.SetDir(player.transform, direction);
    }
    private void SetCameraData(LevelData levelData) {
        cam.transform.position = new Vector3(
            levelData.CameraOffset.X,
            levelData.CameraOffset.Y,
            levelData.CameraOffset.Z ); 
        cam.transform.eulerAngles = new Vector3(
            levelData.CameraRotation.X,
            levelData.CameraRotation.Y,
            levelData.CameraRotation.Z  );
        cam.fieldOfView = DefaultFov = levelData.CamFOV;
    }

    private void LoadPlayer(Vector3 pos) {
        string skinId = _pds.PlayerData.cur_skin.ToString();
        string trailId = _pds.PlayerData.cur_trail.ToString();
        player = resSvc.LoadPrefab("Prefab/Cores/qiu_" + skinId).GetComponent<PlayerController>();
        GameObject trail = resSvc.LoadPrefab("Prefab/Trails/" + trailId);
        trail.transform.parent = player.transform;
        trail.transform.localScale = Vector3.one;
        trail.transform.localPosition = Vector3.zero;
        player.transform.position = pos;
        player.transform.localEulerAngles = Vector3.zero;
        player.transform.localScale = Vector3.one;
        player.Init();
        player.battleMgr = this;
        joystick.OnPointerDownAction += player.OnPointerDown;
        joystick.OnPointerUpAction += player.OnPointerUp;
    }

    public void PauseBattle() {
        StartBattle = false; 
    }

    public void ResumeBattle() {//取消暂停,恢复游戏
        battleWnd.StartCountDown3Seconds().onComplete+=()=> {
            StartBattle = true;
            joystick.IsDown = true;
        };
        battleWnd.ShowHp(); 
    }

    public void ReviveAndContinueBattle() {//消耗生命继续游戏
        Time.timeScale = 0.1f;
        hp--;
        DG.Tweening.Sequence sequence = DOTween.Sequence().SetUpdate(UpdateType.Normal,true);
        sequence.Append(battleWnd.hp_txt.transform.DOScaleY(0.2f,0.25f));
        sequence.AppendCallback(() => battleWnd.hp_txt.text = "x " + hp);
        sequence.Append(battleWnd.hp_txt.transform.DOScaleY(1f,0.25f)); 
        //DOTween.To(() => hp,x => hp = x,hp,1f).SetUpdate(UpdateType.Normal,true)
        //    .OnUpdate(() => battleWnd.hp_txt.text = "x " + hp);

        LoadPlayer(deadPos);
        guideLine.player = player;
        player.GetComponent<PlayerController>().Revive();
        ResumeBattle();
    }
    public void StratNextWave() {//开始下一大关
        CurWaveIndex++;
        Init(CurWaveIndex);
    }
    public void PlayAgain() {
        Init(CurWaveIndex,CurLevelID);
    }
    public void StratNextLevel() {//开始下一关小关
        CurLevelID++;
        Init(CurWaveIndex,CurLevelID);
    }

    public void EndBattle(bool isWin,Vector3 pos = new Vector3()) {
        if(isWin) { 
            Time.timeScale = 0.2f;
            ToolClass.ChangeCameraFov(cam,20,3f).onComplete+=()=> {
                Time.timeScale = 1;
                GameWin();
            };
            battleWnd.win_panel.ShowClearance();
        } else {
            ParticleMgr.Instance.PlayDeadParticle(pos);
            AudioManager.Instance.PlaySound(deadClip);
            StartBattle = false;
            Time.timeScale = 1;
            if(hp > 0) {//剩余生命值大于0才能复活继续
                deadPos = pos;
                battleSys.battleWnd.dead_panel.ShowAndStartCountDown();
                if(_pds.PlayerData.coin < 100) {
                    battleSys.battleWnd.dead_panel.CannotContinueByCoin();
                }
            } else {
                battleSys.battleWnd.fail_panel.gameObject.SetActive(true);
                LevelSettlement();
            }
        }
    }

    private void GameWin() {
        StartBattle = false;
        battleWnd.win_panel.OpenWinPanel(coin);
        PauseBattle();
        LevelSettlement();
    }

    private void LevelSettlement() {//关卡结算
        GameRoot.Instance.LevelSettlement(coin);
    }
     
    public void DestoryBattle() {
        Destroy(gameObject);
    }
}
