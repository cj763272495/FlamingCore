using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEditor.Rendering;

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
    public PageType curPageType;

    private DOTweenAnimation dialogAni;

    public class PageChangedEvent : UnityEvent<PageType> {
    }

    public void Init() {
        gameObject.SetActive(true);
        DisActivatePanel(levelPanel);
        tgLevel.isOn = true;
        buyEnergyDialog.SetActive(false);
        dialogAni = buyEnergyDialog.GetComponentInChildren<DOTweenAnimation>();
    }
     
    public void OnPointerDown(PointerEventData eventData) {
        if(!buyList.rect.Contains(eventData.position)&& buyEnergyDialog.activeSelf) {
            dialogAni.onComplete.AddListener(() => {
                buyEnergyDialog.SetActive(false);
                dialogAni.onComplete.RemoveAllListeners();
            });
            dialogAni.DOPlayBackwards();
        }
    } 

    public void DisActivatePanel(GameObject panel, bool showMainCity=true) {
        levelPanel.SetActive(panel == levelPanel);
        shopPanel.SetActive(panel == shopPanel);
        settingsPanel.SetActive(panel == settingsPanel);
        mainShow.SetActive(showMainCity);
    }

    public PageChangedEvent onPageChanged;


    public void PanelTweenComplete() {
        switch(curPageType) {
            case PageType.Level: 
                DisActivatePanel(levelPanel);
                break;
            case PageType.Store:
                DisActivatePanel(shopPanel,false);
                break;
            case PageType.Settings:
                DisActivatePanel(settingsPanel);
                break;
        }
    }


    public void ChangePage(PageType pageType) {
        curPageType = pageType;
        switch(curPageType) {
            case PageType.Level:
                levelPanel.SetActive(true);
                break;
            case PageType.Store:
                shopPanel.SetActive(true);
                break;
            case PageType.Settings:
                settingsPanel.SetActive(true);
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
