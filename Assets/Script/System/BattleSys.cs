using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : MonoBehaviour
{
    public static BattleSys Instance = null;
    public BattleMgr battleMgr;
    private GameRoot gameRoot;
    public BattleWnd battleWnd;

    public int CurWaveIndex { private set; get; }

    public void InitSys() {
        Instance = this;
        gameRoot = GameRoot.Instance;
    }

    public void StartBattle(int wave) { 
        CurWaveIndex = wave;
        GameObject go = new() {
            name = "BattleRoot"
        };
        go.transform.SetParent(gameRoot.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.battleWnd = battleWnd;
        battleMgr.Init(CurWaveIndex);
        battleWnd.Init();
        gameRoot.PlayerData.energy--;

    }

    public void ContinueBattle() {//玩家重生继续游戏
        battleMgr.ContinueBattle();
    }

    public void ClickPauseBtn() {
        battleWnd.ClickPauseBtn();
    }

}
