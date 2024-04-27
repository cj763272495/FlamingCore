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


    public int CurWaveIndex { private set; get; }

    public void SetBattleStateStart() {
        StartBattle = true;
    }

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
        CurWaveIndex = mapid;
        resSvc = ResSvc.Instance;
        m_hp = 3; //3����
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
        if (eliminate_enemy_num == levelData.EnemyNum) {// ���ݵ�ǰ����õ����������ж���Ϸ�Ƿ�ʤ��
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

    public void ResumeBattle() {//ȡ����ͣ�ָ���Ϸ
        battleWnd.StartCountDown3Seconds();
        battleWnd.ShowHp();
        StartCoroutine(EnterLeveL());
    }

    IEnumerator EnterLeveL() {
        yield return new WaitForSecondsRealtime(3f);
        StartBattle = true; 
        //Time.timeScale = 1;
        GameObject.FindWithTag("JoyStick").GetComponent<FloatingJoystick>().IsDown = true;
    }


    public void ReviveAndContinueBattle() {//��������������Ϸ
        m_hp--;
        battleWnd.hp_txt.text = "x " + m_hp;
        LoadPlayer(dead_pos);
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
            Time.timeScale = 0.01f;
            Invoke(nameof(GameWin), 2f);
        } else {
            if (m_hp > 0) {//ʣ������ֵ����0���ܸ������
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

    private void LevelSettlement() {//�ؿ�����
        GameRoot.Instance.LevelSettlement(m_coin);
    }
     

    //������ͷ
    IEnumerator SmoothTransitionToFov() {
        float transitionDuration = 3.0f;
        float targetFov = 20f;
        //float startTime = Time.time;
        //while (Time.time < startTime + transitionDuration) {
        //    float t = (Time.time - startTime) / transitionDuration;
        //    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, t);
        //    yield return null; // �ȴ�һ֡
        //}
        //cam.fieldOfView = 30f; // ȷ�����������ӳ���
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
