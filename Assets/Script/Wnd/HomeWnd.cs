using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HomeWnd : MonoBehaviour
{ 
    public BattleWnd battleWnd; 
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private GameObject levelPanel;   // ��ҳUI���
    [SerializeField] private GameObject storePanel;  // �̵�UI���
    [SerializeField] private GameObject settingsPanel; // ����UI���
    public Toggle tgLevel;
    public Toggle tgShop;
    public Toggle tgSet;

    public Text coinTxt;
    public Text energyTxt;
    public GameObject mainShow;
    private PlayersDataSystem pds;


    public class PageChangedEvent : UnityEvent<PageType> {
    }

    public void Init() {
        pds = PlayersDataSystem.Instance;
        gameObject.SetActive(true);
        ActivatePanel(levelPanel);
        tgLevel.isOn = true;
        UpdateHomeWndCoinAndEnergy();
    }

    public void UpdateHomeWndCoinAndEnergy() { 
        coinTxt.text = pds.PlayerData.coin.ToString();
        energyTxt.text = pds.PlayerData.energy.ToString();
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
                mainShow.SetActive(true);
                break;
            case PageType.Store:
                ActivatePanel(storePanel);
                mainShow.SetActive(false);
                break;
            case PageType.Settings:
                ActivatePanel(settingsPanel);
                mainShow.SetActive(true);
                break;
        }
        onPageChanged?.Invoke(pageType);
    }

    public void ClickAddEnergy() {
        Debug.Log("Add energy");
    }
}
