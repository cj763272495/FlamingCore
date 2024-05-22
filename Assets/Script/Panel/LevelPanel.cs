using UnityEngine;

public class LevelPanel : MonoBehaviour
{
    private SlideScrollView scrollView;
    private void Start() {
        scrollView = GetComponentInChildren<SlideScrollView>();
        scrollView.Init();
    }

    public void ClickStartBtn() {
        if (PlayersDataSystem.Instance.PlayerData.max_unLock_wave < scrollView.CurrentIndex) {
            return;
        }
        GameRoot.Instance.StartBattle(scrollView.CurrentIndex-1);
    }
}
