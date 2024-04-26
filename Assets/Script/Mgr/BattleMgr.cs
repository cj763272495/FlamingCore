using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.UI;

public class BattleMgr : MonoBehaviour {
    private ResSvc resSvc; 
    private int m_coin;
    public Camera cam;
    private GameObject m_player;
    private Vector3 dead_pos;
    public BattleWnd battleWnd;
    private int m_hp;
    private LevelData levelData;
    private int eliminate_enemy_num;
    public bool StartBattle { private set; get; }
    public ParticleMgr particleMgr;

    public void EarnCoin(int num) {
        m_coin += num;
    }

    public int GetCoinNum() {
        return m_coin;
    }

    public void EliminateEnemy() {
        eliminate_enemy_num ++;
    }

    public void Init(int mapid, Action cb = null) {
        resSvc = ResSvc.Instance;
        m_hp = 3; //3条命
        m_coin = 0;
        eliminate_enemy_num = 0;
        string waveName = "Level" + mapid;
        battleWnd.hp_txt.text = "x "+ m_hp;
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
                StartBattle = true;
                battleWnd.ShowHp();
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
        if (eliminate_enemy_num == levelData.EnemyNum) {// 根据当前消灭得敌人数量来判断游戏是否胜利
            m_player.GetComponent<PlayerController>().destructible = false;
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
        m_player = player;
        player.transform.position = pos;
        player.transform.localEulerAngles = Vector3.zero;
        player.transform.localScale = Vector3.one;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Init();
        playerController.battleMgr = this;
        playerController.effectAudioPlayer = GameRoot.Instance.effectAudioPlayer;
        cam = Camera.main;
    }

    public void PauseBattle() {
        Time.timeScale = 0f;
        StartBattle = false;
        battleWnd.ShowHp(false);
    }

    public void ResumeBattle() {//取消暂停恢复游戏
        StartBattle = true;
        battleWnd.ShowHp();
        GameRoot.Instance.gamePause = false;
        battleWnd.joystick.IsDown = true;
    }
    public void ContinueBattle() {//消耗生命继续游戏
        m_hp--;
        battleWnd.hp_txt.text = "x " + m_hp;
        LoadPlayer(dead_pos);
        ResumeBattle();
        StartBattle = true;
        battleWnd.ShowHp();
    }


    public void EndBattle(bool isWin) {

        if (isWin) {
            StartCoroutine(SmoothTransitionToFov());
            Time.timeScale = 0.01f;
            Invoke(nameof(GameWin), 2f);
        } else {
            if (m_hp > 0) {//剩余生命值大于0才能复活继续
                dead_pos = m_player.transform.position;
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
        BattleSys.Instance.battleWnd.win_panel.OpenWinPanel(m_coin);
        PauseBattle();
        LevelSettlement();
    }

    private void LevelSettlement() {//关卡结算
        GameRoot.Instance.LevelSettlement(m_coin);
    }
     

    //拉近镜头
    IEnumerator SmoothTransitionToFov() {
        float transitionDuration = 3.0f;
        float targetFov = 20f;
        //float startTime = Time.time;
        //while (Time.time < startTime + transitionDuration) {
        //    float t = (Time.time - startTime) / transitionDuration;
        //    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, t);
        //    yield return null; // 等待一帧
        //}
        //cam.fieldOfView = 30f; // 确保到达最终视场角
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
