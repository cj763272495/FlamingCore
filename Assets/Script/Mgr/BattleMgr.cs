using System.Collections; 
using UnityEngine;
using System;  

public class BattleMgr : MonoBehaviour {
    private ResSvc resSvc; 
    private int coin;

    public Camera cam;
    public Vector3 camOriginOffset;

    private PlayerController player;
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

    public event Action<bool> OnStartBattleChanged;
    public ParticleMgr particleMgr;

    public FloatingJoystick joystick;
    public Laser guideLine;

    public int CurWaveIndex { private set; get; }

    private AudioClip hitWallClip;
    private AudioClip deadClip;

    private Coroutine makeGuideLineCoroutine;
     
    public void EarnCoin(int num) {
        coin += num;
    }

    public int GetCoinNum() {
        return coin;
    }

    public void EliminateEnemy() {
        eliminateEnemyNum ++;
    }

    private void HandleStartBattleChanged(bool startBattle) {
        GameRoot.Instance.bgPlayer.audioSource.volume = StartBattle ? 1 : 0.5f;
        if(player) {
            player.gameObject.SetActive(startBattle);
        }
    }

    public void Init(int mapid, Action cb = null) {
        CurWaveIndex = mapid;
        resSvc = ResSvc.Instance;
        hp = 3;
        coin = 0;
        eliminateEnemyNum = 0;
        string waveName = "Level" + mapid;
        battleWnd.hp_txt.text = "x "+ hp;
        particleMgr = gameObject.AddComponent<ParticleMgr>();
        particleMgr.battleMgr = this;
        particleMgr.Init();
        levelData = resSvc.GetMapCfgData(mapid.ToString());

        hitWallClip = ResSvc.Instance.LoadAudio(Constants.HitWallClip,true);
        deadClip = ResSvc.Instance.LoadAudio(Constants.DeadClip);

        if (levelData!=null) {
            resSvc.AsyncLoadScene(waveName, () => {
                LoadPlayer(new Vector3(
                    levelData.PlayerStartPosition.X,
                    levelData.PlayerStartPosition.Y,
                    levelData.PlayerStartPosition.Z));
                SetCameraPositionAndRotation(levelData);
                camOriginOffset = player.transform.position - cam.transform.position;

                foreach(var item in FindObjectsOfType<NormalTurret>()) {
                    item.OnPlayerLoaded();
                }
                if(GameRoot.Instance.gameSettings.bgAudio) {
                    GameRoot.Instance.bgPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.BGGame);
                    GameRoot.Instance.bgPlayer.PlaySound(true);
                }

                guideLine = guideLine == null ? GameObject.FindGameObjectWithTag("GuideLine").GetComponent<Laser>() : guideLine;
                guideLine.gameObject.SetActive(false);
                guideLine.player = player;

                joystick = joystick == null ? GameObject.FindGameObjectWithTag("JoyStick").GetComponent<FloatingJoystick>() : joystick;
                joystick.gameObject.SetActive(true);
                joystick.SetIsShow(GameRoot.Instance.gameSettings.showJoyStick);
                joystick.OnPointerDownAction = OnPointerDown;
                joystick.OnPointerUpAction = OnPointerUp;
                OnStartBattleChanged += HandleStartBattleChanged;
                StartBattle = true;
                if (cb != null) {
                    cb();
                }
            });
        }
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
        // �����ﴦ������������
        guideLine.gameObject.SetActive(false);
        Time.timeScale = 1;
        if(joystick.UpDirection != Vector3.zero) {
            Quaternion rotation = Quaternion.Euler(0,-45,0);
            player.SetDir((rotation * joystick.UpDirection).normalized);
        }
        player.isMove = true;
        player.lastPos = player.transform.position;
    }

    public void PlayHitWallClip() {
        AudioManager.Instance.PlaySound(hitWallClip);
    }

    private void Update() {
        if (!StartBattle) {
            return;
        }
        if (eliminateEnemyNum == 1/* levelData.EnemyNum*/) {// ���ݵ�ǰ����õ����������ж���Ϸ�Ƿ�ʤ��
            player.destructible = false;
            EndBattle(true);
        }
    }

    private void MakeGuideLine() {
        Vector3 direction = (Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal).normalized;
        if(direction == Vector3.zero) {
            direction = player.Dir;
        }
        // �����������ת
        direction = Quaternion.Euler(0,-45,0) * direction;
        // ������ת�Ƕ�
        float angle = Vector3.Angle(Vector3.forward,direction);
        // ������ת��
        Vector3 axis = Vector3.Cross(Vector3.forward,direction);
        // ������Ԫ��
        Quaternion rotation = Quaternion.AngleAxis(angle,axis);
        guideLine.SetDir(rotation * Vector3.forward);
    }
    void SetCameraPositionAndRotation(LevelData levelData) {
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
        string skinId = GameRoot.Instance.PlayerData.cur_skin.ToString();
        string trailId = GameRoot.Instance.PlayerData.cur_trail.ToString();
        player = resSvc.LoadPrefab("Prefab/qiu_" + skinId).GetComponent<PlayerController>();
        GameObject trail = resSvc.LoadPrefab("Prefab/Trails/" + trailId);
        trail.transform.parent = player.transform;
        trail.transform.localScale = Vector3.one;
        trail.transform.localPosition = Vector3.zero;
        player.transform.position = pos;
        player.transform.localEulerAngles = Vector3.zero;
        player.transform.localScale = Vector3.one; 
        player.Init();
        player.battleMgr = this;
        cam = Camera.main;
    }

    public void PauseBattle() {
        StartBattle = false;
        battleWnd.ShowHp(false);
    }

    public void ResumeBattle() {//ȡ����ͣ,�ָ���Ϸ
        battleWnd.StartCountDown3Seconds();
        battleWnd.ShowHp();
        StartCoroutine(EnterLeveL());
    }

    IEnumerator EnterLeveL() {
        yield return new WaitForSecondsRealtime(3f);
        StartBattle = true;
        joystick.IsDown = true;
    }

    public void ReviveAndContinueBattle() {//��������������Ϸ
        Time.timeScale = 0.1f;
        hp--;
        battleWnd.hp_txt.text = "x " + hp;
        LoadPlayer(deadPos);
        player.GetComponent<PlayerController>().Revive();
        foreach(var item in FindObjectsOfType<NormalTurret>()) {
            item.OnPlayerLoaded();
        }
        ResumeBattle();
    }
    public void StratNextLevel() {
        CurWaveIndex++;
        Init(CurWaveIndex);
    }
    public void PlayAgain() {
        Init(CurWaveIndex);
    }

    public void EndBattle(bool isWin, Vector3 contactPoint=new Vector3()) { 
        if (isWin) {
            StartCoroutine(SmoothTransitionToFov());
            Invoke(nameof(GameWin), 1f);
        } else {
            ParticleMgr.Instance.PlayDeadParticle(contactPoint);
            AudioManager.Instance.PlaySound(deadClip);
            StartBattle = false;
            Time.timeScale = 1;
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            if (hp > 0) {//ʣ������ֵ����0���ܸ������
                deadPos = player.transform.position;
                BattleSys.Instance.battleWnd.dead_panel.ShowAndStartCountDown();
                if (GameRoot.Instance.PlayerData.coin < 100) { 
                    BattleSys.Instance.battleWnd.dead_panel.CannotContinueByCoin();
                }
            } else {
                BattleSys.Instance.battleWnd.fail_panel.gameObject.SetActive(true);
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

    private void LevelSettlement() {//�ؿ�����
        GameRoot.Instance.LevelSettlement(coin);
    }
     

    //������ͷ
    IEnumerator SmoothTransitionToFov() {
        float transitionDuration = 1.0f;
        float targetFov = 20f;
        float startTime = Time.time;

        while (Time.time < startTime + transitionDuration) {
            float t = (Time.time - startTime) / transitionDuration;
            // ʹ��EaseIn������ʵ���ȿ������Ч��
            t *= t;// ������Ҫ��������������ı��������
            if (cam) {
                cam.fieldOfView = Mathf.SmoothStep(cam.fieldOfView, targetFov, t);
            }
            yield return null; // �ȴ���һ֡
        }

        // ȷ�����յ�FOV��targetFov
        cam.fieldOfView = targetFov;
    }

    public void DestoryBattle() {
        Destroy(gameObject);
    }

}
