using UnityEngine;

public class FailPanel : MonoBehaviour
{
    public void ClickReturnHomeBtn() {
        BattleSys.Instance.battleMgr.DestoryBattle();
        gameObject.SetActive(false);
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickTryAgainBtn() {
        BattleSys.Instance.battleMgr.PlayAgain();
        gameObject.SetActive(false);
    }

}
