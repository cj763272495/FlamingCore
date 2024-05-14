using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;

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

    private bool startBattle;
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

    public int CurWaveIndex { private set; get; }

    private AudioClip hitWallClip;
    private AudioClip deadClip;

    private Coroutine makeGuideLineCoroutine;

    public void EarnCoin(int num) {
        coin += num;
    }
    public float GetCoin() {
        return coin;
    }

    public void EliminateEnemy() {
        eliminateEnemyNum++;
    }

    private void HandleStartBattleChanged(bool startBattle) {
        gameRoot.bgPlayer.audioSource.volume = StartBattle ? 1 : 0.5f;
        if(player) {
            player.gameObject.SetActive(startBattle);
        }
        if(joystick) {
            joystick.gameObject.SetActive(startBattle);
        }
    }

    public void Init(int mapid,Action cb = null) {
        gameRoot = GameRoot.Instance;
        _pds = PlayersDataSystem.Instance;
        battleSys = BattleSys.Instance;
        resSvc = ResSvc.Instance;

        guideLine = battleWnd.guideLine;
        joystick = battleWnd.joystick;
        CurWaveIndex = mapid;
        hp = 3;
        coin = 0;
        eliminateEnemyNum = 0;
        string waveName = $"Level{mapid}";
        battleWnd.hp_txt.text = $"x {hp}";
        particleMgr = gameObject.AddComponent<ParticleMgr>();
        particleMgr.battleMgr = this;
        particleMgr.Init();
        levelData = resSvc.GetMapCfgData(mapid.ToString());

        hitWallClip = resSvc.LoadAudio(Constants.HitWallClip,true);
        deadClip = resSvc.LoadAudio(Constants.DeadClip);

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

        LoadPlayer(new Vector3(
            levelData.PlayerStartPosition.X,
            levelData.PlayerStartPosition.Y,
            levelData.PlayerStartPosition.Z));
        SetCameraPositionAndRotation(levelData);
        defaultCamOffset = Constants.DefaultCamOffset;
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

    private void Update() {
        if(!StartBattle) {
            return;
        }
        if(eliminateEnemyNum == 1/* levelData.EnemyNum*/) {// 根据当前消灭得敌人数量来判断游戏是否胜利
            player.destructible = false;
            EndBattle(true);
        }
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
    private void SetCameraPositionAndRotation(LevelData levelData) {
        cam.transform.position = new Vector3(
            levelData.CameraOffset.X,
            levelData.CameraOffset.Y,
            levelData.CameraOffset.Z
        );

        cam.transform.eulerAngles = new Vector3(
            levelData.CameraRotation.X,
            levelData.CameraRotation.Y,
            levelData.CameraRotation.Z
        );
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
        cam = Camera.main;
    }

    public void PauseBattle() {
        StartBattle = false;
        battleWnd.ShowHp(false);
    }

    public void ResumeBattle() {//取消暂停,恢复游戏
        battleWnd.StartCountDown3Seconds();
        battleWnd.ShowHp();
        StartCoroutine(EnterLeveL());
    }

    IEnumerator EnterLeveL() {
        yield return new WaitForSecondsRealtime(3f);
        StartBattle = true;
        joystick.IsDown = true;
    }

    public void ReviveAndContinueBattle() {//消耗生命继续游戏
        Time.timeScale = 0.1f;
        hp--;
        battleWnd.hp_txt.text = "x " + hp;
        LoadPlayer(deadPos);
        guideLine.player = player;
        player.GetComponent<PlayerController>().Revive();
        ResumeBattle();
    }
    public void StratNextLevel() {
        CurWaveIndex++;
        Init(CurWaveIndex);
    }
    public void PlayAgain() {
        Init(CurWaveIndex);
    }

    public void EndBattle(bool isWin,Vector3 contactPoint = new Vector3()) {
        if(isWin) {
            StartCoroutine(SmoothTransitionToFov()); 
        } else {
            ParticleMgr.Instance.PlayDeadParticle(contactPoint);
            AudioManager.Instance.PlaySound(deadClip);
            StartBattle = false;
            Time.timeScale = 1;
            new WaitForSeconds(0.5f);
            if(hp > 0) {//剩余生命值大于0才能复活继续
                deadPos = player.transform.position;
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
        battleWnd.ShowHp(false);
        battleWnd.win_panel.OpenWinPanel(coin);
        PauseBattle();
        LevelSettlement();
    }

    private void LevelSettlement() {//关卡结算
        GameRoot.Instance.LevelSettlement(coin);
    }


    //拉近镜头
    IEnumerator SmoothTransitionToFov() {
        if(cam) {
            float transitionDuration = 1.0f;
            float targetFov = 20f;
            float startTime = Time.time;

            while(Time.time < startTime + transitionDuration) {
                float t = (Time.time - startTime) / transitionDuration;
                // 使用EaseIn函数来实现先快后慢的效果
                t *= t;// 根据需要调整这个方程来改变过渡曲线

                cam.fieldOfView = Mathf.SmoothStep(cam.fieldOfView,targetFov,t);
                yield return null; // 等待下一帧
            }

            // 确保最终的FOV是targetFov
            cam.fieldOfView = targetFov;
            GameWin();
        }
    }

    public void DestoryBattle() {
        if(joystick) {
            Destroy(joystick);
        } 
        Destroy(gameObject);
    }
}
