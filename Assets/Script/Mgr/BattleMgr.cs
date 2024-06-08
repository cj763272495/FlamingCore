using System.Collections;
using UnityEngine;
using System; 
using DG.Tweening;

public class BattleMgr:MonoBehaviour {
    private ResSvc resSvc;
    private GameRoot gameRoot; 
    public StateMgr stateMgr;
    private PlayersDataSystem _pds;
    private BattleSys battleSys;
    private int coinGot;

    public Camera cam;
    public Vector3 defaultCamOffset;

    public PlayerController player;
    private Vector3 deadPos;
    public BattleWnd battleWnd;
    private int hp;
    private LevelData levelData;
    private int eliminateEnemyNum;
    public bool isRobotLevel=false;
    
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

    //ÿһ�����5��С�ؿ�����С�ؿ���5���ǻ�����ģʽ
    public int CurWaveIndex { private set; get; }//��ؿ�0++
    public int CurLevelID { private set; get; }//С�ؿ�0-4

    private AudioClip hitWallClip;
    private AudioClip deadClip;

    private Coroutine makeGuideLineCoroutine;
    public float DefaultFov { private set; get; }

    public void EarnCoin(int num) {
        coinGot += num;
    }
    public int GetCoinNum() {
        return coinGot;
    }

    public void EliminateEnemy() {
        eliminateEnemyNum++;
        if(eliminateEnemyNum ==  levelData.EnemyNum) {
            //EndBattle(true);
            //return;
            if(CurLevelID==4) {
                //��ؿ�����
                EndBattle(true);
                player.destructible = false;
            } else {
                //С�ؿ�����
                Time.timeScale = 0.2f;
                player.destructible = false;
                battleWnd.transitionLevelPanel.TransitionToNextLevel(CurWaveIndex,CurLevelID, hp).onComplete += () => {
                    StartBattle = false;
                    Time.timeScale = 1;
                    UIManager.Instance.ShowBlend();
                    StratNextLevel();
                    battleWnd.transitionLevelPanel.gameObject.SetActive(false);
                };
            }
        }
    }

    public void HandleStartBattleChanged(bool startBattle) {
        gameRoot.bgPlayer.audioSource.volume = StartBattle ? 1 : 0.5f;
        if(player) {
            player.gameObject.SetActive(startBattle);
        }
        if(joystick) {
            joystick.gameObject.SetActive(startBattle);
        }
        battleWnd.ShowHp(startBattle);
        Time.timeScale = startBattle ? 1 : 0;
    }

    public void Init(int waveid,int levelid=0, Action cb = null) { 
        gameRoot = GameRoot.Instance;
        _pds = PlayersDataSystem.Instance;
        battleSys = BattleSys.Instance;
        resSvc = ResSvc.Instance;
        guideLine = battleWnd.guideLine;
        joystick = battleWnd.joystick;
        hitWallClip = resSvc.LoadAudio(Constants.HitWallClip,true);
        deadClip = resSvc.LoadAudio(Constants.DeadClip);

        if(!gameObject.GetComponent<ParticleMgr>()) {
            particleMgr = gameObject.AddComponent<ParticleMgr>();
            particleMgr.Init(this);
        }
        if(!gameObject.GetComponent<StateMgr>()) { 
            stateMgr = gameObject.AddComponent<StateMgr>();
            stateMgr.Init();
        }
        if(gameRoot.gameSettings.bgAudio) {
            gameRoot.bgPlayer.clipSource = resSvc.LoadAudio(Constants.BGGame);
            gameRoot.bgPlayer.PlaySound(true);
        }
        StartLevel(waveid, levelid, cb);
    }

    private bool StartLevel(int waveid,int levelid = 0, Action cb = null) {
        battleSys.CleanBattleRoot();
        string waveName = $"Level{waveid * 5 + levelid}";
        levelData = resSvc.GetMapCfgData(waveName);
        if(levelData == null) {
            UIManager.Instance.ShowUserMsg("����������,�����ڴ�");
            return false;
        }
        eliminateEnemyNum = 0;
        CurWaveIndex = waveid;
        CurLevelID = levelid;
        if(levelid==0) {
            hp = 3;
            coinGot = 0;
        }

        battleWnd.hp_txt.text = $"x {hp}";
        isRobotLevel = levelid != 0 && levelid % 4 == 0;
        //isRobotLevel = true; 
        resSvc.AsyncLoadScene(waveName,() => OnSceneLoaded(cb));
        return true;
    }

    private void OnSceneLoaded(Action cb) {
        guideLine.gameObject.SetActive(false);
        joystick.gameObject.SetActive(true);
        joystick.SetIsShow(gameRoot.gameSettings.showJoyStick);
        joystick.OnPointerDownAction = OnPointerDown;
        joystick.OnPointerUpAction = OnPointerUp;
        cam = Camera.main;

        LoadPlayer(new Vector3(
            levelData.PlayerStartPosition.X,
            levelData.PlayerStartPosition.Y,
            levelData.PlayerStartPosition.Z));

        SetCameraData(levelData);
        StartBattle = true;
        cb?.Invoke();
    }

    public void EnterBulletTime() {
        Time.timeScale = 0.1f;
        guideLine.gameObject.SetActive(true);
        makeGuideLineCoroutine = StartCoroutine(MakeGuideLineCoroutine());
    }

