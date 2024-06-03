using UnityEngine;

public class FailPanel : MonoBehaviour
{
    public void ClickReturnHomeBtn() {
        gameObject.SetActive(false);
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickTryAgainBtn() {
        BattleSys.Instance.battleMgr.PlayAgain();
        gameObject.SetActive(false);
    }

}
