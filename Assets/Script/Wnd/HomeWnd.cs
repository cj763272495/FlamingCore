using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HomeWnd : MonoBehaviour,IPointerDownHandler
{ 
    public BattleWnd battleWnd; 
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private GameObject levelPanel;   // 主页UI面板
    [SerializeField] private GameObject shopPanel;  // 商店UI面板
    [SerializeField] private GameObject settingsPanel; // 设置UI面板

    public GameObject buyEnergyDialog;
    public RectTransform buyList;
    public Toggle tgLevel;
    public Toggle tgShop;
    public Toggle tgSet;

    public GameObject mainShow;

    public class PageChangedEvent : UnityEvent<PageType> {
    }

    public void Init() {
        gameObject.SetActive(true);
        ActivatePanel(levelPanel);
        tgLevel.isOn = true;
        buyEnergyDialog.SetActive(false); 
    }
     
    public void OnPointerDown(PointerEventData eventData) {
        if(!buyList.rect.Contains(eventData.position)&& buyEnergyDialog.activeSelf) {
            buyEnergyDialog.SetActive(false);
        }
    } 

    public void ActivatePanel(GameObject panel) {
        levelPanel.SetActive(panel == levelPanel);
        shopPanel.SetActive(panel == shopPanel);
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
                ActivatePanel(shopPanel);
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
        buyEnergyDialog.SetActive(true);
    }

    public void BuyOneEnergyByCoin() {
        CostCoin(50);
        GameRoot.Instance.EnergyCached += 1;
    }

    public void BuyThreeEnergyByCoin() {
        CostCoin(100);
        GameRoot.Instance.EnergyCached += 3;
    }

    private void CostCoin(int cost) {
        if(cost > GameRoot.Instance.CoinCached) {
            UIManager.Instance.ShowUserMsg("余额不足");
            return;
        }
        GameRoot.Instance.CoinCached -= cost;
    }
}
