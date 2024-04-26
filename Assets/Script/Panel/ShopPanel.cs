using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ShopPanel : MonoBehaviour
{
    public HomeWnd homeWnd;
    public SlideScrollView skinView;
    public SlideScrollView trailView;
    public Text priceTxt;
    private bool selectBuySkin;

    public Button buyBtn;
    public Image buyBtnCoinImag;
    public Text buyButtonCoinTxt;
    public Text equiptTxt;

    public Toggle skintg;
    public Toggle trailtg;

    private GameRoot gameRoot;

    private bool hasBuy;


    private void Start() {
        skinView.gameObject.SetActive(true);
        trailView.gameObject.SetActive(false);
        gameRoot = GameRoot.Instance;
        selectBuySkin = true;
        UpdateScrowViewLockInfo(skinView, gameRoot.PlayerData.skin);
        UpdateScrowViewLockInfo(trailView, gameRoot.PlayerData.trail);
    }

    private void Update() {

        SetPurchaseBtnInfo();
    }

    // 定义一个方法用于设置购买按钮和装备文本的状态
    void SetPurchaseBtnInfo( ) {
        int currentViewIndex;
        int currentPlayerDataIndex;
        if (selectBuySkin) {
            currentViewIndex = skinView.CurrentIndex;
            currentPlayerDataIndex = gameRoot.PlayerData.cur_skin;
            hasBuy = gameRoot.PlayerData.skin.Contains(skinView.CurrentIndex-1);
            priceTxt.text = Constants.skinPrice[currentViewIndex - 1].ToString();
            skintg.Select();
        } else {
            currentViewIndex = trailView.CurrentIndex;
            currentPlayerDataIndex = gameRoot.PlayerData.cur_trail;
            hasBuy = gameRoot.PlayerData.trail.Contains(trailView.CurrentIndex - 1);
            priceTxt.text = Constants.trailPrice[currentViewIndex - 1].ToString();
            trailtg.Select();
        }

        bool isEquipped = currentViewIndex - 1 == currentPlayerDataIndex;
        bool canBuy = GameRoot.Instance.PlayerData.coin > int.Parse(priceTxt.text);
        equiptTxt.gameObject.SetActive(hasBuy);
        equiptTxt.text = isEquipped ? "已装备" : "装备"; 

        SetBuyInfoView(!hasBuy); 
        // 更新购买按钮的sprite，如果当前皮肤或拖尾已装备或无法购买则使用禁用的sprite
        string spriteKey = isEquipped ? "bg_btn_small_disable_new" : "bg_btn_small_normal_new";
        buyBtn.image.sprite = canBuy ? ResSvc.Instance.LoadSprite("Sprite/" + spriteKey): 
            ResSvc.Instance.LoadSprite("Sprite/bg_btn_small_disable_new");
    } 
    private void SetBuyInfoView(bool show) { 
        buyBtnCoinImag.gameObject.SetActive(show);
        buyButtonCoinTxt.gameObject.SetActive(show); 
    }

    public void OpenShopPanel() { 
        gameObject.SetActive(true);
    }

    public void CloseShopPanel() { 
        gameObject.SetActive(true);
    }

    public void SelectBuyCore() {
        selectBuySkin = true;
        skinView.gameObject.SetActive(true); 
        trailView.gameObject.SetActive(false);
    }

    public void SelctBuyTrail() {
        selectBuySkin = false;
        skinView.gameObject.SetActive(false);
        trailView.gameObject.SetActive(true);
    }

    public void ClickBuy() {
        if (!hasBuy) {
            if (gameRoot.PlayerData.coin < int.Parse(priceTxt.text)) {
                Debug.Log("余额不足");
                return;
            }
            gameRoot.PlayerData.coin -= int.Parse(priceTxt.text);
            if (selectBuySkin) {
                gameRoot.PlayerData.skin.Add(skinView.CurrentIndex - 1);
                UpdateScrowViewLockInfo(skinView, gameRoot.PlayerData.skin);
            } else {
                gameRoot.PlayerData.trail.Add(trailView.CurrentIndex - 1);
                UpdateScrowViewLockInfo(trailView, gameRoot.PlayerData.trail);
            }
        } else {
            if (selectBuySkin) {//已经购买直接装备上
                gameRoot.PlayerData.cur_skin = skinView.CurrentIndex - 1;
            } else {
                gameRoot.PlayerData.cur_trail = trailView.CurrentIndex - 1;
            }
        } 
    }

    public void UpdateScrowViewLockInfo(SlideScrollView view, List<int> data) {
        foreach (var num in data) {
            Transform curTrans = view.content.transform.GetChild(num);
            Image imgLock = curTrans.Find("lock").GetComponent<Image>();
            if (imgLock) {
                imgLock.gameObject.SetActive(false);
            }
        }
    }
}