    public void OnPointerDown() {
        if(!StartBattle) {
            return;
        }
        if(isRobotLevel) {
            Time.timeScale = 1f;
            player.isMove = true; 
        } else {
            Time.timeScale = 0.1f;
            guideLine.gameObject.SetActive(true);
            makeGuideLineCoroutine = StartCoroutine(MakeGuideLineCoroutine());
        }
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
        if(isRobotLevel) {
            Time.timeScale = 0.2f;
            player.isMove = false;
        } else {
            if(makeGuideLineCoroutine != null) {
                StopCoroutine(makeGuideLineCoroutine);
            }
            guideLine.gameObject.SetActive(false);
            Time.timeScale = 1;
            if(joystick.UpDirection != Vector3.zero) {
                player.SetDir(joystick.UpDirection.normalized);
            }
            player.isMove = true;
            player.lastPos = player.transform.position;
        }
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
        Vector3 camRotationSet = new Vector3(
            levelData.CameraRotation.X,
            levelData.CameraRotation.Y,
            levelData.CameraRotation.Z  );
        cam.transform.eulerAngles = camRotationSet;
        cam.fieldOfView = DefaultFov = levelData.CamFOV;
        defaultCamOffset = Constants.DefaultCamOffset;
        cam.transform.position = player.transform.position + defaultCamOffset;

        if(isRobotLevel) {
            Sequence seq = DOTween.Sequence().SetUpdate(UpdateType.Normal,true);
            seq.AppendInterval(0.3f);
            seq.AppendCallback(() => { 
                Time.timeScale = 0.5f;
                ToolClass.ChangeCameraFov(cam,10,0.5f).SetEase(Ease.InOutSine);
            });
            //seq.AppendCallback(() => {
            //    Vector3[] circlePoints = CalculateArcPoints(cam.transform,player.transform.position,90);
            //    cam.transform.DOPath(circlePoints,2,PathType.CatmullRom)
            //        //.SetLookAt(player.transform)
            //        .SetEase(Ease.InOutSine);
            //});
            seq.AppendInterval(2f);
            seq.AppendCallback(() => {
                Time.timeScale = 1;
                cam.transform.eulerAngles = camRotationSet;
                ToolClass.ChangeCameraFov(cam,DefaultFov,0.7f).SetEase(Ease.InOutSine);
            });
            seq.Play();
        }
    }

    private void LoadPlayer(Vector3 pos) {
        if(isRobotLevel) {
            string robotName = levelData.RobotName;
            player = resSvc.LoadPrefab("Prefab/Robot/" + robotName).GetComponent<PlayerController>();
        } else {
            string skinId = _pds.PlayerData.cur_skin.ToString();
            string trailId = _pds.PlayerData.cur_trail.ToString();
            player = resSvc.LoadPrefab("Prefab/Cores/qiu_" + skinId).GetComponent<PlayerController>();
            GameObject trail = resSvc.LoadPrefab("Prefab/Trails/" + trailId); 
            trail.transform.parent = player.transform;
            trail.transform.localScale = Vector3.one;
            trail.transform.localPosition = Vector3.zero;
        }

        player.transform.position = pos;
        player.transform.localEulerAngles = Vector3.zero;
        player.transform.localScale = Vector3.one;
        player.Init(this, stateMgr);
        EventManager.PlayerLoaded(player);
        joystick.OnPointerDownAction += player.OnPointerDown;
        joystick.OnDragAction = player.OnDrag;
        joystick.OnPointerUpAction += player.OnPointerUp;
    }

    public void PauseBattle() {
        StartBattle = false; 
    }

    public void ResumeBattle() {//ȡ����ͣ,�ָ���Ϸ
        if(player) {
            player.gameObject.SetActive(true);
        }
        battleWnd.StartCountDown3Seconds().onComplete+=()=> {
            StartBattle = true;
            EnterBulletTime();
        };
    }

    public void ReviveAndContinueBattle() {//��������������Ϸ
        Time.timeScale = 0.1f;
        hp--;
        Sequence sequence = DOTween.Sequence().SetUpdate(UpdateType.Normal,true);
        sequence.Append(battleWnd.hp_txt.transform.DOScaleY(0.2f,0.25f));
        sequence.AppendCallback(() => battleWnd.hp_txt.text = "x " + hp);
        sequence.Append(battleWnd.hp_txt.transform.DOScaleY(1f,0.25f));

        LoadPlayer(deadPos);
        guideLine.player = player;
        player.GetComponent<PlayerController>().Revive();
        ResumeBattle();
    }
    public bool StratNextWave() {//��ʼ��һ���
        CurWaveIndex++;
        if(!StartLevel(CurWaveIndex)) {
            return false;
        }
        gameRoot.EnergyCached--;
        UIManager.Instance.ShowImgEnergyDecrease();
        return true;
    }
    public void PlayAgain(int levelID=-1) { 
        StartLevel(CurWaveIndex, levelID==-1? CurLevelID:levelID); 
    }
    public void StratNextLevel() {//��ʼ��һ��С��
        CurLevelID++;
        StartLevel(CurWaveIndex,CurLevelID);
    }

    public void EndBattle(bool isWin, Vector3 pos = new Vector3()) {
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
            joystick.OnPointerDownAction -= player.OnPointerDown;
            joystick.OnDragAction -= player.OnDrag;
            joystick.OnPointerUpAction -= player.OnPointerUp;
            StartBattle = false;
            Time.timeScale = 1;
            deadPos = pos;
            battleSys.battleWnd.dead_panel.ShowAndStartCountDown();
            UIManager.Instance.deadPanel.ShowContinueBtn(hp > 0);
            if(hp <= 0) {  
                battleSys.battleWnd.dead_panel.SetContinueByCoinBtn(gameRoot.CoinCached >= 80); 
            }
        }
    }

    private void GameWin() {
        StartBattle = false;
        battleWnd.win_panel.OpenWinPanel(coinGot);
        PauseBattle();
        LevelSettlement();
    }

    public void LevelSettlement() {//�ؿ�����
        GameRoot.Instance.LevelSettlement(coinGot);
    }
     
    public void DestoryBattle() {
        Destroy(gameObject);
    }

    public int GetHP() {
           return hp;
    }
}
