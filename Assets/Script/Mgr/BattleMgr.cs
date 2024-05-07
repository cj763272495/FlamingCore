using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.UI;

public class BattleMgr : MonoBehaviour {
    private ResSvc resSvc; 
    private int coin;
    public Camera cam;
    private GameObject player;
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
        } }

    public event Action<bool> OnStartBattleChanged;
    public ParticleMgr particleMgr;

    public FloatingJoystick joystick;
    public Laser guideLine;

    public int CurWaveIndex { private set; get; }

    public void EarnCoin(int num) {
        coin += num;
    }

    public int GetCoinNum() {
        return coin;
    }

    public void EliminateEnemy() {
        eliminateEnemyNum ++;
    }

    public void Init(int mapid, Action cb = null) {
        CurWaveIndex = mapid;
        resSvc = ResSvc.Instance;
        hp = 3; //3条命
        coin = 0;
        eliminateEnemyNum = 0;
        string waveName = "Level" + mapid;
        battleWnd.hp_txt.text = "x "+ hp;
        particleMgr = gameObject.AddComponent<ParticleMgr>();
        particleMgr.battleMgr = this;
        particleMgr.Init();
        levelData = resSvc.GetMapCfgData(mapid.ToString());
        if (levelData!=null) {
            resSvc.AsyncLoadScene(waveName, () => {
                LoadPlayer(new Vector3(
                    levelData.PlayerStartPosition.X,
                    levelData.PlayerStartPosition.Y,
                    levelData.PlayerStartPosition.Z));
                SetCameraPositionAndRotation(levelData);
                foreach(var item in FindObjectsOfType<NormalTurret>()) {
                    item.OnPlayerLoaded();
                }
                if(GameRoot.Instance.gameSettings.bgAudio) {
                    GameRoot.Instance.bgPlayer.clipSource = ResSvc.Instance.LoadAudio(Constants.BGGame);
                    GameRoot.Instance.bgPlayer.PlaySound(true);
                }
                StartBattle = true;
                if (cb != null) {
                    cb();
                }
            });
        }
    }

    private void Update() {
        if (!StartBattle) {
            return;
        }
        GameRoot.Instance.bgPlayer.audioSource.volume = StartBattle? 1:0.5f;
        if (eliminateEnemyNum == 1/* levelData.EnemyNum*/) {// 根据当前消灭得敌人数量来判断游戏是否胜利
            player.GetComponent<PlayerController>().destructible = false;
            EndBattle(true);
        }
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
        GameObject player = resSvc.LoadPrefab("Prefab/qiu_" + skinId);
        GameObject trail = resSvc.LoadPrefab("Prefab/Trails/" + trailId);
        trail.transform.parent = player.transform;
        trail.transform.localScale = Vector3.one;
        trail.transform.localPosition = Vector3.zero;
        player.transform.position = pos;
        player.transform.localEulerAngles = Vector3.zero;
        player.transform.localScale = Vector3.one;
        PlayerController playerController = player.GetComponent<PlayerController>();

        guideLine = guideLine==null? GameObject.FindGameObjectWithTag("GuideLine").GetComponent<Laser>():guideLine; 
        playerController.guideLine = guideLine; 

        joystick = joystick == null ? GameObject.FindGameObjectWithTag("JoyStick").GetComponent<FloatingJoystick>() : joystick;
        playerController.joystick = joystick;

        playerController.Init();
        playerController.battleMgr = this;
        cam = Camera.main;
        this.player = player;
    }

    public void PauseBattle() {
        //Time.timeScale = 0f;
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
        GameObject.FindWithTag("JoyStick").GetComponent<FloatingJoystick>().IsDown = true;
    }

    public void ReviveAndContinueBattle() {//消耗生命继续游戏
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

    public void EndBattle(bool isWin) { 
        if (isWin) {
            StartCoroutine(SmoothTransitionToFov());
            Invoke(nameof(GameWin), 1f);
        } else {
            StartBattle = false;
            Time.timeScale = 1;
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            if (hp > 0) {//剩余生命值大于0才能复活继续
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

    private void LevelSettlement() {//关卡结算
        GameRoot.Instance.LevelSettlement(coin);
    }
     

    //拉近镜头
    IEnumerator SmoothTransitionToFov() {
        float transitionDuration = 1.0f;
        float targetFov = 20f;
        float startTime = Time.time;

        while (Time.time < startTime + transitionDuration) {
            float t = (Time.time - startTime) / transitionDuration;
            // 使用EaseIn函数来实现先快后慢的效果
            t *= t;// 根据需要调整这个方程来改变过渡曲线
            if (cam) {
                cam.fieldOfView = Mathf.SmoothStep(cam.fieldOfView, targetFov, t);
            }
            yield return null; // 等待下一帧
        }

        // 确保最终的FOV是targetFov
        cam.fieldOfView = targetFov;
    }

    public void DestoryBattle() {
        Destroy(gameObject);
    }

}
