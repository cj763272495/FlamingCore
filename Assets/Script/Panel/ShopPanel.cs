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

    private GameRoot gameRoot;


    private void Start() {
        skinView.gameObject.SetActive(true);
        trailView.gameObject.SetActive(false);
        gameRoot = GameRoot.Instance;
        UpdateScrowViewLockInfo(skinView, gameRoot.PlayerData.skin);
        UpdateScrowViewLockInfo(trailView, gameRoot.PlayerData.trail);
    }

    private void Update() {
        if (selectBuySkin) {
            priceTxt.text = Constants.skinPrice[skinView.CurrentIndex-1].ToString(); 
        } else {
            priceTxt.text = Constants.trailPrice[trailView.CurrentIndex-1].ToString();
        }
        if (int.Parse(priceTxt.text) > GameRoot.Instance.PlayerData.coin) {
            //¸ü¸ÄÍ¼Æ¬
            buyBtn.image.sprite = ResSvc.Instance.LoadSprite("Sprite/bg_btn_small_disable_new");
        }
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
        if (gameRoot.PlayerData.coin < int.Parse(priceTxt.text)) {
            Debug.Log("Óà¶î²»×ã");
            return;
        }
        gameRoot.PlayerData.coin -= int.Parse(priceTxt.text);
        if (selectBuySkin) {
            gameRoot.PlayerData.skin.Add(skinView.CurrentIndex-1);
            UpdateScrowViewLockInfo(skinView, gameRoot.PlayerData.skin);
        } else {
            gameRoot.PlayerData.trail.Add(trailView.CurrentIndex-1);
            UpdateScrowViewLockInfo(trailView, gameRoot.PlayerData.trail);
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
