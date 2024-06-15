using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HomeWnd : MonoBehaviour, IPointerDownHandler
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
    public PageType curPageType; 
    public DOTweenAnimation bottomBtnBroupAni;

    public GameObject shopGroup;
    public Camera cityCam;


    public class PageChangedEvent : UnityEvent<PageType>{
    }
    private void Start() { 
    }

    public void Init() {
        gameObject.SetActive(true);
        ActivatePanel(levelPanel);

        cityCam = Camera.main;
        shopGroup = GameObject.Find("ShopGroup"); 
        ShowCity();

        tgLevel.isOn = true;
        buyEnergyDialog.SetActive(false); 
        bottomBtnBroupAni.DORestart();
        UIManager.Instance.ShowPlayerAssets();

        levelPanel.GetComponent<LevelPanel>().Init();
        settingsPanel.GetComponent<SetPanel>().Init();
        shopPanel.GetComponent<ShopPanel>().Init(); 
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
        curPageType = pageType;
        switch(curPageType) {
            case PageType.Level:
                ActivatePanel(levelPanel);
                break;
            case PageType.Store:
                ActivatePanel(shopPanel);
                break;
            case PageType.Settings:
                ActivatePanel(settingsPanel);
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

    public void ShowCity() {
        cityCam.enabled = true;
        shopGroup.gameObject.SetActive(false);
    }
    public void ShowShopInfo() {
        shopGroup.gameObject.SetActive(true);
        cityCam.enabled = false;
    }
}