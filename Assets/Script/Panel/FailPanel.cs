using UnityEngine;

public class FailPanel : MonoBehaviour
{
    
    public void ClickReturnHomeBtn() {
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickTryAgainBtn() {
        BattleSys.Instance.battleMgr.PlayAgain();
        gameObject.SetActive(false);
    }

}
