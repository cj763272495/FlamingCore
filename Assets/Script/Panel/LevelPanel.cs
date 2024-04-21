using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanel : MonoBehaviour
{
    public SlideScrollView scrollView;


    public void OpenLevelPanel() {
        gameObject.SetActive(true);
    }

    public void CloseLevelPanel() {
        gameObject.SetActive(true);
    }

    public void ClickStartBtn() {
        GameRoot.Instance.StartBattle(scrollView.currentIndex);
    }
}
