using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.UI;

public class BattleMgr : MonoBehaviour {
    private ResSvc resSvc; 
    private int m_coin;
    public Camera cam;
    private bool endBattle;
    private GameObject m_player;
    private Vector3 dead_pos;
    public BattleWnd battleWnd;
    private int m_hp;
    private LevelData levelData;
    private int eliminate_enemy_num;
    public bool startBattle { private set; get; }

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
        m_hp = 3; //3����
        m_coin = 0;
        eliminate_enemy_num = 0;
        string waveName = "Level" + mapid;
        battleWnd.hp_txt.text = "x "+m_hp;
        levelData = resSvc.GetMapCfgData(mapid.ToString());
        if (levelData!=null) {
            resSvc.AsyncLoadScene(waveName, () => {
                LoadPlayer(new Vector3(
                    levelData.playerStartPosition.x,
                    levelData.playerStartPosition.y,
                    levelData.playerStartPosition.z));
                SetCameraPositionAndRotation(levelData);
                startBattle = true;
                battleWnd.ShowHp();
                if (cb != null) {
                    cb();
                }
            });
        }
    }

    private void Update() {
        if (!startBattle) {
            return;
        }
        if (eliminate_enemy_num == levelData.enemyNum) {// ���ݵ�ǰ����õ����������ж���Ϸ�Ƿ�ʤ��
            EndBattle(true);
        }
    }
    void SetCameraPositionAndRotation(LevelData levelData) {
        cam.transform.position = new Vector3(
            levelData.cameraOffset.x,
            levelData.cameraOffset.y,
            levelData.cameraOffset.z
        );

        cam.transform.eulerAngles = new Vector3(
            levelData.cameraRotation.x,
            levelData.cameraRotation.y,
            levelData.cameraRotation.z
        );
    }

    private void LoadPlayer(Vector3 pos) {
        GameObject player = resSvc.LoadPrefab("Prefab/qiu_1");
        m_player = player;
        player.transform.position = pos;
        player.transform.localEulerAngles = Vector3.zero;
        player.transform.localScale = Vector3.one;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Init();
        playerController.battleMgr = this;
        cam = Camera.main;
    }

    public void PauseBattle() {
        Time.timeScale = 0f;
        startBattle = false;
        battleWnd.ShowHp(false);
    }

    public void ResumeBattle() {//ȡ����ͣ�ָ���Ϸ
        startBattle = true;
        battleWnd.ShowHp();
        GameRoot.Instance.gamePause = false;
        battleWnd.joystick.IsDown = true;
    }
    public void ContinueBattle() {//��������������Ϸ
        m_hp--;
        battleWnd.hp_txt.text = "x " + m_hp;
        LoadPlayer(dead_pos);
        ResumeBattle();
        startBattle = true;
        battleWnd.ShowHp();
    }

    public void EndBattle(bool isWin) {
        startBattle = false;
        battleWnd.ShowHp(false);
        if (isWin) {
            StartCoroutine(SmoothTransitionToFov());
            Time.timeScale = 0.5f;
            Invoke("GameWin", 2f);
        } else {
            if (m_hp > 0) {//ʣ������ֵ����0���ܸ������
                dead_pos = m_player.transform.position;
                GameRoot.Instance.battleWnd.dead_panel.ShowAndStartCountDown();
                GameRoot.Instance.battleWnd.dead_panel.CannotContinueByCoin();
            } else {
                GameRoot.Instance.battleWnd.fail_panel.gameObject.SetActive(true);
                LevelSettlement();
            }
        }
    }

    private void LevelSettlement() {//�ؿ�����
        GameRoot.Instance.LevelSettlement(m_coin);
    }

    private void GameWin() {
        GameRoot.Instance.battleWnd.win_panel.OpenWinPanel(m_coin);
        PauseBattle();
        LevelSettlement();
    }

    public void DestoryBattle() {
        Destroy(gameObject);
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
            t = t * t;// ������Ҫ��������������ı��������

            cam.fieldOfView = Mathf.SmoothStep(cam.fieldOfView, targetFov, t); 

            yield return null; // �ȴ���һ֡
        }

        // ȷ�����յ�FOV��targetFov
        cam.fieldOfView = targetFov;
    }
}
