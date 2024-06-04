using System; 
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
    public int CurrentViewID {
        get { return _currentViewID; }
        set {
            if(_currentViewID != value) {
                _currentViewID = value;
                OnCurrentViewIDChanged?.Invoke(_currentViewID);
            }
        }
    }

    public event Action<int> OnCurrentViewIDChanged;

    private Sprite disableSprite;
    private Sprite normalSprite;

    public Text descript;
    public ShopPlayer player;
    public GameObject Shopgroup;

    private void Start() {
        skinView.gameObject.SetActive(true);
        trailView.gameObject.SetActive(false);
        pds = PlayersDataSystem.Instance;

        selectBuySkin = true;
        UpdateScrowViewLockInfo(skinView,pds.PlayerData.skin);
        UpdateScrowViewLockInfo(trailView,pds.PlayerData.trail);

        disableSprite = ResSvc.Instance.LoadSprite("Sprite/bg_btn_small_disable_new");
        normalSprite = ResSvc.Instance.LoadSprite("Sprite/bg_btn_small_normal_new");
        OnCurrentViewIDChanged += HandleCurrentViewIDChanged;
        UpdatePurchaseBtnInfo();
    }

    private void OnEnable() {
        Shopgroup.SetActive(true);
        player.Init();
    }
    private void OnDisable() {
        Shopgroup.SetActive(false);
        player.gameObject.SetActive(false);
    }

    private void Update() {
        int newIndex = selectBuySkin ? skinView.CurrentIndex - 1 : trailView.CurrentIndex - 1;
        if(CurrentViewID != newIndex) {
            CurrentViewID = newIndex;
            HandleCurrentViewIDChanged(newIndex);
        }
    }

    void HandleCurrentViewIDChanged(int newViewID) {
        if(selectBuySkin) {
            player.ChangeModes(newViewID);
        } else {
            player.ChangeTrails(newViewID);
        }
        UpdatePurchaseBtnInfo();
    }

    // 设置购买按钮和装备文本的状态
    void UpdatePurchaseBtnInfo() {
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
        bool canBuy = GameRoot.Instance.CoinCached > _curPrice;
        equiptTxt.gameObject.SetActive(_hasBuy);
        equiptTxt.text = _isEquipped ? "已装备" : "装备";

        SetBuyInfoView(!_hasBuy);
        // 更新购买按钮的sprite，如果当前皮肤或拖尾已装备或无法购买则使用禁用的sprite
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

    public void SelectBuyCore() {
        selectBuySkin = true;
        skinView.gameObject.SetActive(true);
        trailView.gameObject.SetActive(false);
        UpdatePurchaseBtnInfo();
    }

    public void SelctBuyTrail() {
        selectBuySkin = false;
        skinView.gameObject.SetActive(false);
        trailView.gameObject.SetActive(true);
        UpdatePurchaseBtnInfo();
    }

    public void ClickBuy() {
        if(!_hasBuy) {
            if(GameRoot.Instance.CoinCached < _curPrice) {
                UIManager.Instance.ShowUserMsg("余额不足");
                return;
            }
            GameRoot.Instance.CoinCached -= _curPrice;
            if(selectBuySkin) {
                pds.PlayerData.skin.Add(_currentViewID);
                UpdateScrowViewLockInfo(skinView, pds.PlayerData.skin);
            } else {
                pds.PlayerData.trail.Add(_currentViewID);
                UpdateScrowViewLockInfo(trailView, pds.PlayerData.trail);
            } 
        } else {
            if(selectBuySkin) {//已经购买直接装备上
                pds.PlayerData.cur_skin = _currentViewID;
            } else {
                pds.PlayerData.cur_trail = _currentViewID;
            }
        }
        pds.SavePlayerData();
        UpdatePurchaseBtnInfo();
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
