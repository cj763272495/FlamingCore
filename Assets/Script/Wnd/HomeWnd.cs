using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HomeWnd : MonoBehaviour
{
    private BattleMgr battleMgr;
    public BattleWnd battleWnd;
    //public FloatingJoystick joystick;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private GameObject levelPanel;   // 主页UI面板
    [SerializeField] private GameObject storePanel;  // 商店UI面板
    [SerializeField] private GameObject settingsPanel; // 设置UI面板
    public Toggle tgLevel;
    public Toggle tgShop;
    public Toggle tgSet;

    public Text coinTxt;
    public Text EnergyTxt;
    public GameObject mainShow;


    public class PageChangedEvent : UnityEvent<PageType> {
    }

    private void Start() {
        ActivatePanel(levelPanel);
        tgLevel.isOn = true;
    }
    public void StartGame() {
        gameObject.SetActive(false);
        GameObject go = new GameObject {
            name = "BattleRoot"
        };
        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        //battleMgr.joystick = joystick;
        battleMgr.Init(1, () => { });
        battleWnd.Init();
    }
    public void ActivatePanel(GameObject panel) {
        levelPanel.SetActive(panel == levelPanel);
        storePanel.SetActive(panel == storePanel);
        settingsPanel.SetActive(panel == settingsPanel);
    }

    public PageChangedEvent onPageChanged;

    public void ChangePage(PageType pageType) {
        switch (pageType) {
            case PageType.Level:
                ActivatePanel(levelPanel);
                break;
            case PageType.Store:
                ActivatePanel(storePanel);
                break;
            case PageType.Settings:
                ActivatePanel(settingsPanel);
                break;
        }
        onPageChanged?.Invoke(pageType);
    }
}
