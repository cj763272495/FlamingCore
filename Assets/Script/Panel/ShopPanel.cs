using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ShopPanel:MonoBehaviour {
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

    private PlayersDataSystem pds;

    private bool _hasBuy;
    private bool _isEquipped; 
    private int _curPrice;
    private int _currentViewID;

    private Sprite disableSprite;
    private Sprite normalSprite;

    public Text descript;

    private void Start() {
        skinView.gameObject.SetActive(true);
        trailView.gameObject.SetActive(false);
        pds = PlayersDataSystem.Instance;

        selectBuySkin = true;
        UpdateScrowViewLockInfo(skinView,pds.PlayerData.skin);
        UpdateScrowViewLockInfo(trailView,pds.PlayerData.trail);

        disableSprite = ResSvc.Instance.LoadSprite("Sprite/bg_btn_small_disable_new");
        normalSprite = ResSvc.Instance.LoadSprite("Sprite/bg_btn_small_normal_new");
    }

    private void Update() {
        UpdatePurchaseBtnInfo();
    }

    // ����һ�������������ù���ť��װ���ı���״̬
    void UpdatePurchaseBtnInfo() {
        _currentViewID = selectBuySkin ? skinView.CurrentIndex-1 : trailView.CurrentIndex-1;
        int currentPlayerDataIndex = selectBuySkin ? pds.PlayerData.cur_skin : pds.PlayerData.cur_trail;
        if(selectBuySkin) {
            _hasBuy = pds.PlayerData.skin.Contains(_currentViewID);
            try {
                Constants.CoresInfo.TryGetValue((CoreType)Enum.Parse(typeof(CoreType),_currentViewID.ToString()),out CoreInfo coreinfo);
                _curPrice = coreinfo.price;
                descript.text = coreinfo.descript;
            } catch(Exception ex) {
                Debug.LogError("Exception occurred: " + ex.Message);
            }
        } else {
            _hasBuy = pds.PlayerData.trail.Contains(_currentViewID); 
            _curPrice = Constants.trailPrice[_currentViewID];
        }
        priceTxt.text = _curPrice.ToString();
        if(selectBuySkin) {
            skintg.Select();
        } else {
            trailtg.Select();
        }

        _isEquipped = _currentViewID == currentPlayerDataIndex;
        bool canBuy = pds.PlayerData.coin > _curPrice;
        equiptTxt.gameObject.SetActive(_hasBuy);
        equiptTxt.text = _isEquipped ? "��װ��" : "װ��";

        SetBuyInfoView(!_hasBuy);
        // ���¹���ť��sprite�������ǰƤ������β��װ�����޷�������ʹ�ý��õ�sprite
        if(_hasBuy) {
            if(_isEquipped) {
                buyBtn.image.sprite = disableSprite;
            } else {
                buyBtn.image.sprite = normalSprite;
            } 
            buyBtn.enabled = !_isEquipped;
        } else {
            if(canBuy) {
                buyBtn.image.sprite = normalSprite; 
            } else {
                buyBtn.image.sprite = disableSprite; 
            }
            buyBtn.enabled = canBuy;
        }
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
        if(!_hasBuy) {
            if(pds.PlayerData.coin < _curPrice) {
                Debug.Log("����");
                return;
            }
            pds.PlayerData.coin -= _curPrice;
            if(selectBuySkin) {
                pds.PlayerData.skin.Add(_currentViewID);
                UpdateScrowViewLockInfo(skinView, pds.PlayerData.skin);
            } else {
                pds.PlayerData.trail.Add(_currentViewID);
                UpdateScrowViewLockInfo(trailView, pds.PlayerData.trail);
            }
            PlayersDataSystem.Instance.SavePlayerData();
        } else {
            if(selectBuySkin) {//�Ѿ�����ֱ��װ����
                pds.PlayerData.cur_skin = _currentViewID;
            } else {
                pds.PlayerData.cur_trail = _currentViewID;
            }
        }
    }

    public void UpdateScrowViewLockInfo(SlideScrollView view,List<int> data) {
        foreach(var num in data) {
            Transform curTrans = view.content.transform.GetChild(num);
            Image imgLock = curTrans.Find("lock").GetComponent<Image>();
            if(imgLock) {
                imgLock.gameObject.SetActive(false);
            }
        }
    }
}
