using UnityEngine;

public class BattleSys : MonoBehaviour
{
    public static BattleSys Instance = null;
    public BattleMgr battleMgr;
    private GameRoot gameRoot;
    public BattleWnd battleWnd;
    public GameObject battleRoot;

    public void InitSys() {
        Instance = this;
        gameRoot = GameRoot.Instance;
    }

    public void StartBattle(int wave) { 
        battleRoot = new() {
            name = "BattleRoot"
        };
        battleRoot.transform.SetParent(gameRoot.transform);
        battleMgr = battleRoot.AddComponent<BattleMgr>();
        battleMgr.battleWnd = battleWnd;
        battleMgr.Init(wave);

        battleWnd.battleMgr = battleMgr;
        battleWnd.Init();

        PlayersDataSystem.Instance.PlayerData.energy--;
    }

    public void ReviveAndContinueBattle() {//玩家重生继续游戏
        battleMgr.ReviveAndContinueBattle();
    }

    public void CleanBattleRoot() {
        foreach(Transform child in battleRoot.transform) {
            child.gameObject.SetActive(false);
        }
    }
}
