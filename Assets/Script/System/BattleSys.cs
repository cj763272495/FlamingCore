using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : MonoBehaviour
{
    public static BattleSys Instance = null;
    public BattleMgr battleMgr;
    private GameRoot gameRoot;
    public BattleWnd battleWnd;


    public void InitSys() {
        Instance = this;
        gameRoot = GameRoot.Instance;
    }

    public void StartBattle(int wave) { 
        GameObject go = new() {
            name = "BattleRoot"
        };
        go.transform.SetParent(gameRoot.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.battleWnd = battleWnd;
        battleMgr.Init(wave);

        battleWnd.battleMgr = battleMgr;
        battleWnd.Init();

        gameRoot.PlayerData.energy--;

    }

    public void ReviveAndContinueBattle() {//玩家重生继续游戏
        battleMgr.ReviveAndContinueBattle();
    }

    public void ClickPauseBtn() {
        battleWnd.ClickPauseBtn();
    }

}
