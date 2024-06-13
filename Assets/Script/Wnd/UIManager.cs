using DG.Tweening;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager:MonoBehaviour {
    public HomeWnd homeWnd;
    public PausePanel pausePanel;
    public DeadPanel deadPanel;
    public WinPanel winPanel;
    public SetPanel setPanel;
    public StartPanel startPanel;
    public RegLoginWnd regLogInWnd;
    public LoadingWnd loadingWnd;
    public BattleWnd battleWnd;

    public Image blendImg;
    public CanvasGroup canvasGroup;
    public static UIManager Instance { get; private set; }

    public GameObject ImgEnergyDecrease;

    public TextMeshProUGUI coinTxt;
    public GameObject coinChangedShow;
    public TextMeshProUGUI coinChangedTxt;
    public Text energyTxt;

    public GameObject userMessage;
    private TextMeshProUGUI msgTxt;

    private GameRoot gameRoot;

    private void Awake() {
        Instance = this;
        canvasGroup = blendImg.GetComponent<CanvasGroup>();
        msgTxt = userMessage.GetComponentInChildren<TextMeshProUGUI>();
        userMessage.SetActive(false);
    }

    private void Start() {
        gameRoot = GameRoot.Instance;
        coinChangedTxt = coinChangedShow.GetComponentInChildren<TextMeshProUGUI>();
        coinChangedShow.SetActive(false);
        gameRoot.OnCoinChanged += OnCoinChanged;
        gameRoot.OnEnergyChanged += OnEnergyChanged;
    }

    private Tween tween;
    private void OnCoinChanged(int changedNum) {
        coinChangedShow.SetActive(true);
        coinChangedTxt.text = "+ 0";
        coinChangedShow.transform.DOKill();
        DOTween.To(() => 0,x => UpdateCoinText(changedNum,x),Mathf.Abs(changedNum),1f).SetEase(Ease.OutQuint);
        if(tween != null) {
            tween.Complete();
        }
        tween = ToolClass.MoveUpAndFadeOut(coinChangedShow,3,1);
        tween.onComplete += () => {
            coinTxt.text = gameRoot.CoinCached.ToString();
        };
    }
    private void OnEnergyChanged(int totalEnergy) {
        energyTxt.text = totalEnergy.ToString() + "/5";
    }

    private void UpdateCoinText(int changedNum,int value) {
        if(changedNum >= 0)
            coinChangedTxt.text = $"+ {value}";
        else
            coinChangedTxt.text = $"- {value}";
    }

    private void Update() {
        if(Input.touchCount > 0 && userMessage.activeSelf || Input.GetMouseButtonDown(0) && userMessage.activeSelf) {
            userMessage.SetActive(false);
        }
    }

    public void SetBgAuidoOn(bool isOn) {
        setPanel.SetBgAudio(isOn);
    }

    public void ShowJoyStick(bool isOn) {
        setPanel.SetJoyStick(isOn);
    }

    public Tween ShowBlend() {
        return ToolClass.ShowBlendImg(blendImg);
    }

    public Tween FadeIn() {
        canvasGroup.gameObject.SetActive(true);
        blendImg.raycastTarget = true;
        canvasGroup.alpha = 0;
        return canvasGroup.DOFade(1,0.5f).SetUpdate(UpdateType.Normal,true);
    }

    public Tween FadeOut() {
        canvasGroup.alpha = 1;
        return canvasGroup.DOFade(0,1f).SetUpdate(UpdateType.Normal,true).OnComplete(() => {
            blendImg.raycastTarget = false;
        });
    }

    public void ShowImgEnergyDecrease() {
        ImgEnergyDecrease.SetActive(true);
        ToolClass.MoveUpAndFadeOut(ImgEnergyDecrease,0,1);
    }

    private CancellationTokenSource _cts;
    public async void ShowUserMsg(string msg) {
        try {
            msgTxt.text = msg;
            userMessage.SetActive(true);
            if(_cts != null) {
                _cts.Cancel();
            }
            _cts = new CancellationTokenSource();
            await ToolClass.CallAfterDelay(2,() => { userMessage.gameObject.SetActive(false); },_cts);
        } catch(OperationCanceledException) {
            ToolClass.PrintLog("ShowUserMsg 任务被取消。");
        } catch(Exception ex) {
            ToolClass.PrintLog("ShowUserMsg 发生异常: " + ex.Message);
        }
    }

    public void ShowPlayerAssets(bool isShow = true) {
        coinTxt.transform.parent.gameObject.SetActive(isShow);
        energyTxt.transform.parent.gameObject.SetActive(isShow);
        coinTxt.text = gameRoot.CoinCached.ToString();
        energyTxt.text = gameRoot.EnergyCached.ToString();
    }
}
